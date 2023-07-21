using GameLogicClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities.Json
{
    public class GameConverter : JsonConverter<Game>
    {
        public override Game ReadJson(JsonReader reader, Type objectType, Game existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            // Create a new Game object with the extracted properties
            Game game = new Game();

            // Extract the necessary properties from the JSON
            int gameId = (int)jObject["gameId"];
            game.gameId = gameId;

            // Retrieve the board object
            var boardObject = (JObject)jObject["board"];

            // Extract the matrix from the board object
            game.board = (string)boardObject["matrix"];

            // Deserialize the GameStatus
            GameStatus gameStatus = jObject["gameStatus"].ToObject<GameStatus>();
            game.gameStatus = gameStatus;

            // Check if "Moves" exists and is an array
            if (jObject["Moves"] != null && jObject["Moves"].Type == JTokenType.Array)
            {
                // Cast to JArray
                JArray movesArray = (JArray)jObject["Moves"];

                // Deserialize each Move object in the array
                foreach (JToken moveToken in movesArray)
                {
                    Move move = moveToken.ToObject<Move>(serializer);
                    game.moves.Add(move);
                }
            }

            return game;
        }


        public override void WriteJson(JsonWriter writer, Game value, JsonSerializer serializer)
        {
            Game game = (Game)value;
            JObject jObject = new JObject
        {
            { "gameId", game.gameId },
            { "board", game.board },
            { "gameStatus", JToken.FromObject(game.gameStatus) },
            { "moves", JArray.FromObject(game.moves) }
        };

            // Serialize the Moves array
            JArray movesArray = new JArray();
            foreach (Move move in game.moves)
            {
                JToken moveToken = JToken.FromObject(move, serializer);
                movesArray.Add(moveToken);
            }
            jObject["Moves"] = movesArray;

            jObject.WriteTo(writer);
        }
    }
}
