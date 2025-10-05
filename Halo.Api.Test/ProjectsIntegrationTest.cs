using AwesomeAssertions;
using Halo.Api.Models.Projects;
using Xunit;

namespace Halo.Api.Test;

[Collection("Integration Tests")]
public class ProjectsIntegrationTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task GetProjects_ShouldReturnProjects()
	{
		// Act - Use the proper HaloClient API
		var result = await HaloClient.Psa.Projects.GetAllAsync(CancellationToken);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeAssignableTo<IReadOnlyList<Project>>();

		// If there are projects, verify the structure
		if (result.Count > 0)
		{
			var firstProject = result[0];
			firstProject.Id.Should().BePositive();
			firstProject.Name.Should().NotBeNullOrEmpty();
		}
	}
}