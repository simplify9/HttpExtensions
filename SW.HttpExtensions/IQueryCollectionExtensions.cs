using Microsoft.AspNetCore.Http;
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

        public static object GetInstance(this IQueryCollection queryCollection, Type type)
        {
            try
            {
                var obj = Activator.CreateInstance(type);
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    object value = null;
                    Type propType = property.PropertyType;
                    if (propType.IsInterface)
                        throw new SWException($"Type of {property.Name} is an interface. If it's a collection type, consider using List<> or an array.");

                    var queries = queryCollection[property.Name];

                    bool isEnumerable = property.PropertyType.GetInterface(nameof(IEnumerable)) != null;

                    if (isEnumerable)
                    {
                        Type nested = propType.GetElementType() ?? propType.GetGenericArguments()[0];
                        Array tmp = Array.CreateInstance(nested, queries.Count);

                        var queryObjects = queries.Select(q => q.ConvertValueToType(nested));
                        for (int i = 0; i < queries.Count; i++)
                            tmp.SetValue(queryObjects.ElementAt(i), i);

                        Type listType = propType.IsInterface ? typeof(List<object>) : propType;
                        bool isArray = propType.IsArray;
                        value = isArray ? tmp : Activator.CreateInstance(listType, new object[] { new object[] { tmp } });
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
