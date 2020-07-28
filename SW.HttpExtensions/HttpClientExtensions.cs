using Newtonsoft.Json;
using System;
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

        async public static Task<TResult> PostAsync<TResult>(this HttpClient client, string url, object payload)
        {
            var httpResponseMessage = await client.PostAsync(url, payload);
            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadAsAsync<TResult>();
        }

        async public static Task<TResult> GetAsync<TResult>(this HttpClient client, string url)
        {
            var httpResponseMessage = await client.GetAsync(url);
            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadAsAsync<TResult>();
        }

        public static StringContent CreateStringContent(this HttpClient client, object payload)
        {
            if (payload == null)
                return new StringContent(string.Empty, Encoding.UTF8, "application/json");
            else if (payload.GetType() == typeof(string) || payload.GetType().IsPrimitive)
                return new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
            else
                return new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        }
    }
}
