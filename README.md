# Halo PSA API .NET Library

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/bdd973c7af5b4a6b8797fbe94ce01b76)](https://app.codacy.com/gh/panoramicdata/Halo.Api?utm_source=github.com&utm_medium=referral&utm_content=panoramicdata/Halo.Api&utm_campaign=Badge_Grade)
[![NuGet Version](https://img.shields.io/nuget/v/Halo.Api)](https://www.nuget.org/packages/Halo.Api)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Halo.Api)](https://www.nuget.org/packages/Halo.Api)
[![Build Status](https://img.shields.io/github/actions/workflow/status/panoramicdata/HaloPSA.Api/build.yml)](https://github.com/panoramicdata/HaloPSA.Api/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A comprehensive, modern .NET library for interacting with the [Halo PSA](https://halopsa.com/) API. This library provides full coverage of the Halo PSA API with a clean, intuitive interface using modern C# patterns and best practices.

## 📚 Official Documentation

- **API Documentation**: [https://halo.haloservicedesk.com/apidoc/info](https://halo.haloservicedesk.com/apidoc/info)
- **Authentication Guide**: [https://halo.haloservicedesk.com/apidoc/authentication/password](https://halo.haloservicedesk.com/apidoc/authentication/password)
- **Halo PSA Official Site**: [https://haloservicedesk.com/halopsa](https://haloservicedesk.com/halopsa)

## Features

- 🎯 **Complete API Coverage** - Full support for all Halo PSA endpoints
- 🚀 **Modern .NET** - Built for .NET 9+ with modern C# features
- 🔒 **Type Safety** - Strongly typed models and responses
- 📝 **Comprehensive Logging** - Built-in logging and request/response interception
- 🔄 **Retry Logic** - Automatic retry with exponential backoff
- 📖 **Rich Documentation** - IntelliSense-friendly XML documentation
- ✅ **Thoroughly Tested** - Comprehensive unit and integration tests
- ⚡ **High Performance** - Optimized for efficiency and low memory usage

## Installation

Install the package via NuGet Package Manager:

```bash
dotnet add package Halo.Api
```

Or via Package Manager Console:

```powershell
Install-Package Halo.Api
```

## Quick Start

### 1. Authentication Setup

Halo API uses **password-based authentication** with agent credentials. You'll need:

1. **Halo Account Name** - Your instance identifier (e.g., "yourcompany" for "yourcompany.halopsa.com")
2. **Client ID** - Your application's registered client ID (GUID format)
3. **Username** - A Halo agent's username with API permissions
4. **Password** - The agent's password
5. **Tenant** - For hosted solutions, typically "Halo" (optional for on-premise)

Refer to the [official authentication documentation](https://halo.haloservicedesk.com/apidoc/authentication/password) for detailed guidance on obtaining these credentials.

```csharp
using Halo.Api;

var options = new HaloClientOptions
{
    HaloAccount = "your-account-name",      // e.g., "yourcompany" 
    HaloClientId = "your-client-id-guid",   // Application client ID
    HaloUsername = "agent-username",        // Halo agent username
    HaloPassword = "agent-password",        // Halo agent password
    Tenant = "Halo"                        // For hosted solutions (optional)
};

var client = new HaloClient(options);
```

### 2. Basic Usage Examples

#### Working with Tickets

```csharp
// Use a CancellationToken for all async operations
using var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;

// Get all open tickets
var filter = new TicketFilter
{
    Status = TicketStatus.Open,
    Count = 50
};

var tickets = await client.Psa.Tickets.GetAllAsync(filter, cancellationToken);

foreach (var ticket in tickets.Tickets)
{
    Console.WriteLine($"Ticket #{ticket.Id}: {ticket.Summary}");
}

// Get a specific ticket with details
var ticket = await client.Psa.Tickets.GetByIdAsync(12345, includeDetails: true, cancellationToken);
Console.WriteLine($"Ticket: {ticket.Summary}");
Console.WriteLine($"Status: {ticket.Status}");
Console.WriteLine($"Assigned to: {ticket.AssignedAgent?.Name}");

// Create a new ticket
var newTicket = new CreateTicketRequest
{
    Summary = "New ticket from API",
    Details = "This ticket was created using the Halo.Api library",
    ClientId = 123,
    UserId = 456,
    TicketTypeId = 1
};

var createdTicket = await client.Psa.Tickets.CreateAsync(newTicket, cancellationToken);
Console.WriteLine($"Created ticket #{createdTicket.Id}");
```

#### Working with Users

```csharp
// Search for users
var userFilter = new UserFilter
{
    Search = "john.doe",
    IncludeActive = true
};

var users = await client.Psa.Users.GetAllAsync(userFilter, cancellationToken);

// Get user details
var user = await client.Psa.Users.GetByIdAsync(123, includeDetails: true, cancellationToken);
Console.WriteLine($"User: {user.Name} ({user.EmailAddress})");

// Create a new user
var newUser = new CreateUserRequest
{
    Name = "Jane Smith",
    EmailAddress = "jane.smith@example.com",
    SiteId = 1,
    IsActive = true
};

var createdUser = await client.Psa.Users.CreateAsync(newUser, cancellationToken);
```

#### Working with Clients

```csharp
// Get all active clients
var clientFilter = new ClientFilter
{
    IncludeActive = true,
    Count = 100
};

var clients = await client.Psa.Clients.GetAllAsync(clientFilter, cancellationToken);

// Get client with additional details
var clientDetails = await client.Psa.Clients.GetByIdAsync(123, includeDetails: true, cancellationToken);
Console.WriteLine($"Client: {clientDetails.Name}");
Console.WriteLine($"Contact: {clientDetails.MainContact?.Name}");
```

### 3. Advanced Configuration

#### Custom HTTP Configuration

```csharp
var options = new HaloClientOptions
{
    HaloAccount = "your-account",
    HaloClientId = "your-client-id",
    HaloUsername = "your-username",
    HaloPassword = "your-password",
    
    // Custom timeout
    RequestTimeout = TimeSpan.FromSeconds(30),
    
    // Custom retry policy
    MaxRetryAttempts = 3,
    RetryDelay = TimeSpan.FromSeconds(1),
    
    // Custom base URL (if using on-premises)
    BaseUrl = "https://your-instance.halopsa.com",
    
    // Custom scope (default is "all")
    Scope = "tickets users clients"
};

var client = new HaloClient(options);
```

#### Logging Configuration

```csharp
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

// Create a service collection with logging
var services = new ServiceCollection();
services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug));

var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<HaloClient>>();

var options = new HaloClientOptions
{
    // ... authentication details
    Logger = logger,
    EnableRequestLogging = true,
    EnableResponseLogging = true
};

var client = new HaloClient(options);
```

### 4. Authentication Troubleshooting

If you're experiencing authentication issues:

1. **Verify Credentials**: Ensure the username/password can log into the Halo web interface
2. **Check API Permissions**: The user account must have API access permissions in Halo
3. **Validate Client ID**: Ensure your application is registered in Halo with the correct client ID
4. **Tenant Parameter**: For hosted solutions, include the `Tenant = "Halo"` parameter
5. **On-Premise vs Hosted**: On-premise installations may not require the tenant parameter

### 5. Pagination and Large Result Sets

```csharp
// Handle pagination automatically
var allTickets = new List<Ticket>();
var pageSize = 100;
var pageNumber = 1;

do
{
    var filter = new TicketFilter
    {
        PageSize = pageSize,
        PageNumber = pageNumber,
        IncludeActive = true
    };
    
    var response = await client.Psa.Tickets.GetAllAsync(filter, cancellationToken);
    allTickets.AddRange(response.Tickets);
    
    pageNumber++;
    
    // Continue while we got a full page
} while (response.Tickets.Count == pageSize);

Console.WriteLine($"Retrieved {allTickets.Count} total tickets");
```

### 6. Error Handling

```csharp
try
{
    var ticket = await client.Psa.Tickets.GetByIdAsync(99999, cancellationToken);
}
catch (HaloNotFoundException ex)
{
    Console.WriteLine($"Ticket not found: {ex.Message}");
}
catch (HaloAuthenticationException ex)
{
    Console.WriteLine($"Authentication failed: {ex.Message}");
    Console.WriteLine("Check your username, password, and API permissions");
}
catch (HaloApiException ex)
{
    Console.WriteLine($"API error: {ex.Message}");
    Console.WriteLine($"Status code: {ex.StatusCode}");
    Console.WriteLine($"Error details: {ex.ErrorDetails}");
}
```

## API Coverage

This library provides comprehensive coverage of the Halo PSA API, organized into logical groups. For complete API endpoint documentation, refer to the [official API documentation](https://halo.haloservicedesk.com/apidoc/info).

### PSA Module (`client.Psa`)
- **Tickets** - Full CRUD operations, filtering, actions, and workflow
- **Users** - User management, authentication, and permissions
- **Clients** - Client and site management
- **Assets** - Asset tracking and configuration management
- **Projects** - Project management and time tracking
- **Reports** - Reporting and analytics

### ServiceDesk Module (`client.ServiceDesk`)
- **Knowledge Base** - Article management and search
- **Service Catalog** - Service requests and approvals
- **Assets** - IT asset management
- **Workflows** - Custom workflows and automation

### System Module (`client.System`)
- **Configuration** - System settings and customization
- **Integration** - Third-party system integrations
- **Audit** - Audit logs and activity tracking

## Configuration Options

The `HaloClientOptions` class provides extensive configuration:

```csharp
public class HaloClientOptions
{
    // Required authentication
    public required string HaloAccount { get; init; }
    public required string HaloClientId { get; init; }
    public required string HaloUsername { get; init; }
    public required string HaloPassword { get; init; }
    
    // Optional authentication
    public string? Tenant { get; init; } = null;  // For hosted solutions
    public string? Scope { get; init; } = "all";  // API permissions scope
    
    // Optional configuration
    public string? BaseUrl { get; init; } = null;  // Uses default Halo cloud URL
    public TimeSpan RequestTimeout { get; init; } = TimeSpan.FromSeconds(30);
    public int MaxRetryAttempts { get; init; } = 3;
    public TimeSpan RetryDelay { get; init; } = TimeSpan.FromSeconds(1);
    public ILogger? Logger { get; init; } = null;
    
    // Advanced options
    public bool EnableRequestLogging { get; init; } = false;
    public bool EnableResponseLogging { get; init; } = false;
    public Dictionary<string, string> DefaultHeaders { get; init; } = [];
}
```

## API Reference

For detailed API endpoint documentation, parameters, and response formats, please refer to the official resources:

- 📖 **[Halo API Documentation](https://halo.haloservicedesk.com/apidoc/info)** - Complete API reference
- 🔐 **[Authentication Guide](https://halo.haloservicedesk.com/apidoc/authentication/password)** - How to obtain and use API credentials
- 🌐 **[Halo Service Desk](https://haloservicedesk.com/)** - Official product documentation

## Contributing

We welcome contributions from the community! Here's how you can help:

### Development Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/panoramicdata/HaloPSA.Api.git
   cd HaloPSA.Api
   ```

2. **Install .NET 9 SDK**:
   Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)

3. **Set up User Secrets for testing**:
   ```bash
   cd Halo.Api.Test
   dotnet user-secrets init
   dotnet user-secrets set "HaloApi:HaloAccount" "your-test-account"
   dotnet user-secrets set "HaloApi:HaloClientId" "your-test-client-id"
   dotnet user-secrets set "HaloApi:HaloUsername" "your-test-username"
   dotnet user-secrets set "HaloApi:HaloPassword" "your-test-password"
   dotnet user-secrets set "HaloApi:Tenant" "Halo"
   ```

4. **Build and test**:
   ```bash
   dotnet build
   dotnet test
   ```

### Coding Standards

- **Follow the project's coding standards** as defined in `copilot-instructions.md`
- **Use modern C# patterns** (primary constructors, collection expressions, etc.)
- **Maintain zero warnings policy** - all code must compile without warnings
- **Write comprehensive tests** - both unit and integration tests
- **Document public APIs** - use XML documentation comments

### Pull Request Process

1. **Fork the repository** and create a feature branch
2. **Follow the implementation plan** in `Specification/ImplementationPlan.md`
3. **Write tests** for all new functionality
4. **Ensure all tests pass** including integration tests
5. **Update documentation** as needed
6. **Submit a pull request** with a clear description of changes

### Issue Reporting

When reporting issues:

- **Use the issue templates** provided in the repository
- **Include minimal reproduction code** when possible
- **Specify the library version** and .NET version
- **Include relevant error messages** and stack traces

### Development Guidelines

- **API-First Approach**: All new endpoints should be defined in interfaces first
- **Test-Driven Development**: Write tests before implementing functionality
- **Documentation**: Update both XML docs and README examples
- **Performance**: Consider performance implications of new features
- **Backward Compatibility**: Maintain compatibility when possible

## Support

- **Official Documentation**: [Halo API Docs](https://halo.haloservicedesk.com/apidoc/info)
- **GitHub Issues**: [Report Issues](https://github.com/panoramicdata/HaloPSA.Api/issues)
- **GitHub Discussions**: [Community Support](https://github.com/panoramicdata/HaloPSA.Api/discussions)
- **Halo Support**: Contact Halo Service Desk for API access and account issues

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Copyright

Copyright © 2025 Panoramic Data Limited. All rights reserved.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for a detailed history of changes and releases
