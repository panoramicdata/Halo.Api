using AwesomeAssertions;
using HaloPsa.Api.Models.Assets;

namespace HaloPsa.Api.Test;

[Collection("Integration Tests")]
public class AssetsIntegrationTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task GetAssets_ShouldReturnAssets()
	{
		// Act - Use the proper HaloClient API
		var result = await HaloClient.Psa.Assets.GetAllAsync(CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().BeAssignableTo<IReadOnlyList<Asset>>();

		// If there are assets, verify the structure
		if (result.Count > 0)
		{
			var firstAsset = result[0];
			_ = firstAsset.Id.Should().BePositive();
			_ = firstAsset.Name.Should().NotBeNullOrEmpty();
		}
	}
}