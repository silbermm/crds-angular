using System;
using Newtonsoft.Json.Linq;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public static class StripeExpandableProperty<T> where T : StripeObject
    {
        public static void Map(object value, Action<string> updateId, Action<T> updateObject)
        {
            if (value is JObject)
            {
                T item = ((JToken)value).ToObject<T>();
                updateId(item.Id);
                updateObject(item);
            }
            else if (value is string)
            {
                updateId((string)value);
                updateObject(null);
            }
        }
    }
}