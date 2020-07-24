using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SW.HttpExtensions
{
    public static class IAppBuilderExtensions
    {

        public static IApplicationBuilder UseHttpUserRequestContext(this IApplicationBuilder applicationBuilder)
        {

            applicationBuilder.Use(async (httpContext, next) =>
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    var vals = new List<RequestValue>();

                    foreach (var h in httpContext.Request.Headers)
                        vals.Add(new RequestValue(h.Key, string.Join(";", h.Value.ToArray()), RequestValueType.HttpHeader));

                    foreach (var q in httpContext.Request.Query)
                        //if (!ignoredKeys.Contains(q.Key, StringComparer.OrdinalIgnoreCase))
                        vals.Add(new RequestValue(q.Key, string.Join(";", q.Value.ToArray()), RequestValueType.QueryParameter));

                    string correlationId = Guid.NewGuid().ToString("N");

                    if (httpContext.Request.Headers.TryGetValue("request-correlation-id", out var cid) && cid.Count > 0)
                        correlationId = cid.First();

                    var requestContext = httpContext.RequestServices.GetRequiredService<RequestContext>();
                    //var logger = httpContext.RequestServices.GetRequiredService<ILogger>();

                    requestContext.Set(httpContext.User, vals, correlationId);
                    //logger.LogInformation("Request context set successfully");

                }


                await next();
            });

            return applicationBuilder;

        }
    }
}
