using System.Linq;
using System.Security.Claims;
using SW.PrimitiveTypes;

namespace SW.HttpExtensions;

public static class RequestContextExtensions
{
    public static bool IsFromMicroservice(this RequestContext requestContext) =>
        requestContext.User?.Claims.Contains(new Claim(ClaimTypes.Actor, JwtTokenParameters.MicroService)) ?? false;
}