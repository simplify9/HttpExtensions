using SW.PrimitiveTypes;
using System.Net.Http;


namespace SW.HttpExtensions
{
    public abstract class ApiClientBase<TApiClientOptions> where TApiClientOptions : ApiClientOptionsBase
    {
        protected ApiClientBase(HttpClient httpClient, RequestContext requestContext, TApiClientOptions options)
        {
            HttpClient = httpClient;
            Options = options;
            Builder = new ApiOperationBuilder<TApiClientOptions>(httpClient, requestContext, Options);
        }

        protected HttpClient HttpClient { get; }
        protected TApiClientOptions Options { get; }
        protected ApiOperationBuilder<TApiClientOptions> Builder { get; }

        //protected void AddApiKey()
        //{
        //    operationBuilder.Jwt().Body("").Url("").MustSucceed().  
        //    //HttpClient.DefaultRequestHeaders.Add(Options.ApiKey.Name, Options.ApiKey.Value);
        //}

        //protected void AddJwt()
        //{
        //    var user = requestContext.User;

        //    var jwt = Options.Token.WriteJwt((ClaimsIdentity)user.Identity);

        //    if (jwt != null)
        //        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        //}

        //async public Task<HttpResponseMessage> PostAsync(string url, object payload, ApiHeaderOptions httpOperationHeaderOptions = ApiHeaderOptions.None)
        //{
        //    PrepareHeaderOptions(httpOperationHeaderOptions);
        //    return await HttpClient.PostAsync(url, payload);
        //}

        //async public Task<TResult> PostAsync<TResult>(string url, object payload, ApiHeaderOptions httpOperationHeaderOptions = ApiHeaderOptions.None)
        //{
        //    PrepareHeaderOptions(httpOperationHeaderOptions);
        //    return await HttpClient.PostAsync<TResult>(url, payload);
        //}

        //async public Task<TResult> GetAsync<TResult>(string url, ApiHeaderOptions httpOperationHeaderOptions = ApiHeaderOptions.None)
        //{
        //    PrepareHeaderOptions(httpOperationHeaderOptions);
        //    return await HttpClient.GetAsync<TResult>(url);
        //}

        //async public Task DeleteAsync<TResult>(string url, ApiHeaderOptions httpOperationHeaderOptions = ApiHeaderOptions.None)
        //{
        //    PrepareHeaderOptions(httpOperationHeaderOptions);
        //    var httpResponseMessage = await HttpClient.DeleteAsync(url);
        //    httpResponseMessage.EnsureSuccessStatusCode();
        //    return;
        //}

        //private void PrepareHeaderOptions(ApiHeaderOptions httpOperationHeaderOptions)
        //{
        //    switch (httpOperationHeaderOptions)
        //    {
        //        case ApiHeaderOptions.AddJwt:
        //            AddJwt();
        //            break;
        //        case ApiHeaderOptions.AddApiKey:
        //            AddApiKey();
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}
