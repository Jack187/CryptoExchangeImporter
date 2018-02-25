using BitfinexApi.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BitfinexApi.JsonConverters
{
    public class WalletsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<Wallet>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);

            var results = new List<Wallet>();

            foreach (var item in array)
            {
                var wallet = JsonConvert.DeserializeObject<Wallet>(item.ToString(), new WalletConverter());
                results.Add(wallet);
            }

            return results;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class WalletConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Wallet);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
                        
            return new Wallet()
            {
                WalletType = array[0].ToObject<string>(),
                Currency = array[1].ToObject<string>(),
                Balance = array[2].ToObject<double>(),
                UnsettledInterest = array[3].ToObject<double>(),
                BalanceAvailable = array[4].ToObject<double?>()
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}