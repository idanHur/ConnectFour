using GameLogic.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities.Json
{
    public class MoveConveter : JsonConverter<Move>
    {
        public override Move ReadJson(JsonReader reader, Type objectType, Move existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            // Extract the necessary properties from the JSON
            int columnNumber = (int)jObject["ColumnNumber"];
            PlayerType player = jObject["Player"].ToObject<PlayerType>();
            int id = (int)jObject["Id"];

            // Create a new Move object with the extracted properties
            Move move = new Move(columnNumber, player, id);

            return move;
        }

        public override void WriteJson(JsonWriter writer, Move value, JsonSerializer serializer)
        {
            Move move = (Move)value;
            JObject jObject = new JObject
        {
            { "ColumnNumber", move.columnNumber },
            { "Player", JToken.FromObject(move.Player) },
            { "Id", move.id }
        };

            jObject.WriteTo(writer);
        }
    }
}
