using SW.HttpExtensions;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SW.HttpExtensions
{
    public abstract class HttpClientBase<TOptions> where TOptions : HttpClientOptionsBase
    {
        protected HttpClientBase(HttpClient httpClient, RequestContextManager requestContextManager, TOptions options)
        {
            HttpClient = httpClient;
            RequestContextManager = requestContextManager;
            Options = options;
        }

        protected HttpClient HttpClient { get; }
        protected RequestContextManager RequestContextManager { get; }
        protected TOptions Options { get; }

        protected void AddApiKey()
        {
            HttpClient.DefaultRequestHeaders.Add(Options.ApiKey.Name, Options.ApiKey.Value);
        }

        protected async Task AddJwt()
        {

            var user = (await RequestContextManager.GetCurrentContext()).User;

            var jwt = ((ClaimsIdentity)user.Identity).GenerateJwt(Options.Token.Key, Options.Token.Issuer, Options.Token.Audience);

            if (jwt != null)
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }
    }
}
