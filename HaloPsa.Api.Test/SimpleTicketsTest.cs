using AwesomeAssertions;

namespace HaloPsa.Api.Test;

[Collection("Integration Tests")]
public class SimpleTicketsTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task SimpleTicketsCall_ShouldWork()
	{
		// Act - Simple test to verify tickets API works
		var result = await HaloClient.Psa.Tickets.GetAllAsync(CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Tickets.Should().NotBeNull();
		_ = result.RecordCount.Should().BeGreaterThanOrEqualTo(0);
	}
}