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
        private readonly HttpContent stringContent;

        public ApiOperationRunnerWrapped(HttpClient httpClient, string path, HttpContent stringContent)
        {
            this.httpClient = httpClient;
            this.path = path;
            this.stringContent = stringContent;
        }

        async public Task<ApiResult> GetAsync()
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

        async public Task<ApiResult> PostAsync()
        {
            try
            {
                var httpResponseMessage = await httpClient.PostAsync(path, stringContent);
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
