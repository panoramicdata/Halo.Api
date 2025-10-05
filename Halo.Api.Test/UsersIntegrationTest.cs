using AwesomeAssertions;
using Halo.Api.Models.Users;
using Xunit;

namespace Halo.Api.Test;

[Collection("Integration Tests")]
public class UsersIntegrationTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task GetUsers_ShouldReturnUsers()
	{
		// Act - Use the proper HaloClient API
		var result = await HaloClient.Psa.Users.GetAllAsync(CancellationToken);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeAssignableTo<IReadOnlyList<User>>();

		// If there are users, verify the structure
		if (result.Count > 0)
		{
			var firstUser = result[0];
			firstUser.Id.Should().BePositive();
			firstUser.Name.Should().NotBeNullOrEmpty();
		}
	}
}