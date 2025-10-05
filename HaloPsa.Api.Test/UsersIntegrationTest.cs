using AwesomeAssertions;
using HaloPsa.Api.Models.Users;
using Xunit;

namespace HaloPsa.Api.Test;

[Collection("Integration Tests")]
public class UsersIntegrationTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task GetUsers_ShouldReturnUsers()
	{
		// Act - Use the proper HaloClient API
		var result = await HaloClient.Psa.Users.GetAllAsync(CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().BeAssignableTo<IReadOnlyList<User>>();

		// If there are users, verify the structure
		if (result.Count > 0)
		{
			var firstUser = result[0];
			_ = firstUser.Id.Should().BePositive();
			_ = firstUser.Name.Should().NotBeNullOrEmpty();
		}
	}
}