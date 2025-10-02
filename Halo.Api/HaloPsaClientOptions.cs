namespace Halo.Api;

public class HaloClientOptions
{
	public required string HaloAccount { get; init; }
	public required string HaloClientId { get; init; }
	public required string HaloClientSecret { get; init; }
}