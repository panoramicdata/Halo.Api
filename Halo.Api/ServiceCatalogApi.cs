using Halo.Api.Interfaces;

namespace Halo.Api;

#pragma warning disable CS9113 // Parameter is unread
internal sealed class ServiceCatalogApi(HttpClient httpClient) : IServiceCatalogApi { }
#pragma warning restore CS9113 // Parameter is unread
