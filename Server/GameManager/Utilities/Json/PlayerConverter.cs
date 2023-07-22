using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using GameManager.Models;

namespace GameManager.Utilities.Json
{
    public class PlayerConverter : JsonConverter<Player>
    {
        public override Player ReadJson(JsonReader reader, Type objectType, Player existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            var player = new Player(); //(int)jObject["Id"]
            player.games = (ICollection<Connect4Game.Game>)jObject["Games"];
            player.playerName = (string)jObject["PlayerName"];
            player.playerId = (int)jObject["PlayerId"];
            player.phoneNumber = (string)jObject["PhoneNumber"];
            player.country = (string)jObject["Country"];
            player.password = (string)jObject["Password"];

            return player;
        }

        public override void WriteJson(JsonWriter writer, Player value, JsonSerializer serializer)
        {
            Player player = (Player)value;
            JObject jObject = new JObject
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
