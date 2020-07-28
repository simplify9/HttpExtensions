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
        private string url = string.Empty;
        private bool mustSucceed;

        public ApiOperationBuilder(HttpClient httpClient, RequestContext requestContext, TApiClientOptions options)
        {
            this.httpClient = httpClient;
            this.requestContext = requestContext;
            this.options = options;
        }

        public ApiOperationBuilder<TApiClientOptions> Url(string url)
        {
            this.url = url;
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

        public ApiOperationBuilder<TApiClientOptions> ApiKey(ApiKeyParameters apiKeyParameters)
        {
            if (apiKeyParameters == null)
                apiKeyParameters = options.ApiKey;

            httpClient.DefaultRequestHeaders.Add(apiKeyParameters.Name, apiKeyParameters.Value);
            return this;
        }

        public ApiOperationBuilder<TApiClientOptions> MustSucceed(bool mustSucceed = true)
        {
            this.mustSucceed = mustSucceed;
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

        async public Task<ApiResult<TResponse>> GetAsync<TResponse>()
        {
            try
            {
                var httpResponseMessage = await httpClient.GetAsync(url);

                if (mustSucceed) httpResponseMessage.EnsureSuccessStatusCode();

                if ((int)httpResponseMessage.StatusCode >= 200 && (int)httpResponseMessage.StatusCode < 300)
                {
                    TResponse response;
                    if (typeof(TResponse) == typeof(string))

                        response = (TResponse)(object)(await httpResponseMessage.Content.ReadAsStringAsync());

                    else
                        response = await httpResponseMessage.Content.ReadAsAsync<TResponse>();

                    return new ApiResult<TResponse>
                    {
                        Response = response,
                        Success = true,
                        StatusCode = (int)httpResponseMessage.StatusCode
                    };
                }

                else
                {
                    return new ApiResult<TResponse>
                    {
                        StatusCode = (int)httpResponseMessage.StatusCode,
                        Body = await httpResponseMessage.Content.ReadAsStringAsync()
                    };
                }
            }
            catch (Exception ex)
            {
                if (mustSucceed) throw;

                return new ApiResult<TResponse>
                {
                    StatusCode = 0,
                    Body = ex.Message
                };
            }
        }

        async public Task<ApiResult> DeleteAsync()
        {
            try
            {
                //await PopulateJwt();
                var httpResponseMessage = await httpClient.DeleteAsync(url);

                if (mustSucceed) httpResponseMessage.EnsureSuccessStatusCode();

                return new ApiResult
                {
                    StatusCode = (int)httpResponseMessage.StatusCode,
                    Success = (int)httpResponseMessage.StatusCode >= 200 && (int)httpResponseMessage.StatusCode < 300
                };

            }
            catch (Exception ex)
            {

                if (mustSucceed) throw;

                return new ApiResult
                {
                    StatusCode = 0,
                    Body = ex.Message
                };
            }
        }

        async public Task<ApiResult> PostAsync()
        {
            var result = await PostAsync<NoT>();

            return new ApiResult
            {
                Body = result.Body,
                StatusCode = result.StatusCode,
                Success = result.Success
            };
        }

        async public Task<ApiResult<TResponse>> PostAsync<TResponse>()
        {
            try
            {
                var httpResponseMessage = await httpClient.PostAsync(url, stringContent);

                if (mustSucceed) httpResponseMessage.EnsureSuccessStatusCode();

                if ((int)httpResponseMessage.StatusCode >= 200 && (int)httpResponseMessage.StatusCode < 300)
                {

                    TResponse response;
                    string body = null;

                    if (typeof(TResponse) == typeof(string))
                        response = (TResponse)(object)(await httpResponseMessage.Content.ReadAsStringAsync());

                    else if (typeof(TResponse) == typeof(NoT))
                    {
                        response = default;
                        body = await httpResponseMessage.Content.ReadAsStringAsync();
                    }
                    else
                        response = await httpResponseMessage.Content.ReadAsAsync<TResponse>();

                    return new ApiResult<TResponse>
                    {
                        StatusCode = (int)httpResponseMessage.StatusCode,
                        Success = true,
                        Response = response,
                        Body = body
                    };
                }

                else
                    return new ApiResult<TResponse>
                    {
                        StatusCode = (int)httpResponseMessage.StatusCode,
                        Body = await httpResponseMessage.Content.ReadAsStringAsync()
                    };
            }
            catch (Exception ex)
            {

                if (mustSucceed) throw;

                return new ApiResult<TResponse>
                {
                    StatusCode = 0,
                    Body = ex.Message
                };
            }

        }

        private class NoT { }
    }
}
