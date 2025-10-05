using AwesomeAssertions;
using Halo.Api.Models.Assets;
using Xunit;

namespace Halo.Api.Test;

[Collection("Integration Tests")]
public class AssetsIntegrationTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task GetAssets_ShouldReturnAssets()
	{
		// Act - Use the proper HaloClient API
		var result = await HaloClient.Psa.Assets.GetAllAsync(CancellationToken);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeAssignableTo<IReadOnlyList<Asset>>();

		// If there are assets, verify the structure
		if (result.Count > 0)
		{
			var firstAsset = result[0];
			firstAsset.Id.Should().BePositive();
			firstAsset.Name.Should().NotBeNullOrEmpty();
		}
	}
}