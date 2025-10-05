using AwesomeAssertions;

namespace HaloPsa.Api.Test;

public class ClientTests
{
	[Fact]
	public void CreateClient_ValidCredentials_Succeeds()
		=> _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = "11111111-1111-1111-1111-111111111111",
			HaloClientSecret = "11111111-1111-1111-1111-111111111111-11111111-1111-1111-1111-111111111111"
		});

	[Fact]
	public void CreateClient_ValidCredentials_ExposesProperties()
	{
		// Arrange
		var options = new HaloClientOptions
		{
			HaloAccount = "test-account",
			HaloClientId = "22222222-2222-2222-2222-222222222222",
			HaloClientSecret = "11111111-1111-1111-1111-111111111111-11111111-1111-1111-1111-111111111111"
		};

		// Act
		var client = new HaloClient(options);

		// Assert
		_ = client.Account.Should().Be("test-account");
	}

	[Fact]
	public void HaloClientOptions_Properties_ReturnExpectedValues()
	{
		// Arrange & Act
		var options = new HaloClientOptions
		{
			HaloAccount = "my-account",
			HaloClientId = "33333333-3333-3333-3333-333333333333",
			HaloClientSecret = "44444444-4444-4444-4444-444444444444-55555555-5555-5555-5555-555555555555"
		};

		// Assert
		_ = options.HaloAccount.Should().Be("my-account");
		_ = options.HaloClientId.Should().Be("33333333-3333-3333-3333-333333333333");
		_ = options.HaloClientSecret.Should().Be("44444444-4444-4444-4444-444444444444-55555555-5555-5555-5555-555555555555");
	}

	[Fact]
	public void CreateClient_InvalidClientId_Throws()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
			HaloClientSecret = "11111111-1111-1111-1111-111111111111-11111111-1111-1111-1111-111111111111"
		});
		_ = act.Should().ThrowExactly<FormatException>()
			.WithMessage("HaloClientId must be a valid GUID format (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
	}

	[Fact]
	public void CreateClient_NullOptions_ThrowsArgumentNullException()
	{
		Action act = () => _ = new HaloClient(null!);
		_ = act.Should().ThrowExactly<ArgumentNullException>()
			.WithMessage("Value cannot be null. (Parameter 'options')");
	}

	[Fact]
	public void CreateClient_NullHaloAccount_ThrowsArgumentException()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = null!,
			HaloClientId = "11111111-1111-1111-1111-111111111111",
			HaloClientSecret = "11111111-1111-1111-1111-111111111111-11111111-1111-1111-1111-111111111111"
		});
		_ = act.Should().ThrowExactly<ArgumentException>()
			.WithMessage("HaloAccount cannot be null or empty. (Parameter 'HaloAccount')");
	}

	[Fact]
	public void CreateClient_EmptyHaloAccount_ThrowsArgumentException()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "",
			HaloClientId = "11111111-1111-1111-1111-111111111111",
			HaloClientSecret = "11111111-1111-1111-1111-111111111111-11111111-1111-1111-1111-111111111111"
		});
		_ = act.Should().ThrowExactly<ArgumentException>()
			.WithMessage("HaloAccount cannot be null or empty. (Parameter 'HaloAccount')");
	}

	[Fact]
	public void CreateClient_WhitespaceHaloAccount_ThrowsArgumentException()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "   ",
			HaloClientId = "11111111-1111-1111-1111-111111111111",
			HaloClientSecret = "11111111-1111-1111-1111-111111111111-11111111-1111-1111-1111-111111111111"
		});
		_ = act.Should().ThrowExactly<ArgumentException>()
			.WithMessage("HaloAccount cannot be null or empty. (Parameter 'HaloAccount')");
	}

	[Fact]
	public void CreateClient_NullHaloClientId_ThrowsArgumentException()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = null!,
			HaloClientSecret = "11111111-1111-1111-1111-111111111111-11111111-1111-1111-1111-111111111111"
		});
		_ = act.Should().ThrowExactly<ArgumentException>()
			.WithMessage("HaloClientId cannot be null or empty. (Parameter 'HaloClientId')");
	}

	[Fact]
	public void CreateClient_EmptyHaloClientId_ThrowsArgumentException()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = "",
			HaloClientSecret = "11111111-1111-1111-1111-111111111111-11111111-1111-1111-1111-111111111111"
		});
		_ = act.Should().ThrowExactly<ArgumentException>()
			.WithMessage("HaloClientId cannot be null or empty. (Parameter 'HaloClientId')");
	}

	[Fact]
	public void CreateClient_InvalidHaloClientIdFormat_NoHyphens_ThrowsFormatException()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = "111111111111111111111111111111111111",
			HaloClientSecret = "11111111-1111-1111-1111-111111111111-11111111-1111-1111-1111-111111111111"
		});
		_ = act.Should().ThrowExactly<FormatException>()
			.WithMessage("HaloClientId must be a valid GUID format (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
	}

	[Fact]
	public void CreateClient_InvalidHaloClientIdFormat_WrongLength_ThrowsFormatException()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = "1111-1111-1111-1111-111111111111",
			HaloClientSecret = "11111111-1111-1111-1111-111111111111-11111111-1111-1111-1111-111111111111"
		});
		_ = act.Should().ThrowExactly<FormatException>()
			.WithMessage("HaloClientId must be a valid GUID format (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
	}

	[Fact]
	public void CreateClient_NullHaloClientSecret_ThrowsArgumentException()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = "11111111-1111-1111-1111-111111111111",
			HaloClientSecret = null!
		});
		_ = act.Should().ThrowExactly<ArgumentException>()
			.WithMessage("HaloClientSecret cannot be null or empty. (Parameter 'HaloClientSecret')");
	}

	[Fact]
	public void CreateClient_EmptyHaloClientSecret_ThrowsArgumentException()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = "11111111-1111-1111-1111-111111111111",
			HaloClientSecret = ""
		});
		_ = act.Should().ThrowExactly<ArgumentException>()
			.WithMessage("HaloClientSecret cannot be null or empty. (Parameter 'HaloClientSecret')");
	}

	[Fact]
	public void CreateClient_InvalidHaloClientSecretFormat_SingleGuid_ThrowsFormatException()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = "11111111-1111-1111-1111-111111111111",
			HaloClientSecret = "11111111-1111-1111-1111-111111111111"
		});
		_ = act.Should().ThrowExactly<FormatException>()
			.WithMessage("HaloClientSecret must be in the format of two concatenated GUIDs (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
	}

	[Fact]
	public void CreateClient_InvalidHaloClientSecretFormat_NoHyphens_ThrowsFormatException()
	{
		Action act = () => _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = "11111111-1111-1111-1111-111111111111",
			HaloClientSecret = "1111111111111111111111111111111111111111111111111111111111111111111111111111"
		});
		_ = act.Should().ThrowExactly<FormatException>()
			.WithMessage("HaloClientSecret must be in the format of two concatenated GUIDs (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx-xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
	}

	[Fact]
	public void CreateClient_ValidCredentialsWithMixedCase_Succeeds()
		=> _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = "AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE",
			HaloClientSecret = "AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE-FFFFFFFF-1111-2222-3333-444444444444"
		});

	[Fact]
	public void CreateClient_ValidCredentialsWithLowerCase_Succeeds()
		=> _ = new HaloClient(new HaloClientOptions
		{
			HaloAccount = "test",
			HaloClientId = "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
			HaloClientSecret = "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee-ffffffff-1111-2222-3333-444444444444"
		});
}
