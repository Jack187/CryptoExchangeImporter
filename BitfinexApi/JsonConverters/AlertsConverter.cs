using System;
using System.Collections.Generic;
using BitfinexApi.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BitfinexApi.JsonConverters
{
    public class AlertsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<Alert>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);

            var results = new List<Alert>();
            if (array.Count == 0)
                return null;

            foreach (var item in array)
            {
                var alert = BitfinexApiClient.DeserializeObject<Alert>(item.ToString(), new AlertConverter());
                results.Add(alert);
            }

            return results;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class AlertConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Alert);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);

            if (array.Count == 0)
                return null;
            
            return new Alert()
            {

                Id = array[0].ToString(),
                Type = array[1].ToString(),
                Symbol = array[2].ToString(),
                Price = Convert.ToDouble(array[3]),
                Unknown = Convert.ToInt32(array[4])
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}