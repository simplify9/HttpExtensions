using Newtonsoft.Json;
using SW.PrimitiveTypes;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SW.HttpExtensions
{
    public class ApiOperationBuilder<TApiClientOptions> where TApiClientOptions : ApiClientOptionsBase
    {
        private readonly HttpClient httpClient;
        private readonly RequestContext requestContext;
        private readonly TApiClientOptions options;

        private StringContent stringContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        private string path = string.Empty;

        public ApiOperationBuilder(HttpClient httpClient, RequestContext requestContext, TApiClientOptions options)
        {
            this.httpClient = httpClient;
            this.requestContext = requestContext;
            this.options = options;
        }

        public ApiOperationBuilder<TApiClientOptions> Path(string path)
        {
            this.path = path;
            return this;
        }

        public ApiOperationBuilder<TApiClientOptions> Jwt(string jwt = null)
        {
            var user = requestContext.User;

            if (jwt == null)
                jwt = options.Token.WriteJwt((ClaimsIdentity)user.Identity);

            if (jwt != null)
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            return this;
        }

        public ApiOperationBuilder<TApiClientOptions> ApiKey(ApiKeyParameters apiKeyParameters = null)
        {
            if (apiKeyParameters == null)
                apiKeyParameters = options.ApiKey;

            httpClient.DefaultRequestHeaders.Add(apiKeyParameters.Name, apiKeyParameters.Value);
            return this;
        }

        public ApiOperationBuilder<TApiClientOptions> Header(string name, string value)
        {
            httpClient.DefaultRequestHeaders.Add(name, value);
            return this;
        }

        public ApiOperationBuilder<TApiClientOptions> Body(string body)
        {
            stringContent = new StringContent(body, Encoding.UTF8, "application/json");
            return this;
        }

        public ApiOperationBuilder<TApiClientOptions> Body(object body)
        {
            stringContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            return this;
        }

        async public Task<int> DeleteAsync(bool throwOnFailure = true)
        {
            try
            {
                var httpResponseMessage = await httpClient.DeleteAsync(path);
                if (throwOnFailure) httpResponseMessage.EnsureSuccessStatusCode();
                return (int)httpResponseMessage.StatusCode;
            }
            catch
            {
                if (throwOnFailure) throw;
                return 0;
            }
        }

        async public Task<int> PostAsync(bool throwOnFailure = true)
        {
            try
            {
                var httpResponseMessage = await httpClient.PostAsync(path, stringContent);
                if (throwOnFailure) httpResponseMessage.EnsureSuccessStatusCode();
                return (int)httpResponseMessage.StatusCode;
            }
            catch
            {
                if (throwOnFailure) throw;
                return 0;
            }
        }
        public ApiOperationRunnerTyped<TResponse> As<TResponse>(bool throwOnFailure = true)
            => new ApiOperationRunnerTyped<TResponse>(httpClient, path, stringContent, throwOnFailure);
        public ApiOperationRunnerWrapped<TResponse> AsApiResult<TResponse>()
            => new ApiOperationRunnerWrapped<TResponse>(httpClient, path, stringContent);
        public ApiOperationRunnerWrapped AsApiResult()
            => new ApiOperationRunnerWrapped(httpClient, path, stringContent);
    }
}
