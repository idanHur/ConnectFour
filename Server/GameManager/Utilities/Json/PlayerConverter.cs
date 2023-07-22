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
            player.Games = (ICollection<Connect4Game.Game>)jObject["Games"];
            player.Name = (string)jObject["Name"];
            player.PlayerId = (int)jObject["PlayerId"];
            player.PhoneNumber = (string)jObject["PhoneNumber"];
            player.Country = (string)jObject["Country"];
            player.Password = (string)jObject["Password"];

            return player;
        }

        public override void WriteJson(JsonWriter writer, Player value, JsonSerializer serializer)
        {
            Player player = (Player)value;
            JObject jObject = new JObject
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
