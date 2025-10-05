using HaloPsa.Api.Interfaces;

namespace HaloPsa.Api;

#pragma warning disable CS9113 // Parameter is unread
internal sealed class ServiceCatalogApi(HttpClient httpClient) : IServiceCatalogApi { }
#pragma warning restore CS9113 // Parameter is unread
