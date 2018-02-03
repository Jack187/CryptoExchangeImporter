using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using BfAPI.Resources;

namespace BfAPI.JsonConverters
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

            if (array.Count == 0)
                return null;

            return new PlatformStatus() { Operative = Convert.ToInt32(array[0]) };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}