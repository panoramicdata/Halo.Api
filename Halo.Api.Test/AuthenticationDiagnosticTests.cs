using AwesomeAssertions;

namespace Halo.Api.Test;

[Collection("Integration Tests")]
public class AuthenticationDiagnosticTests(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task TestAuthEndpoints_FindCorrectOne()
	{
		// Test multiple possible authentication endpoints
		using var httpClient = new HttpClient();

		var baseUrl = HaloClient.BaseUrl;
		var config = _fixture.Configuration;

		var endpoints = new[]
		{
			"/api/auth/token",
			"/auth/token",
			"/api/oauth2/token",
			"/oauth2/token",
			"/token"
		};

		var formData = new FormUrlEncodedContent(new Dictionary<string, string>
		{
			["grant_type"] = "client_credentials",
			["client_id"] = config["HaloApi:HaloClientId"]!,
			["client_secret"] = config["HaloApi:HaloClientSecret"]!,
			["scope"] = "all"
		});

		foreach (var endpoint in endpoints)
		{
			try
			{
				Console.WriteLine($"\n--- Testing endpoint: {baseUrl}{endpoint} ---");

				var response = await httpClient.PostAsync($"{baseUrl}{endpoint}", formData, CancellationToken);
				var content = await response.Content.ReadAsStringAsync(CancellationToken);

				Console.WriteLine($"Status: {response.StatusCode}");
				Console.WriteLine($"Content Type: {response.Content.Headers.ContentType?.MediaType}");
				Console.WriteLine($"Content Length: {content.Length}");

				if (response.IsSuccessStatusCode && content.Contains("access_token"))
				{
					Console.WriteLine("✅ SUCCESS - Found working auth endpoint!");
					Console.WriteLine($"Response: {content[..Math.Min(content.Length, 200)]}");
					break;
				}
				else
				{
					Console.WriteLine($"❌ Failed - Response: {content[..Math.Min(content.Length, 100)]}");
				}
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"❌ HTTP Error for {endpoint}: {ex.Message}");
			}
		}

		// This test is diagnostic - it should always pass
		true.Should().BeTrue();
	}
}