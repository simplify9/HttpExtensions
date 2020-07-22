using SW.HttpExtensions;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SW.HttpExtensions
{
    public abstract class ApiClientBase<TApiClientOptions> where TApiClientOptions : ApiClientOptionsBase
    {
        protected ApiClientBase(HttpClient httpClient, RequestContextManager requestContextManager, TApiClientOptions options)
        {
            HttpClient = httpClient;
            RequestContextManager = requestContextManager;
            Options = options;

        }

        protected HttpClient HttpClient { get; }
        protected RequestContextManager RequestContextManager { get; }
        protected TApiClientOptions Options { get; }

        protected void AddApiKey()
        {
            HttpClient.DefaultRequestHeaders.Add(Options.ApiKey.Name, Options.ApiKey.Value);
        }

        protected async Task AddJwt()
        {
            var user = (await RequestContextManager.GetCurrentContext()).User;

            var jwt = Options.Token.WriteJwt((ClaimsIdentity)user.Identity);

            if (jwt != null)
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        async public Task<HttpResponseMessage> PostAsync(string url, object payload, ApiHeaderOptions httpOperationHeaderOptions = ApiHeaderOptions.None)
        {
            await PrepareHeaderOptions(httpOperationHeaderOptions);
            return await HttpClient.PostAsync(url, payload);
        }

        async public Task<TResult> PostAsync<TResult>(string url, object payload, ApiHeaderOptions httpOperationHeaderOptions = ApiHeaderOptions.None)
        {
            await PrepareHeaderOptions(httpOperationHeaderOptions);
            return await HttpClient.PostAsync<TResult>(url, payload);
        }

        async public Task<TResult> GetAsync<TResult>(string url, ApiHeaderOptions httpOperationHeaderOptions = ApiHeaderOptions.None)
        {
            await PrepareHeaderOptions(httpOperationHeaderOptions);
            return await HttpClient.GetAsync<TResult>(url);
        }

        async public Task DeleteAsync<TResult>(string url, ApiHeaderOptions httpOperationHeaderOptions = ApiHeaderOptions.None)
        {
            await PrepareHeaderOptions(httpOperationHeaderOptions);
            var httpResponseMessage = await HttpClient.DeleteAsync(url);
            httpResponseMessage.EnsureSuccessStatusCode();
            return;
        }

        private async Task PrepareHeaderOptions(ApiHeaderOptions httpOperationHeaderOptions)
        {
            switch (httpOperationHeaderOptions)
            {
                case ApiHeaderOptions.AddJwt:
                    await AddJwt();
                    break;
                case ApiHeaderOptions.AddApiKey:
                    AddApiKey();
                    break;
                default:
                    break;
            }
        }
    }
}
