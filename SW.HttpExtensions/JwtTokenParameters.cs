using System;
using System.Collections.Generic;
using System.Text;

namespace SW.HttpExtensions
{
    public class JwtTokenParameters
    {

        public const string ConfigurationSection = "Token";
        public string Issuer { get; set; }
        public string Key { get; set; }
        public string Audience { get; set; }

        public bool IsValid 
        {
            get
            {
                if (Issuer == null || Key == null || Audience == null) return false;
                return true;
            }
        }
    }
}
