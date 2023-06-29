using GameLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Utilities.Json
{
    public class GameConverter : JsonConverter<Game>
    {
        public override Game Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Game value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
