using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CrossroadsStripeOnboarding.Models.Json
{
    public class StripeCustomerDictionaryConverter : JsonConverter
    {
        private readonly Random _random = new Random();

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var valueType = objectType.GetGenericArguments()[1];
            var intermediateDictionaryType = typeof(Dictionary<,>).MakeGenericType(typeof(string), valueType);
            var intermediateDictionary = (IDictionary)Activator.CreateInstance(intermediateDictionaryType);
            serializer.Populate(reader, intermediateDictionary);

            var finalDictionary = (IDictionary)Activator.CreateInstance(objectType);
            foreach (DictionaryEntry pair in intermediateDictionary)
                finalDictionary.Add(pair.Key + "|" + _random.Next(), pair.Value);

            return finalDictionary;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsA(typeof(IDictionary<,>)) &&
                   objectType.GetGenericArguments()[0].IsA(typeof(string)) &&
                   objectType.GetGenericArguments()[1].IsA(typeof(StripeJsonCustomer));
        }
    }

    public static class Extensions
    {
        public static bool IsA(this Type type, Type typeToBe)
        {
            if (!typeToBe.IsGenericTypeDefinition)
                return typeToBe.IsAssignableFrom(type);

            var toCheckTypes = new List<Type> { type };
            if (typeToBe.IsInterface)
                toCheckTypes.AddRange(type.GetInterfaces());

            var basedOn = type;
            while (basedOn.BaseType != null)
            {
                toCheckTypes.Add(basedOn.BaseType);
                basedOn = basedOn.BaseType;
            }

            return toCheckTypes.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeToBe);
        }

    }
}
