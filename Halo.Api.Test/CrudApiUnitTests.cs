using AwesomeAssertions;
using Halo.Api.Models.Assets;
using Halo.Api.Models.Clients;
using Halo.Api.Models.Projects;
using Halo.Api.Models.Users;
using Xunit;

namespace Halo.Api.Test;

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
		result.Should().NotBeNull();
		result.Should().BeAssignableTo<IReadOnlyList<User>>();
		
		if (result.Count > 0)
		{
			var firstUser = result[0];
			firstUser.Id.Should().BePositive("User ID should be positive");
			firstUser.Name.Should().NotBeNullOrEmpty("User name should not be null or empty");
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
		result.Should().NotBeNull();
		result.Should().BeAssignableTo<IReadOnlyList<Asset>>();
		
		if (result.Count > 0)
		{
			var firstAsset = result[0];
			firstAsset.Id.Should().BePositive("Asset ID should be positive");
			firstAsset.Name.Should().NotBeNullOrEmpty("Asset name should not be null or empty");
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
		result.Should().NotBeNull();
		result.Should().BeAssignableTo<IReadOnlyList<Project>>();
		
		if (result.Count > 0)
		{
			var firstProject = result[0];
			firstProject.Id.Should().BePositive("Project ID should be positive");
			firstProject.Name.Should().NotBeNullOrEmpty("Project name should not be null or empty");
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
		result.Should().NotBeNull();
		result.Should().BeAssignableTo<IReadOnlyList<Client>>();
		
		if (result.Count > 0)
		{
			var firstClient = result[0];
			firstClient.Id.Should().BePositive("Client ID should be positive");
			firstClient.Name.Should().NotBeNullOrEmpty("Client name should not be null or empty");
		}
	}

	[Fact]
	public async Task GetClientById_WithValidId_ShouldReturnClient()
	{
		// Arrange - First get a client to use for testing
		var allClients = await HaloClient.Psa.Clients.GetAllAsync(CancellationToken);
		allClients.Should().NotBeEmpty("Need at least one client for GetById test");
		
		var testClientId = allClients[0].Id;

		// Act
		var result = await HaloClient.Psa.Clients.GetByIdAsync(testClientId, CancellationToken);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(testClientId);
		result.Name.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetClientById_WithInvalidId_ShouldThrowException()
	{
		// Arrange
		var invalidClientId = -999999;

		// Act & Assert
		var act = async () => await HaloClient.Psa.Clients.GetByIdAsync(invalidClientId, CancellationToken);
		await act.Should().ThrowAsync<Exception>("Getting non-existent client should throw exception");
	}
}