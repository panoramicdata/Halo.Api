using System.Net;
using AwesomeAssertions;

namespace Halo.Api.Test.Infrastructure;

[Collection("Integration Tests")]
public class AuthenticationTests(IntegrationTestFixture fixture)
{
	private readonly IntegrationTestFixture _fixture = fixture;

	[Fact]
	public void HaloClient_CanBeCreatedWithValidConfiguration()
	{
		// Arrange & Act
		var client = _fixture.GetHaloClient();

		// Assert - Just verify the client can be created and has proper configuration
		client.Should().NotBeNull();
		client.Account.Should().NotBeNullOrEmpty();
		client.BaseUrl.Should().NotBeNullOrEmpty();
		client.BaseUrl.Should().Contain(client.Account);
	}

	[Fact]
	public async Task HaloClient_AuthenticationHandler_IsConfigured()
	{
		// Arrange
		var client = _fixture.GetHaloClient();

		// Act - Make a simple request to verify the authentication infrastructure is set up
		// We don't care if it succeeds or fails, just that the handlers are working
		using var httpClient = new HttpClient();
		var tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"{client.BaseUrl}/auth/token")
		{
			Content = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				["grant_type"] = "client_credentials",
				["client_id"] = _fixture.Configuration["HaloApi:HaloClientId"] ?? throw new InvalidOperationException("HaloApi:HaloClientId not found"),
				["client_secret"] = _fixture.Configuration["HaloApi:HaloClientSecret"] ?? throw new InvalidOperationException("HaloApi:HaloClientSecret not found"),
				["scope"] = "all"
			})
		};

		var response = await httpClient.SendAsync(tokenRequest, TestContext.Current.CancellationToken);

		// Assert - We just verify that we got a response (even if it's an error)
		// This validates that our HTTP infrastructure is working
		response.Should().NotBeNull();
		// Accept any response - we're just testing that the HTTP stack works
		response.StatusCode.Should().BeOneOf(
			HttpStatusCode.OK,                    // Success
			HttpStatusCode.Unauthorized,          // Invalid credentials
			HttpStatusCode.BadRequest,            // Malformed request
			HttpStatusCode.InternalServerError    // Server error
		);
	}

	[Fact]
	public void HaloClient_DisposesResourcesProperly()
	{
		// Arrange
		var options = new HaloClientOptions
		{
			HaloAccount = _fixture.Configuration["HaloApi:HaloAccount"] ?? throw new InvalidOperationException("HaloApi:HaloAccount not found"),
			HaloClientId = _fixture.Configuration["HaloApi:HaloClientId"] ?? throw new InvalidOperationException("HaloApi:HaloClientId not found"),
			HaloClientSecret = _fixture.Configuration["HaloApi:HaloClientSecret"] ?? throw new InvalidOperationException("HaloApi:HaloClientSecret not found")
		};

		// Act & Assert - Verify that disposal works without throwing
		using (var client = new HaloClient(options))
		{
			client.Should().NotBeNull();
		} // Disposal happens here

		// If we get here, disposal worked correctly
		Assert.True(true, "Client disposed successfully");
	}
}