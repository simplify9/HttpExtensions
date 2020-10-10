using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using SW.PrimitiveTypes;

namespace SW.HttpExtensions
{
    public static class ObjectExtensions
    {
        public static string ToQueryString(this object obj)
        {
            string[] props = obj.GetType().GetProperties()
                .Select(property =>
                {

                    bool isEnumerable = property.PropertyType.GetInterface(nameof(IEnumerable)) != null && property.PropertyType != typeof(string);
                    if (isEnumerable)
                    {
                        IEnumerable tmp = ((IEnumerable)property.GetValue(obj).ConvertValueToType(property.PropertyType));
                        IEnumerable<string> enumerable = tmp.Cast<string>();

                        Type nested = property.PropertyType.GetElementType() ?? property.PropertyType.GetGenericArguments()[0];

                        int length = 0;
                        foreach (var val in tmp) ++length;

                        Array array = Array.CreateInstance(nested, length);

                        int counter = 0;
                        foreach (var val in tmp)
                        {
                            array.SetValue(val, counter);
                            ++counter;
                        }

                        string q = string.Empty;
                        for (int i = 0; i < array.Length - 1; i++) q += $"{property.Name}={array.GetValue(i)}&";
                        q += $"{property.Name}={array.GetValue(array.Length - 1)}";

                        return q;
                    }
                    else return $"{property.Name}={property.GetValue(obj)}";
                })
                .ToArray();

            string queryString = string.Empty;

            for (int i = 0; i < props.Count() - 1; i++)
                queryString += props[i] + '&';

            queryString += props[props.Length - 1];

            return queryString;
        }
    }
}
