using System;
using System.Collections.Generic;
using System.Text;

namespace SW.HttpExtensions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ApiPathAttribute : Attribute
    {
        public ApiPathAttribute(string path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public string Path { get; }
    }
}
