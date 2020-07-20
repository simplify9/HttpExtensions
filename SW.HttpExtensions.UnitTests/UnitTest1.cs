using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;

namespace SW.HttpExtensions.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestReadWriteJwt()
        {
            var jwtTokenParameters = new JwtTokenParameters
            {
                Audience = "local",
                Issuer = "local",
                Key = "8768747658765975758758758746"
            };

            var claimsIdentity = new ClaimsIdentity("testauth");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "111"));
            var jwt = jwtTokenParameters.WriteJwt(claimsIdentity);

            var claimsPrincipal = jwtTokenParameters.ReadJwt(jwt);
        }
    }
}
