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

            var player = new Player((int)jObject["Id"]);

            return player;
        }

        public override void WriteJson(JsonWriter writer, Player value, JsonSerializer serializer)
        {
            Player player = (Player)value;
            JObject jObject = new JObject
            {
                { "Id", player.id }
            };

            jObject.WriteTo(writer);
        }
    
    }
}
