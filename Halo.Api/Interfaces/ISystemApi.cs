namespace Halo.Api.Interfaces;

/// <summary>
/// Interface for System API operations including configuration and administration
/// </summary>
public interface ISystemApi
{
	/// <summary>
	/// Gets the Configuration API for system settings and customization
	/// </summary>
	IConfigurationApi Configuration { get; }

	/// <summary>
	/// Gets the Integration API for third-party system integrations
	/// </summary>
	IIntegrationApi Integration { get; }

	/// <summary>
	/// Gets the Audit API for audit logs and activity tracking
	/// </summary>
	IAuditApi Audit { get; }

	/// <summary>
	/// Gets the Custom Fields API for custom field management
	/// </summary>
	ICustomFieldsApi CustomFields { get; }
}