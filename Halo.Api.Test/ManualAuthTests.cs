using AwesomeAssertions;
using System.Text.Json;

namespace Halo.Api.Test;

[Collection("Integration Tests")]
public class ManualAuthTests(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task ManualAuth_TestCompleteFlow()
	{
		// Manually implement the complete auth + API call flow to isolate issues
		using var httpClient = new HttpClient();

		var baseUrl = HaloClient.BaseUrl;
		var config = _fixture.Configuration;

		// Step 1: Get authentication token
		var authUrl = $"{baseUrl}/auth/token";
		var formData = new FormUrlEncodedContent(new Dictionary<string, string>
		{
			["grant_type"] = "client_credentials",
			["client_id"] = config["HaloApi:HaloClientId"]!,
			["client_secret"] = config["HaloApi:HaloClientSecret"]!,
			["scope"] = "all"
		});

		var authResponse = await httpClient.PostAsync(authUrl, formData, CancellationToken);
		var authContent = await authResponse.Content.ReadAsStringAsync(CancellationToken);

		Console.WriteLine($"Auth URL: {authUrl}");
		Console.WriteLine($"Auth Status: {authResponse.StatusCode}");
		Console.WriteLine($"Auth Content: {authContent}");

		if (!authResponse.IsSuccessStatusCode)
		{
			// Authentication failed - this tells us the exact issue
			throw new Exception($"Authentication failed: {authResponse.StatusCode} - {authContent}");
		}

		// Step 2: Parse the token
		var tokenDoc = JsonDocument.Parse(authContent);
		var accessToken = tokenDoc.RootElement.GetProperty("access_token").GetString();

		Console.WriteLine($"Access Token: {accessToken?[..20]}...");

		// Step 3: Use the token to call the API
		httpClient.DefaultRequestHeaders.Authorization =
			new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

		var apiUrl = $"{baseUrl}/api/tickets";
		var apiResponse = await httpClient.GetAsync(apiUrl, CancellationToken);
		var apiContent = await apiResponse.Content.ReadAsStringAsync(CancellationToken);

		Console.WriteLine($"API URL: {apiUrl}");
		Console.WriteLine($"API Status: {apiResponse.StatusCode}");
		Console.WriteLine($"API Content Length: {apiContent.Length}");
		Console.WriteLine($"API Content Preview: {apiContent[..Math.Min(apiContent.Length, 200)]}");

		// This will tell us exactly what's happening
		apiResponse.IsSuccessStatusCode.Should().BeTrue($"API call failed: {apiResponse.StatusCode} - {apiContent}");
	}
}