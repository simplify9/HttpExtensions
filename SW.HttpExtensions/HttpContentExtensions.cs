using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SW.HttpExtensions
{
    public static class HttpContentExtensions
    {
        async public static Task<T> ReadAsAsync<T>(this HttpContent httpContent)
        {
            var resutlStr = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(resutlStr);
        }

    }
}
