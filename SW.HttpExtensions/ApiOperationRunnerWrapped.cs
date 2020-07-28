using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SW.HttpExtensions
{
    public class ApiOperationRunnerWrapped
    {
        private readonly HttpClient httpClient;
        private readonly string path;

        public ApiOperationRunnerWrapped(HttpClient httpClient, string path)
        {
            this.httpClient = httpClient;
            this.path = path;
        }

        async public Task<ApiResult> GetAsync(object parameters)
        {
            try
            {
                var httpResponseMessage = await httpClient.GetAsync(path);
                return new ApiResult
                {
                    Body = await httpResponseMessage.Content.ReadAsStringAsync(),
                    Success = (int)httpResponseMessage.StatusCode >= 200 && (int)httpResponseMessage.StatusCode < 300,
                    StatusCode = (int)httpResponseMessage.StatusCode
                };

            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Body = ex.Message
                };
            }
        }

        async public Task<ApiResult> PostAsync(object payload)
        {
            try
            {
                var httpResponseMessage = await httpClient.PostAsync(path, httpClient.CreateStringContent(payload));
                return new ApiResult
                {
                    Body = await httpResponseMessage.Content.ReadAsStringAsync(),
                    Success = (int)httpResponseMessage.StatusCode >= 200 && (int)httpResponseMessage.StatusCode < 300,
                    StatusCode = (int)httpResponseMessage.StatusCode
                };

            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Body = ex.Message
                };
            }
        }

        async public Task<ApiResult> DeleteAsync()
        {
            try
            {
                var httpResponseMessage = await httpClient.DeleteAsync(path);
                return new ApiResult
                {
                    StatusCode = (int)httpResponseMessage.StatusCode,
                    Success = (int)httpResponseMessage.StatusCode >= 200 && (int)httpResponseMessage.StatusCode < 300
                };

            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Body = ex.Message
                };
            }
        }
    }
}
