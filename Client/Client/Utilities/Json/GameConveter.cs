using GameLogic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities.Json
{
    public class GameConveter : JsonConverter<Game>
    {
        public override Game ReadJson(JsonReader reader, Type objectType, Game existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, Game value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
