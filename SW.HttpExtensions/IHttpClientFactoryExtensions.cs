using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SW.HttpExtensions
{
    public static  class IHttpClientFactoryExtensions
    {
        public static HttpClient CreateClient(this IHttpClientFactory httpClientFactory, Uri baseAddress, string jwt = null)
        {
            var httpClient = httpClientFactory.CreateClient();

            httpClient.BaseAddress = baseAddress;

            if (jwt != null)
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

            return httpClient;
        }
    }
}
