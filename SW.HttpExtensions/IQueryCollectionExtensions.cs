using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SW.PrimitiveTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SW.HttpExtensions
{
    public static class IQueryCollectionExtensions
    {

        public static T GetInstance<T>(this IQueryCollection queryCollection)
        {
            return (T)queryCollection.GetInstance(typeof(T)); 
        }

        private static Array GetDynamicArray(Type nested, int count, IEnumerable<object> contents)
        {
            Array tmp = Array.CreateInstance(nested, count);
            for (int i = 0; i < count; i++)
                tmp.SetValue(contents.ElementAt(i), i);
            return tmp;
        }

        private static void AssignEnumerable(int count, Type propType, StringValues queries, out dynamic value)
        {

            if (count == 0)
            {
                value = null;
                return;
            }

            // Element type if array, gen arg if complex enumerable type.
            Type nested = propType.GetElementType() ?? propType.GetGenericArguments()[0];
            //Generate Array type to construct enumerable members.
            var dynamicArray = GetDynamicArray(nested, count, queries.Select(q => q.ConvertValueToType(nested)));

            bool isArray = propType.IsArray;

            if (propType.IsArray)
            {
                value = dynamicArray;
                return;
            }

            // Interface types can not be constructed, assigned a list.
            if (propType.IsInterface)
            {
                propType = typeof(List<>);
                propType = propType.MakeGenericType(nested);
            }

            value = Activator.CreateInstance(propType);

            foreach(var item in dynamicArray)
                value.Add((dynamic)item.ConvertValueToType(nested));
        }

        public static object GetInstance(this IQueryCollection queryCollection, Type type)
        {
            try
            {
                var obj = Activator.CreateInstance(type);
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    dynamic value = null;
                    Type propType = property.PropertyType;

                    var queries = queryCollection[property.Name];

                    bool isEnumerable = property.PropertyType.GetInterface(nameof(IEnumerable)) != null && property.PropertyType != typeof(string);

                    if (isEnumerable)
                    {
                        AssignEnumerable(queries.Count, propType, queries, out dynamic enumerableValue);
                        value = enumerableValue;
                    }
                    else
                    {
                        value = queries.FirstOrDefault()
                                .ConvertValueToType(propType);
                    }

                    if (value == null)
                        continue;

                    property.SetValue(obj, value, null);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new SWException($"Error constructing type: '{type.Name}' from parameters. {ex.Message}");

            }
        }
    }
}
