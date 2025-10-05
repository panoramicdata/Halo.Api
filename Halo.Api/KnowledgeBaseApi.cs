using Halo.Api.Interfaces;

namespace Halo.Api;

#pragma warning disable CS9113 // Parameter is unread
internal sealed class KnowledgeBaseApi(HttpClient httpClient) : IKnowledgeBaseApi { }
#pragma warning restore CS9113 // Parameter is unread
