using System.Text.Json;

namespace Halo.Api.Test;

/// <summary>
/// Simple command-line test to verify Halo API credentials and endpoints
/// This class provides diagnostic utilities for testing API connectivity
/// </summary>
public class Program
{
	// Note: Entry point removed to avoid conflict with test framework's auto-generated Main method
	
	public static async Task TestHaloApiCredentials()
	{
		Console.WriteLine("?? Testing Halo API Credentials...");
		Console.WriteLine("=====================================");
		
		// Using the exact credentials from user secrets
		var haloAccount = "panoramicdlsandbox";
		var clientId = "0c743b18-d9fd-4bc8-b08d-ca9d22b12d63";
		var clientSecret = "055114b9-3997-4bf9-9811-56431776c7af-cf94ffcc-e451-4a5b-8619-f4536e4681dc";
		
		var baseUrl = $"https://{haloAccount}.halopsa.com";
		Console.WriteLine($"?? Base URL: {baseUrl}");
		Console.WriteLine($"?? Client ID: {clientId}");
		Console.WriteLine($"?? Client Secret: {clientSecret[..20]}...");
		Console.WriteLine();
		
		using var httpClient = new HttpClient();
		
		// Test 1: Check if the base URL is reachable
		Console.WriteLine("?? Test 1: Checking base URL accessibility...");
		try
		{
			var pingResponse = await httpClient.GetAsync(baseUrl);
			Console.WriteLine($"   Status: {pingResponse.StatusCode}");
			Console.WriteLine($"   Headers: {string.Join(", ", pingResponse.Headers.Select(h => h.Key).Take(5))}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"   ? Failed to reach base URL: {ex.Message}");
			return;
		}
		
		// Test 2: Try authentication
		Console.WriteLine($"\n?? Test 2: Testing authentication...");
		
		var formData = new FormUrlEncodedContent(new Dictionary<string, string>
		{
			["grant_type"] = "client_credentials",
			["client_id"] = clientId,
			["client_secret"] = clientSecret,
			["scope"] = "all"
		});
		
		try
		{
			var authUrl = $"{baseUrl}/auth/token";
			var authResponse = await httpClient.PostAsync(authUrl, formData);
			var authContent = await authResponse.Content.ReadAsStringAsync();
			
			Console.WriteLine($"   Status: {authResponse.StatusCode}");
			Console.WriteLine($"   Content-Type: {authResponse.Content.Headers.ContentType?.MediaType}");
			Console.WriteLine($"   Content Length: {authContent.Length}");
			
			if (authResponse.IsSuccessStatusCode)
			{
				Console.WriteLine("   ? SUCCESS! Authentication worked!");
				
				// Try to parse as JSON
				try
				{
					var json = JsonDocument.Parse(authContent);
					if (json.RootElement.TryGetProperty("access_token", out var tokenElement))
					{
						var token = tokenElement.GetString();
						Console.WriteLine($"   ?? Got access token: {token?[..20]}...");
						
						// Test 3: Try to use the token
						await TestApiWithToken(httpClient, baseUrl, token!);
						return; // Success, no need to test other endpoints
					}
				}
				catch (JsonException)
				{
					Console.WriteLine($"   ??  Response is not JSON: {authContent[..Math.Min(200, authContent.Length)]}");
				}
			}
			else
			{
				Console.WriteLine($"   ? Failed: {authContent[..Math.Min(500, authContent.Length)]}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"   ? Exception: {ex.Message}");
		}
	}
	
	private static async Task TestApiWithToken(HttpClient httpClient, string baseUrl, string accessToken)
	{
		Console.WriteLine("\n?? Test 3: Testing API endpoints with token...");
		
		httpClient.DefaultRequestHeaders.Authorization = 
			new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
		
		var apiEndpoints = new[] { "/api/tickets", "/api/ticket", "/tickets", "/ticket" };
		
		foreach (var endpoint in apiEndpoints)
		{
			try
			{
				var apiUrl = $"{baseUrl}{endpoint}";
				Console.WriteLine($"   Testing: {apiUrl}");
				
				var apiResponse = await httpClient.GetAsync(apiUrl);
				var apiContent = await apiResponse.Content.ReadAsStringAsync();
				
				Console.WriteLine($"   Status: {apiResponse.StatusCode}");
				
				if (apiResponse.IsSuccessStatusCode)
				{
					Console.WriteLine("   ? API endpoint works!");
					Console.WriteLine($"   Response preview: {apiContent[..Math.Min(200, apiContent.Length)]}");
					return;
				}
				else
				{
					Console.WriteLine($"   ? Failed: {apiResponse.StatusCode}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"   ? Exception: {ex.Message}");
			}
		}
	}
}