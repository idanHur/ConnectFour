using GameLogicClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Client.Utilities.Json
{
    public class PlayerConverter : JsonConverter<Player>
    {
        public override Player ReadJson(JsonReader reader, Type objectType, Player existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            var player = new Player();

            if (jObject["Games"] != null)
            {
                player.Games = jObject["Games"].ToObject<List<Game>>();
            }

            player.Name = (string)jObject["Name"];
            player.PlayerId = (int)jObject["PlayerId"];
            player.PhoneNumber = (string)jObject["PhoneNumber"];
            player.Country = (string)jObject["Country"];

            return player;
        }


        public override void WriteJson(JsonWriter writer, Player value, JsonSerializer serializer)
        {
            var player = (Player)value;
            var jObject = new JObject
            {
                { "PlayerId", player.PlayerId },
                { "Name", player.Name },
                { "PhoneNumber", player.PhoneNumber },
                { "Country", player.Country },
                { "Games", JArray.FromObject(player.Games) }
            };

            jObject.WriteTo(writer);

        }
    }
}
