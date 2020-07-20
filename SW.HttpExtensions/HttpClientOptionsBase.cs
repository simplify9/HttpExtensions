using System;
using System.Collections.Generic;
using System.Text;

namespace SW.HttpExtensions
{
    public abstract class HttpClientOptionsBase
    {
        protected HttpClientOptionsBase()
        {
            Token = new JwtTokenParameters();
            ApiKey = new ApiKeyParameters();
        }

        public abstract string ConfigurationSection { get; }
        public string BaseUrl { get; set; }
        public JwtTokenParameters Token { get; set; }
        public ApiKeyParameters ApiKey { get; set; }
    }
}
