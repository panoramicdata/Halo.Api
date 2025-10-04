using Halo.Api.Interfaces;
using Microsoft.Extensions.Logging;

namespace Halo.Api.Test;

/// <summary>
/// Abstract base class for tests that provides common dependencies
/// </summary>
public abstract class TestBase
{
	/// <summary>
	/// Gets the Halo API client for testing
	/// </summary>
	protected IHaloClient HaloClient { get; }

	/// <summary>
	/// Gets the logger for test output and diagnostics
	/// </summary>
	protected ILogger Logger { get; }

	/// <summary>
	/// Initializes a new instance of the TestBase class
	/// </summary>
	/// <param name="fixture">The integration test fixture containing dependencies</param>
	protected TestBase(IntegrationTestFixture fixture)
	{
		ArgumentNullException.ThrowIfNull(fixture);
		
		HaloClient = fixture.GetHaloClient();
		Logger = fixture.Logger;
	}
}