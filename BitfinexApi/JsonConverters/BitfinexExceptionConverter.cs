﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BitfinexApi.JsonConverters
{
    internal class BitfinexExceptionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BitfinexException);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);

            if (array.Count == 0)
                return null;

            JToken error = array[0];
            if (error.Type == JTokenType.String && (string)array[0] == "error")
            {
                return new BitfinexException((int) array[1], (string) array[2]);
            }

            return null;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}