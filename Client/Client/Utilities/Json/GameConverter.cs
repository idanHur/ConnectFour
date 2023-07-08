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
            int gameId = (int)jObject["GameId"];
            game.gameId = gameId;

            // Deserialize the Board array
            JArray boardArray = (JArray)jObject["Board"];
            if (boardArray != null)
            {
                // Deserialize the board values
                int[,] board = boardArray.ToObject<int[,]>();
                game.board = board;
            }

            // Deserialize the GameStatus
            GameStatus gameStatus = jObject["GameStatus"].ToObject<GameStatus>();
            game.gameStatus = gameStatus;

            // Deserialize the Moves array
            JArray movesArray = (JArray)jObject["Moves"];
            if (movesArray != null)
            {
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
            { "GameId", game.gameId },
            { "Board", JArray.FromObject(game.board) },
            { "GameStatus", JToken.FromObject(game.gameStatus) }
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
