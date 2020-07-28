using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SW.HttpExtensions
{

    public class ApiOperationRunnerWrapped<TResponse>
    {
        private readonly HttpClient httpClient;
        private readonly string path;
        private readonly HttpContent stringContent;
        private readonly bool throwOnFailure;

        public ApiOperationRunnerWrapped(HttpClient httpClient, string path, HttpContent stringContent, bool throwOnFailure = false)
        {
            this.httpClient = httpClient;
            this.path = path;
            this.stringContent = stringContent;
            this.throwOnFailure = throwOnFailure;
        }

        async public Task<ApiResult<TResponse>> GetAsync()
        {
            try
            {
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

        async public Task<ApiResult<TResponse>> PostAsync()
        {
            try
            {
                var httpResponseMessage = await httpClient.PostAsync(path, stringContent);
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
                return new ApiResult<TResponse>
                {
                    StatusCode = (int)httpResponseMessage.StatusCode,
                    Success = true,
                    Response = await httpResponseMessage.Content.ReadAsAsync<TResponse>(),
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
