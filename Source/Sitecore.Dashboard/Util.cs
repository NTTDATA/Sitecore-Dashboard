using System;
using System.IO;
//using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Sitecore.Dashboard
{
    public static class Util
    {
        public static string ToJson<T>(this T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string GetFieldValueOrDefault(this Item item, string fieldId)
        {
            return item.GetFieldValueOrDefault(fieldId, "");
        }

        public static string GetFieldValueOrDefault(this Item item, string fieldId, string defaultValue)
        {
            if (item.Fields[fieldId] == null)
                return defaultValue;

            return item.Fields[fieldId].Value ?? defaultValue;
        }

        public static TResult ValueOrDefault<T, TResult>(this T source, Func<T, TResult> resultGetter) where T : class
        {
            Assert.ArgumentNotNull(resultGetter, "resultGetter");
            if (source == null)
            {
                return default(TResult);
            }
            return resultGetter(source);
        }
    }
}