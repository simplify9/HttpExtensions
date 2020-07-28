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

        public ApiOperationRunnerTyped(HttpClient httpClient, string path, HttpContent stringContent, bool throwOnFailure)
            => apiOperationRunnerWrapped = new ApiOperationRunnerWrapped<TResponse>(httpClient, path, stringContent, throwOnFailure);

        async public Task<TResponse> GetAsync()
        {
            return (await apiOperationRunnerWrapped.GetAsync()).Response;
        }

        async public Task<TResponse> PostAsync()
        {
            return (await apiOperationRunnerWrapped.PostAsync()).Response;
        }
    }
}
