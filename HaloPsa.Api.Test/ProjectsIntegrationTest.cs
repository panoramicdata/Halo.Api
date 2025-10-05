using AwesomeAssertions;
using HaloPsa.Api.Models.Projects;
using Xunit;

namespace HaloPsa.Api.Test;

[Collection("Integration Tests")]
public class ProjectsIntegrationTest(IntegrationTestFixture fixture) : TestBase(fixture)
{
	[Fact]
	public async Task GetProjects_ShouldReturnProjects()
	{
		// Act - Use the proper HaloClient API
		var result = await HaloClient.Psa.Projects.GetAllAsync(CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Should().BeAssignableTo<IReadOnlyList<Project>>();

		// If there are projects, verify the structure
		if (result.Count > 0)
		{
			var firstProject = result[0];
			_ = firstProject.Id.Should().BePositive();
			_ = firstProject.Name.Should().NotBeNullOrEmpty();
		}
	}
}