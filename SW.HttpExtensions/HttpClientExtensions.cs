using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using System.Threading.Tasks;

namespace SW.HttpExtensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string url, object payload)
        {
            var payloadStr = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            try
            {
                return client.PostAsync(url, payloadStr);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error connecting to: {client.BaseAddress}", ex);
            }
        }
    }
}
