using BfAPI.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BfAPI.JsonConverters
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
            if (array.Count == 0)
                return null;

            foreach (var item in array)
            {
                var wallet = BitfinexRestClient.DeserializeObject<Wallet>(item.ToString(), new WalletConverter());
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

            if (array.Count == 0)
                return null;

            var t = array[4];

            double? balanceAvailable = Convert.ToDouble(array[4].ToObject<object>());// != null ? Convert.ToDouble(array[4]) : -1.0;

            return new Wallet()
            {
                WalletType = array[0].ToString(),
                Currency = array[1].ToString(),
                Balance = Convert.ToDouble(array[2]),
                UnsettledInterest = Convert.ToDouble(array[3]),
                BalanceAvailable = balanceAvailable
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}