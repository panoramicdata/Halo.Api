using AwesomeAssertions;

namespace Halo.Api.Test;

[Collection("Integration Tests")]
public class DiagnosticTests(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task RawHttpClient_CheckApiEndpoint()
	{
		// Test raw HTTP access to see what we actually get back
		using var httpClient = new HttpClient();

		var baseUrl = HaloClient.BaseUrl;
		Console.WriteLine($"Testing raw HTTP access to: {baseUrl}");
		Logger.LogInformation("Testing raw HTTP access to: {BaseUrl}", baseUrl);

		try
		{
			var response = await httpClient.GetAsync($"{baseUrl}/api/Tickets", CancellationToken);
			var content = await response.Content.ReadAsStringAsync(CancellationToken);

			Console.WriteLine($"HTTP Status: {response.StatusCode}");
			Console.WriteLine($"Content Type: {response.Content.Headers.ContentType?.MediaType}");
			Console.WriteLine($"Content Length: {content.Length}");

			// Log response headers
			Console.WriteLine("Response Headers:");
			foreach (var header in response.Headers)
			{
				Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value.Take(1))}");
			}

			if (content.Length > 0)
			{
				Console.WriteLine("Content Preview (first 1000 chars):");
				Console.WriteLine(content.Length > 1000 ? content[..1000] + "..." : content);
			}
			else
			{
				Console.WriteLine("Content is empty");
			}

			Logger.LogInformation("HTTP Status: {StatusCode}", response.StatusCode);
			Logger.LogInformation("Content Type: {ContentType}", response.Content.Headers.ContentType?.MediaType);
			Logger.LogInformation("Content Length: {Length}", content.Length);

			// Log what we actually got for analysis
			response.Should().NotBeNull();
			content.Should().NotBeNull();
		}
		catch (HttpRequestException ex)
		{
			Console.WriteLine($"HTTP request failed: {ex.Message}");
			Logger.LogError(ex, "HTTP request failed: {Message}", ex.Message);
			throw;
		}
	}

	[Fact]
	public void CheckUserSecrets()
	{
		// Diagnostic test to verify user secrets are loaded correctly
		var config = _fixture.Configuration;

		var account = config["HaloApi:HaloAccount"];
		var clientId = config["HaloApi:HaloClientId"];
		var clientSecret = config["HaloApi:HaloClientSecret"];

		Console.WriteLine($"HaloAccount: {account}");
		Console.WriteLine($"HaloClientId: {clientId}");
		Console.WriteLine($"HaloClientSecret: {(!string.IsNullOrEmpty(clientSecret) ? "***SET***" : "NOT SET")}");

		Logger.LogInformation("HaloAccount: {Account}", account);
		Logger.LogInformation("HaloClientId: {ClientId}", clientId);
		Logger.LogInformation("HaloClientSecret: {HasSecret}", !string.IsNullOrEmpty(clientSecret) ? "***SET***" : "NOT SET");

		account.Should().NotBeNullOrEmpty();
		clientId.Should().NotBeNullOrEmpty();
		clientSecret.Should().NotBeNullOrEmpty();

		// Check the constructed URL
		var expectedUrl = $"https://{account}.halopsa.com";
		Console.WriteLine($"Expected API Base URL: {expectedUrl}");
		Console.WriteLine($"Actual API Base URL: {HaloClient.BaseUrl}");

		Logger.LogInformation("Expected API Base URL: {Url}", expectedUrl);

		HaloClient.BaseUrl.Should().Be(expectedUrl);
	}
}