using GameLogic.Models;
using Newtonsoft.Json;
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
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, Move value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
