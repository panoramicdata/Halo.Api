namespace Halo.Api;

public class HaloClient
{
	public HaloClient(HaloClientOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);
		options.Validate();
	}
}
