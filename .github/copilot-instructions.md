# GitHub Copilot Instructions

## Code Style Guidelines

### Zero Warnings Policy
- **Always aim for zero warnings and messages on every build**
- Address all compiler warnings before considering code complete
- Use `#pragma warning disable` only in exceptional cases with clear justification
- Enable "Treat warnings as errors" where appropriate

### Modern C# Practices (.NET 9/.NET 10)

#### Primary Constructors
- **Use primary constructors where possible** for cleaner, more concise code:
```csharp
// Preferred
public class HaloClient(HaloClientOptions options) : IHaloClient
{
    public string Account => options.HaloAccount;
}

// Instead of
public class HaloClient : IHaloClient
{
    private readonly HaloClientOptions _options;
    
    public HaloClient(HaloClientOptions options)
    {
        _options = options;
    }
    
    public string Account => _options.HaloAccount;
}
```

#### Collection Initialization
- **Use collection expressions `[]` instead of `new List<T>()`**:
```csharp
// Preferred
var items = [item1, item2, item3];
var emptyList = <string>[];

// Instead of
var items = new List<string> { item1, item2, item3 };
var emptyList = new List<string>();
```

#### Required Properties
- Use `required` keyword for mandatory properties:
```csharp
public class HaloClientOptions
{
    public required string HaloAccount { get; init; }
    public required string HaloClientId { get; init; }
}
```

#### File-Scoped Namespaces
- Always use file-scoped namespaces:
```csharp
// Preferred
namespace Halo.Api;

public class HaloClient
{
}

// Instead of
namespace Halo.Api
{
    public class HaloClient
    {
    }
}
```

#### String Interpolation
- Use string interpolation over concatenation:
```csharp
// Preferred
var message = $"Error in {methodName}: {errorDetails}";

// Instead of
var message = "Error in " + methodName + ": " + errorDetails;
```

#### Pattern Matching
- Use modern pattern matching features:
```csharp
// Preferred
var result = value switch
{
    null => "null value",
    string s when s.Length > 0 => $"String: {s}",
    _ => "other"
};

// Use is patterns for type checks
if (obj is HaloClient client && client.IsValid)
{
    // Process client
}
```

#### Null Handling
- Use null-conditional operators and null-coalescing:
```csharp
// Preferred
var result = client?.GetData()?.FirstOrDefault() ?? defaultValue;

// Use ArgumentNullException.ThrowIfNull
ArgumentNullException.ThrowIfNull(parameter);
```

#### Expression-Bodied Members
- Use expression-bodied members for simple operations:
```csharp
// Preferred
public string FullName => $"{FirstName} {LastName}";
public void LogError(string message) => _logger.LogError(message);
```

#### Records
- Use records for immutable data structures:
```csharp
public record HaloApiResponse(string Data, int StatusCode, DateTime Timestamp);
```

### Code Organization

#### Using Statements
- Place using statements outside namespace (file-scoped)
- Group and sort using statements
- Remove unused using statements

#### Access Modifiers
- Always be explicit about access modifiers
- Use `internal` for implementation details not meant for public API

#### Naming Conventions
- Use PascalCase for public members
- Use camelCase with underscore prefix for private fields: `_fieldName`
- Use meaningful, descriptive names

### Testing Standards
- Use AwesomeAssertions for fluent test assertions
- Follow AAA pattern (Arrange, Act, Assert)
- Use descriptive test method names that explain the scenario

### EditorConfig Compliance
- Follow the .editorconfig settings in the workspace
- Use tabs for indentation as configured
- Maintain consistent formatting across all files

### Performance Considerations
- Use `ConfigureAwait(false)` for library code
- Prefer `StringBuilder` for multiple string concatenations
- Use `span` and `memory` types where appropriate for performance-critical code

### Documentation
- Use XML documentation comments for public APIs
- Include `<summary>`, `<param>`, and `<returns>` tags
- Document any non-obvious behavior or assumptions

### Error Handling
- Use specific exception types (FormatException, ArgumentException, etc.)
- Provide meaningful error messages
- Use structured logging where applicable