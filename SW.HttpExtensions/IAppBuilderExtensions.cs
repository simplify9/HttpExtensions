using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SW.HttpExtensions
{
    public static class IAppBuilderExtensions
    {

        public static IApplicationBuilder UseHttpAsRequestContext(this IApplicationBuilder applicationBuilder)
        {

            applicationBuilder.Use(async (httpContext, next) =>
            {
                var requestContext = httpContext.RequestServices.GetRequiredService<RequestContext>();
                var vals = new List<RequestValue>();

                foreach (var h in httpContext.Request.Headers)
                    vals.Add(new RequestValue(h.Key, string.Join(";", h.Value.ToArray()), RequestValueType.HttpHeader));

                foreach (var q in httpContext.Request.Query)
                    vals.Add(new RequestValue(q.Key, string.Join(";", q.Value.ToArray()), RequestValueType.QueryParameter));

                httpContext.Request.Headers.TryGetValue(RequestContext.CorrelationIdHeaderName, out var cid);

                requestContext.SetLocale(CultureInfo.CurrentCulture.Name);

                requestContext.Set(httpContext.User, vals, cid.FirstOrDefault());

                var loggerFactory = httpContext.RequestServices.GetService<ILoggerFactory>();
                if (loggerFactory != null) loggerFactory.CreateLogger("UseHttpUserRequestContext").LogInformation("Request context set successfully");

                await next();
            });

            return applicationBuilder;
        }
    }
}
