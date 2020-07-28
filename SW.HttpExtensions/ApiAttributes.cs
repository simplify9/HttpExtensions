using System;
using System.Collections.Generic;
using System.Text;

namespace SW.HttpExtensions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ApiGetAttribute : Attribute
    {
        public ApiGetAttribute(string path)
            => Path = path ?? throw new ArgumentNullException(nameof(path));
        public string Path { get; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ApiPostAttribute : Attribute
    {
        public ApiPostAttribute(string path)
            => Path = path ?? throw new ArgumentNullException(nameof(path));
        public string Path { get; }
    }
}
