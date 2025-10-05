using AwesomeAssertions;
using HaloPsa.Api.Models.Assets;
using HaloPsa.Api.Models.Clients;
using HaloPsa.Api.Models.Projects;
using HaloPsa.Api.Models.Users;

namespace HaloPsa.Api.Test;

[Collection("Integration Tests")]
public class UsersApiUnitTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task GetAllUsers_ShouldReturnUsersList()
	{
		// Arrange
		var usersApi = HaloClient.Psa.Users;

		// Act
		var result = await usersApi.GetAllAsync(CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().BeAssignableTo<IReadOnlyList<User>>();

		if (result.Count > 0)
		{
			var firstUser = result[0];
			_ = firstUser.Id.Should().BePositive("User ID should be positive");
			_ = firstUser.Name.Should().NotBeNullOrEmpty("User name should not be null or empty");
		}
	}
}

[Collection("Integration Tests")]
public class AssetsApiUnitTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task GetAllAssets_ShouldReturnAssetsList()
	{
		// Arrange
		var assetsApi = HaloClient.Psa.Assets;

		// Act
		var result = await assetsApi.GetAllAsync(CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().BeAssignableTo<IReadOnlyList<Asset>>();

		if (result.Count > 0)
		{
			var firstAsset = result[0];
			_ = firstAsset.Id.Should().BePositive("Asset ID should be positive");
			_ = firstAsset.Name.Should().NotBeNullOrEmpty("Asset name should not be null or empty");
		}
	}
}

[Collection("Integration Tests")]
public class ProjectsApiUnitTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task GetAllProjects_ShouldReturnProjectsList()
	{
		// Arrange
		var projectsApi = HaloClient.Psa.Projects;

		// Act
		var result = await projectsApi.GetAllAsync(CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().BeAssignableTo<IReadOnlyList<Project>>();

		if (result.Count > 0)
		{
			var firstProject = result[0];
			_ = firstProject.Id.Should().BePositive("Project ID should be positive");
			_ = firstProject.Name.Should().NotBeNullOrEmpty("Project name should not be null or empty");
		}
	}
}

[Collection("Integration Tests")]
public class ClientsApiUnitTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task GetAllClients_ShouldReturnClientsList()
	{
		// Arrange
		var clientsApi = HaloClient.Psa.Clients;

		// Act
		var result = await clientsApi.GetAllAsync(CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().BeAssignableTo<IReadOnlyList<Client>>();

		if (result.Count > 0)
		{
			var firstClient = result[0];
			_ = firstClient.Id.Should().BePositive("Client ID should be positive");
			_ = firstClient.Name.Should().NotBeNullOrEmpty("Client name should not be null or empty");
		}
	}

	[Fact]
	public async Task GetClientById_WithValidId_ShouldReturnClient()
	{
		// Arrange - First get a client to use for testing
		var allClients = await HaloClient.Psa.Clients.GetAllAsync(CancellationToken);
		_ = allClients.Should().NotBeEmpty("Need at least one client for GetById test");

		var testClientId = allClients[0].Id;

		// Act
		var result = await HaloClient.Psa.Clients.GetByIdAsync(testClientId, CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Id.Should().Be(testClientId);
		_ = result.Name.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetClientById_WithInvalidId_ShouldThrowException()
	{
		// Arrange
		var invalidClientId = -999999;

		// Act & Assert
		var act = async () => await HaloClient.Psa.Clients.GetByIdAsync(invalidClientId, CancellationToken);
		_ = await act.Should().ThrowAsync<Exception>("Getting non-existent client should throw exception");
	}
}