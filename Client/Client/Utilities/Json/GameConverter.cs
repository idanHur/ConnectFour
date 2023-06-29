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

            var game = new Game
            {
                gameId = (int)jObject["Id"],
                gameStatus = (GameStatus)Enum.Parse(typeof(GameStatus), (string)jObject["Status"]),
                moves = jObject["Moves"].ToObject<List<Move>>() // Deserializes the moves

            };

            // Deserialize into a jagged array
            var jaggedArray = jObject["Board"].ToObject<int[][]>();

            // Convert the jagged array into a 2D array
            game.board = new int[jaggedArray.Length, jaggedArray[0].Length];
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                for (int j = 0; j < jaggedArray[0].Length; j++)
                {
                    game.board[i, j] = jaggedArray[i][j];
                }
            }
            return game;
        }

        public override void WriteJson(JsonWriter writer, Game value, JsonSerializer serializer)
        {
            // Convert the 2D array to a jagged array
            int[][] board = new int[value.board.GetLength(0)][];
            for (int i = 0; i < value.board.GetLength(0); i++)
            {
                board[i] = new int[value.board.GetLength(1)];
                for (int j = 0; j < value.board.GetLength(1); j++)
                {
                    board[i][j] = value.board[i, j];
                }
            }

            JObject jObject = new JObject
        {
            { "GameId", value.gameId },
            { "GameStatus", value.gameStatus.ToString()},
            { "Moves", JArray.FromObject(value.moves) }, // Serializes the moves
            { "Board", JArray.FromObject(board) } // Serializes the board
        };

            jObject.WriteTo(writer);
        }
    }
}
