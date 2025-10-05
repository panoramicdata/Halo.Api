using AwesomeAssertions;
using Halo.Api.Models.TicketTypes;
using Xunit;

namespace Halo.Api.Test;

[Collection("Integration Tests")]
public class TicketTypeIntegrationTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task GetTicketTypes_ShouldReturnTicketTypes()
	{
		// Act - Use the proper HaloClient API
		var result = await HaloClient.Psa.TicketTypes.GetAllAsync(CancellationToken);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeAssignableTo<IReadOnlyList<TicketType>>();

		// If there are ticket types, verify the structure
		if (result.Count > 0)
		{
			var firstTicketType = result[0];
			firstTicketType.Id.Should().BePositive();
			firstTicketType.Name.Should().NotBeNullOrEmpty();
		}
	}
}
