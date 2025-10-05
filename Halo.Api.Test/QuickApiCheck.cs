using AwesomeAssertions;
using Halo.Api.Models.Tickets;

namespace Halo.Api.Test;

[Collection("Integration Tests")]
public class QuickApiCheck(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task CheckFirstTicketProperties()
	{
		// Arrange
		var filter = new TicketFilter { Count = 1 };

		// Act - Get the first ticket to check its properties
		var result = await HaloClient.Psa.Tickets.GetAllAsync(filter, CancellationToken);

		// Assert
		result.Should().NotBeNull();
		result.Tickets.Should().NotBeNull();

		if (result.Tickets.Count > 0)
		{
			var firstTicket = result.Tickets[0];
			firstTicket.Should().NotBeNull();
			firstTicket.Id.Should().BePositive();
		}
	}
}