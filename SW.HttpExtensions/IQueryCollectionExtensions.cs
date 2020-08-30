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
                    dynamic value = null;
                    Type propType = property.PropertyType;

                    var queries = queryCollection[property.Name];

                    bool isEnumerable = property.PropertyType.GetInterface(nameof(IEnumerable)) != null && property.PropertyType != typeof(string);

                    if (isEnumerable)
                    {
                        bool isArray = propType.IsArray;

                        Type nested = propType.GetElementType() ?? propType.GetGenericArguments()[0];
                        Array tmp = Array.CreateInstance(nested, queries.Count);

                        //Populate tmp array
                        var queryObjects = queries.Select(q => q.ConvertValueToType(nested));
                        for (int i = 0; i < queries.Count; i++)
                            tmp.SetValue(queryObjects.ElementAt(i), i);

                        // Interface types can not be constructed 
                        if (propType.IsInterface)
                        {
                            propType = typeof(List<>);
                            propType = propType.MakeGenericType(nested);
                        }
                        if (isArray)
                        {
                            value = tmp;
                        }
                        else
                        {
                            var dynamicList  = (List<object>)Activator.CreateInstance(typeof(List<object>), new object[] { tmp });
                            value = Activator.CreateInstance(propType);
                            foreach(var item in dynamicList)
                                value.Add((dynamic)item.ConvertValueToType(nested));

                        }
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
