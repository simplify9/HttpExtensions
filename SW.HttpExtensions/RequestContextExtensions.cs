using System;
using System.Linq;
using System.Security.Claims;
using SW.PrimitiveTypes;

namespace SW.HttpExtensions;

public static class RequestContextExtensions
{
    public static bool IsFromMicroservice(this RequestContext requestContext) =>
        requestContext.User?.Claims.Any(c =>
            string.Equals(c.Type, ClaimTypes.Actor, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(c.Value, JwtTokenParameters.MicroService, StringComparison.CurrentCultureIgnoreCase)) ?? false;
    
}