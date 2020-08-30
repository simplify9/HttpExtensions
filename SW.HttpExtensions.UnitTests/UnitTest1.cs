using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace SW.HttpExtensions.UnitTests
{
    [TestClass]
    public class UnitTest1
    {

        private class TestInstance
        {
            public int Q { get; set; }
            public int Paging { get; set; }
            public string Stuff { get; set; }
            public List<int> ConcreteList { get; set; }
            public IEnumerable<string> Stuffs { get; set; }
            public string[] SupposedlyEmpty { get; set; }
            public string Q2 { get; set; }

            public override bool Equals(object obj)
            {
                return obj is TestInstance instance &&
                       Q == instance.Q &&
                       Paging == instance.Paging &&
                       Stuff == instance.Stuff &&
                       instance.Stuffs.All(item => Stuffs.Contains(item)) &&
                       instance.SupposedlyEmpty == null &&
                       instance.ConcreteList.All(item => ConcreteList.Contains(item)) &&
                       Q2 == instance.Q2;
            }

            public override int GetHashCode()
            {
                return System.HashCode.Combine(Q, Paging, Stuff, Stuffs, Q2);
            }
        }

        [TestMethod]
        public void TestFromQuery()
        {
            QueryString queryString = new QueryString("?q=1&q2=Hello There&paging=12&stuff=TestWords&Stuffs=1&Stuffs=2&ConcreteList=3&ConcreteList=4");
            HttpRequest rq = new DefaultHttpContext().Request;
            rq.QueryString = queryString;
            var queryCollection = rq.Query;

            TestInstance instance = queryCollection.GetInstance<TestInstance>();
            TestInstance trueInstance = new TestInstance
            {
                Q = 1,
                Q2 = "Hello There",
                Paging = 12,
                Stuff = "TestWords",
                Stuffs = new string[] { "1", "2"},
                ConcreteList = new List<int> { 3, 4}
            };
            Assert.AreEqual(instance, trueInstance);
        }
        [TestMethod] 
        public void TestToQuery()
        {
            var a = new
            {
                Name = "Test name",
                Age = 90,
                TestVar = 10.0f,
                Test2 = 500,
                ListTest = new List<string> { "okay1", "okay2", "okay3"}
            };
            var str = a.ToQueryString();
            var trueCase = "Name=Test name&Age=90&TestVar=10&Test2=500&ListTest=okay1&ListTest=okay2&ListTest=okay3";
            Assert.AreEqual(str, trueCase);
        }

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
