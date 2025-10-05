using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Halo.Api;

/// <summary>
/// Configuration options for the Halo API client
/// </summary>
public partial class HaloClientOptions
{
	private static readonly Regex _guidRegex = GetGuidRegex();
	private static readonly Regex _haloClientSecretRegex = GetHaloClientSecretRegex();

	/// <summary>
	/// Gets or sets the Halo account identifier
	/// </summary>
	public required string HaloAccount { get; init; }

	/// <summary>
	/// Gets or sets the Halo client ID (must be in GUID format)
	/// </summary>
	public required string HaloClientId { get; init; }

	/// <summary>
	/// Gets or sets the Halo client secret (must be in the format of two concatenated GUIDs)
	/// </summary>
	public required string HaloClientSecret { get; init; }

	/// <summary>
	/// Gets or sets the base URL for the Halo API. If null, uses the default cloud URL based on HaloAccount
	/// </summary>
	public string? BaseUrl { get; init; }

	/// <summary>
	/// Gets or sets the HTTP request timeout. Default is 30 seconds
	/// </summary>
	public TimeSpan RequestTimeout { get; init; } = TimeSpan.FromSeconds(30);

	/// <summary>
	/// Gets or sets the maximum number of retry attempts for failed requests. Default is 3
	/// </summary>
	public int MaxRetryAttempts { get; init; } = 3;

	/// <summary>
	/// Gets or sets the initial delay between retry attempts. Default is 1 second
	/// </summary>
	public TimeSpan RetryDelay { get; init; } = TimeSpan.FromSeconds(1);

	/// <summary>
	/// Gets or sets the logger instance for HTTP operations. If null, no logging is performed
	/// </summary>
	public ILogger? Logger { get; init; }

	/// <summary>
	/// Gets or sets whether to log HTTP requests
	/// </summary>
	public bool EnableRequestLogging { get; init; }

	/// <summary>
	/// Gets or sets whether to log HTTP responses
	/// </summary>
	public bool EnableResponseLogging { get; init; }

	/// <summary>
	/// Gets or sets additional default headers to include with all requests
	/// </summary>
	public IReadOnlyDictionary<string, string> DefaultHeaders { get; init; } = new Dictionary<string, string>();

	/// <summary>
	/// Gets or sets whether to use exponential back-off for retries. Default is true
	/// </summary>
	public bool UseExponentialBackoff { get; init; } = true;

	/// <summary>
	/// Gets or sets the maximum retry delay when using exponential back-off. Default is 30 seconds
	/// </summary>
	public TimeSpan MaxRetryDelay { get; init; } = TimeSpan.FromSeconds(30);

	/// <summary>
	/// Gets the effective base URL for the Halo API
	/// </summary>
	internal string EffectiveBaseUrl => BaseUrl ?? $"https://{HaloAccount}.halopsa.com";

	internal void Validate()
	{
		if (string.IsNullOrWhiteSpace(HaloAccount))
		{
			throw new ArgumentException("HaloAccount cannot be null or empty.", nameof(HaloAccount));
		}

		if (string.IsNullOrWhiteSpace(HaloClientId))
		{
			throw new ArgumentException("HaloClientId cannot be null or empty.", nameof(HaloClientId));
		}

		if (string.IsNullOrWhiteSpace(HaloClientSecret))
		{
			throw new ArgumentException("HaloClientSecret cannot be null or empty.", nameof(HaloClientSecret));
		}

		if (!_guidRegex.IsMatch(HaloClientId))
		{
			throw new FormatException("HaloClientId must be a valid GUID format (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
		}

		if (!_haloClientSecretRegex.IsMatch(HaloClientSecret))
		{
			throw new FormatException("HaloClientSecret must be in the format of two concatenated GUIDs (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
		}

		if (RequestTimeout <= TimeSpan.Zero)
		{
			throw new ArgumentException("RequestTimeout must be greater than zero.", nameof(RequestTimeout));
		}

		if (MaxRetryAttempts < 0)
		{
			throw new ArgumentException("MaxRetryAttempts cannot be negative.", nameof(MaxRetryAttempts));
		}

		if (RetryDelay < TimeSpan.Zero)
		{
			throw new ArgumentException("RetryDelay cannot be negative.", nameof(RetryDelay));
		}

		if (MaxRetryDelay < RetryDelay)
		{
			throw new ArgumentException("MaxRetryDelay must be greater than or equal to RetryDelay.", nameof(MaxRetryDelay));
		}

		// Validate BaseUrl if provided
		if (BaseUrl != null && !Uri.TryCreate(BaseUrl, UriKind.Absolute, out _))
		{
			throw new ArgumentException("BaseUrl must be a valid absolute URI.", nameof(BaseUrl));
		}
	}

	[GeneratedRegex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", RegexOptions.Compiled)]
	private static partial Regex GetGuidRegex();

	[GeneratedRegex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}-[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", RegexOptions.Compiled)]
	private static partial Regex GetHaloClientSecretRegex();
}