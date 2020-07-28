using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SW.HttpExtensions
{
    public class ApiOperationRunnerTyped<TResponse>
    {
        private readonly ApiOperationRunnerWrapped<TResponse> apiOperationRunnerWrapped;

        public ApiOperationRunnerTyped(HttpClient httpClient, string path, bool throwOnFailure)
            => apiOperationRunnerWrapped = new ApiOperationRunnerWrapped<TResponse>(httpClient, path, throwOnFailure);

        async public Task<TResponse> GetAsync(object parameters = null)
        {
            return (await apiOperationRunnerWrapped.GetAsync(parameters)).Response;
        }

        async public Task<TResponse> PostAsync(object payload = null)
        {
            return (await apiOperationRunnerWrapped.PostAsync(payload)).Response;
        }
    }
}
