using SW.PrimitiveTypes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SW.HttpExtensions
{
    public class ApiOperationBuilder<TApiClientOptions> where TApiClientOptions : ApiClientOptionsBase
    {
        private readonly HttpClient httpClient;
        private readonly RequestContext requestContext;
        private readonly TApiClientOptions options;

        private string path = string.Empty;

        public ApiOperationBuilder(HttpClient httpClient, RequestContext requestContext, TApiClientOptions options)
        {
            this.httpClient = httpClient;
            this.requestContext = requestContext;
            this.options = options;

            if (requestContext.IsValid)
                httpClient.DefaultRequestHeaders.Add(RequestContext.CorrelationIdHeaderName, requestContext.CorrelationId);
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

        public ApiOperationBuilder<TApiClientOptions> Key(ApiKeyParameters apiKeyParameters = null)
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

        async public Task<int> DeleteAsync(bool throwOnFailure = true)
        {
            try
            {
                var httpResponseMessage = await httpClient.DeleteAsync(path);
                return ProcessResponse(httpResponseMessage, throwOnFailure);
            }
            catch
            {
                if (throwOnFailure) throw;
                return 0;
            }
        }

        async public Task<int> PostAsync(object payload = null, bool throwOnFailure = true)
        {
            try
            {
                var httpResponseMessage = await httpClient.PostAsync(path, httpClient.CreateHttpContent(payload));
                return ProcessResponse(httpResponseMessage, throwOnFailure);
            }
            catch
            {
                if (throwOnFailure) throw;
                return 0;
            }
        }

        private int ProcessResponse(HttpResponseMessage httpResponseMessage, bool throwOnFailure)
        {
            if (throwOnFailure) httpResponseMessage.EnsureSuccessStatusCode();
            return (int)httpResponseMessage.StatusCode;
        }

        public ApiOperationRunnerTyped<TResponse> As<TResponse>(bool throwOnFailure = true)
            => new ApiOperationRunnerTyped<TResponse>(httpClient, path, throwOnFailure);
        public ApiOperationRunnerWrapped<TResponse> AsApiResult<TResponse>()
            => new ApiOperationRunnerWrapped<TResponse>(httpClient, path);
        public ApiOperationRunnerWrapped AsApiResult()
            => new ApiOperationRunnerWrapped(httpClient, path);
    }
}
