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

            // Deserialize the Board array
            if (jObject["board"] is JObject boardObject)
            {
                // Get the matrix property from the board JObject
                JArray matrixArray = (JArray)boardObject["matrix"];
                if (matrixArray != null)
                {
                    // Deserialize the matrix values
                    int[,] matrix = matrixArray.ToObject<int[,]>();
                    game.board = matrix;
                }
            }


            // Deserialize the GameStatus
            GameStatus gameStatus = jObject["gameStatus"].ToObject<GameStatus>();
            game.gameStatus = gameStatus;

            // Check if "moves" exists and is an array
            if (jObject["moves"] != null && jObject["moves"].Type == JTokenType.Array)
            {
                // Cast to JArray
                JArray movesArray = (JArray)jObject["moves"];

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
            { "board", JArray.FromObject(game.board) },
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
