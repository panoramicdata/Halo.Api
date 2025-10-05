using Halo.Api.Interfaces;

namespace Halo.Api;

// Placeholder implementations - will be replaced with Refit interfaces in future phases
#pragma warning disable CS9113 // Parameter is unread
internal sealed class ActionsApi(HttpClient httpClient) : IActionsApi { }
#pragma warning restore CS9113 // Parameter is unread
