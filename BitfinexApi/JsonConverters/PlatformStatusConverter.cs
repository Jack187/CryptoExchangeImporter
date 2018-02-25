using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using BitfinexApi.Resources;

namespace BitfinexApi.JsonConverters
{
    public class PlatformStatusConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PlatformStatus);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);

            if (array.Count == 0 || array[0].Type != JTokenType.Integer)
                return null;

            return new PlatformStatus() { Operative = array[0].ToObject<int>() };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}