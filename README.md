# Halo PSA API .NET Library

[![NuGet Version](https://img.shields.io/nuget/v/Halo.Api)](https://www.nuget.org/packages/Halo.Api)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Halo.Api)](https://www.nuget.org/packages/Halo.Api)
[![Build Status](https://img.shields.io/github/actions/workflow/status/panoramicdata/HaloPSA.Api/build.yml)](https://github.com/panoramicdata/HaloPSA.Api/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A comprehensive, modern .NET library for interacting with the [Halo PSA](https://halopsa.com/) API. This library provides full coverage of the Halo PSA API with a clean, intuitive interface using modern C# patterns and best practices.

## Features

- ?? **Complete API Coverage** - Full support for all Halo PSA endpoints
- ?? **Modern .NET** - Built for .NET 9+ with modern C# features
- ?? **Type Safety** - Strongly typed models and responses
- ?? **Comprehensive Logging** - Built-in logging and request/response interception
- ?? **Retry Logic** - Automatic retry with exponential backoff
- ?? **Rich Documentation** - IntelliSense-friendly XML documentation
- ?? **Thoroughly Tested** - Comprehensive unit and integration tests
- ? **High Performance** - Optimized for efficiency and low memory usage

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

First, obtain your API credentials from your Halo PSA instance:

```csharp
using Halo.Api;

var options = new HaloClientOptions
{
    HaloAccount = "your-account-name",
    HaloClientId = "your-client-id-guid",
    HaloClientSecret = "your-client-secret-two-guids"
};

var client = new HaloClient(options);
```

### 2. Basic Usage Examples

#### Working with Tickets

```csharp
// Get all open tickets
var filter = new TicketFilter
{
    Status = TicketStatus.Open,
    Count = 50
};

var tickets = await client.Psa.Tickets.GetAllAsync(filter);

foreach (var ticket in tickets.Tickets)
{
    Console.WriteLine($"Ticket #{ticket.Id}: {ticket.Summary}");
}

// Get a specific ticket with details
var ticket = await client.Psa.Tickets.GetByIdAsync(12345, includeDetails: true);
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

var createdTicket = await client.Psa.Tickets.CreateAsync(newTicket);
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

var users = await client.Psa.Users.GetAllAsync(userFilter);

// Get user details
var user = await client.Psa.Users.GetByIdAsync(123, includeDetails: true);
Console.WriteLine($"User: {user.Name} ({user.EmailAddress})");

// Create a new user
var newUser = new CreateUserRequest
{
    Name = "Jane Smith",
    EmailAddress = "jane.smith@example.com",
    SiteId = 1,
    IsActive = true
};

var createdUser = await client.Psa.Users.CreateAsync(newUser);
```

#### Working with Clients

```csharp
// Get all active clients
var clientFilter = new ClientFilter
{
    IncludeActive = true,
    Count = 100
};

var clients = await client.Psa.Clients.GetAllAsync(clientFilter);

// Get client with additional details
var clientDetails = await client.Psa.Clients.GetByIdAsync(123, includeDetails: true);
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
    HaloClientSecret = "your-client-secret",
    
    // Custom timeout
    RequestTimeout = TimeSpan.FromSeconds(30),
    
    // Custom retry policy
    MaxRetryAttempts = 3,
    RetryDelay = TimeSpan.FromSeconds(1),
    
    // Custom base URL (if using on-premises)
    BaseUrl = "https://your-instance.haloitsm.com"
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
    Logger = logger
};

var client = new HaloClient(options);
```

### 4. Pagination and Large Result Sets

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
    
    var response = await client.Psa.Tickets.GetAllAsync(filter);
    allTickets.AddRange(response.Tickets);
    
    pageNumber++;
    
    // Continue while we got a full page
} while (response.Tickets.Count == pageSize);

Console.WriteLine($"Retrieved {allTickets.Count} total tickets");
```

### 5. Error Handling

```csharp
try
{
    var ticket = await client.Psa.Tickets.GetByIdAsync(99999);
}
catch (HaloNotFoundException ex)
{
    Console.WriteLine($"Ticket not found: {ex.Message}");
}
catch (HaloAuthenticationException ex)
{
    Console.WriteLine($"Authentication failed: {ex.Message}");
}
catch (HaloApiException ex)
{
    Console.WriteLine($"API error: {ex.Message}");
    Console.WriteLine($"Status code: {ex.StatusCode}");
    Console.WriteLine($"Error details: {ex.ErrorDetails}");
}
```

## API Coverage

This library provides comprehensive coverage of the Halo PSA API, organized into logical groups:

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
    public required string HaloClientSecret { get; init; }
    
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
   dotnet user-secrets set "HaloApi:HaloClientSecret" "your-test-client-secret"
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

- **Documentation**: [GitHub Wiki](https://github.com/panoramicdata/HaloPSA.Api/wiki)
- **Issues**: [GitHub Issues](https://github.com/panoramicdata/HaloPSA.Api/issues)
- **Discussions**: [GitHub Discussions](https://github.com/panoramicdata/HaloPSA.Api/discussions)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Copyright

Copyright © 2025 Panoramic Data Limited. All rights reserved.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for a detailed history of changes and releases
