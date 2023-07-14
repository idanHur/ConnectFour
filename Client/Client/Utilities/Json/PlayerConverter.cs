using GameLogic.Models;
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
                player.games = jObject["Games"].ToObject<List<Game>>();
            }

            player.playerName = (string)jObject["PlayerName"];
            player.playerId = (int)jObject["PlayerId"];
            player.phoneNumber = (string)jObject["PhoneNumber"];
            player.country = (string)jObject["Country"];

            return player;
        }


        public override void WriteJson(JsonWriter writer, Player value, JsonSerializer serializer)
        {
            var player = (Player)value;
            var jObject = new JObject
            {
                { "PlayerId", player.playerId },
                { "PlayerName", player.playerName },
                { "PhoneNumber", player.phoneNumber },
                { "Country", player.country },
                { "Games", JArray.FromObject(player.games) }
            };

            jObject.WriteTo(writer);

        }
    }
}
