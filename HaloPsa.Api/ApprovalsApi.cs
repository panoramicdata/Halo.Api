using HaloPsa.Api.Interfaces;

namespace HaloPsa.Api;

#pragma warning disable CS9113 // Parameter is unread.
internal sealed class ApprovalsApi(HttpClient httpClient) : IApprovalsApi { }
#pragma warning restore CS9113 // Parameter is unread.
