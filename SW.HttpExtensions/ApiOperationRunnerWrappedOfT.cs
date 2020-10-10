using SW.PrimitiveTypes;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SW.HttpExtensions
{
    public class ApiOperationRunnerWrapped<TResponse>
    {
        private readonly HttpClient httpClient;
        private string path;
        private readonly bool throwOnFailure;

        public ApiOperationRunnerWrapped(HttpClient httpClient, string path, bool throwOnFailure = false)
        {
            this.httpClient = httpClient;
            this.path = path;
            this.throwOnFailure = throwOnFailure;
        }

        async public Task<ApiResult<TResponse>> GetAsync(object parameters = null)
        {
            try
            {
                if (parameters  != null)
                {
                    //var pathUri = Uri.;
                    path = $"{path}?{parameters.ToQueryString()}";
                }
                    
                var httpResponseMessage = await httpClient.GetAsync(path);
                return await ProcessResponse(httpResponseMessage);
            }
            catch (Exception ex)
            {
                if (throwOnFailure) throw;
                return new ApiResult<TResponse>
                {
                    StatusCode = 0,
                    Body = ex.Message
                };
            }
        }

        async public Task<ApiResult<TResponse>> PostAsync(object payload = null)
        {
            try
            {
                var httpResponseMessage = await httpClient.PostAsync(path, httpClient.CreateHttpContent(payload));
                return await ProcessResponse(httpResponseMessage);
            }
            catch (Exception ex)
            {
                if (throwOnFailure) throw;
                return new ApiResult<TResponse>
                {
                    Body = ex.Message
                };
            }
        }

        async private Task<ApiResult<TResponse>> ProcessResponse(HttpResponseMessage httpResponseMessage)
        {
            if (throwOnFailure) httpResponseMessage.EnsureSuccessStatusCode();
            if ((int)httpResponseMessage.StatusCode >= 200 && (int)httpResponseMessage.StatusCode < 300)
            {
                TResponse response;
                if (typeof(TResponse) == typeof(string))
                    response = (TResponse)(object)await httpResponseMessage.Content.ReadAsStringAsync();

                else
                    response = await httpResponseMessage.Content.ReadAsAsync<TResponse>();

                return new ApiResult<TResponse>
                {
                    StatusCode = (int)httpResponseMessage.StatusCode,
                    Success = true,
                    Response = response
                };
            }
            else
                return new ApiResult<TResponse>
                {
                    StatusCode = (int)httpResponseMessage.StatusCode,
                    Body = await httpResponseMessage.Content.ReadAsStringAsync()
                };
        }

    }
}
