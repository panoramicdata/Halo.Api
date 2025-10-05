using HaloPsa.Api.Interfaces;

namespace HaloPsa.Api.Test;

/// <summary>
/// Abstract base class for tests that provides common dependencies
/// </summary>
public abstract class TestBase(IntegrationTestFixture fixture)
{
	/// <summary>
	/// Gets the test fixture for creating fresh client instances
	/// </summary>
	protected readonly IntegrationTestFixture _fixture = fixture;

	/// <summary>
	/// Gets the Halo API client for testing
	/// </summary>
	protected IHaloClient HaloClient { get; } = fixture.GetHaloClient();

	/// <summary>
	/// Gets the logger for test output and diagnostics
	/// </summary>
	protected ILogger Logger { get; } = fixture.Logger;

	/// <summary>
	/// Gets a cancellation token for test operations with a reasonable timeout
	/// </summary>
	protected static CancellationToken CancellationToken { get; } = new CancellationTokenSource(TimeSpan.FromMinutes(2)).Token;
}