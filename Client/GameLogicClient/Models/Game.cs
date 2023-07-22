using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogicClient.Models
{
    public enum PlayerType
    {
        Human = 1,
        Ai = 2
    }
    public enum GameStatus
    {
        Won,
        Lost,
        Draw,
        OnGoing,
        DNF
    }
    public class Game
    {
        public string Board { get; set; } // EF Core dosnt supports int[,]
        public GameStatus Status { get; set; }
        public int GameId { get;  set; }
        public int PlayerId { get; set; } // Add PlayerId property
        public Player Player { get; set; } // Add navigation property for Player

        public ICollection<Move> Moves { get; set; }

        public Game(int rows, int columns, int gameId)
        {
            this.GameId = gameId;
            Board = "";
            Status = GameStatus.OnGoing;
            Moves = new List<Move>();
        }
        public Game()
        {
            Moves = new List<Move>();
        }
        

        public int[,] GetMatrix() // New method
        {
            var rows = this.Board.Split(';');
            var matrix = new int[rows.Length, rows[0].Split(',').Length];

            for (int i = 0; i < rows.Length; i++)
            {
                var values = rows[i].Split(',');

                for (int j = 0; j < values.Length; j++)
                {
                    matrix[i, j] = int.Parse(values[j]);
                }
            }

            return matrix;
        }
        private void UpdateMatrix(int[,] matrix)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sb.Append(matrix[i, j]);

                    if (j != matrix.GetLength(1) - 1)
                    {
                        sb.Append(",");
                    }
                }

                if (i != matrix.GetLength(0) - 1)
                {
                    sb.Append(";");
                }
            }
            this.Board = sb.ToString();
        }

        public void UpdateGameState(string board, GameStatus gameStatus, int lastMoveCol, PlayerType whichPlayer)
        {
            this.Board = board;
            this.Status = gameStatus;
            Move newMove = new Move(lastMoveCol, whichPlayer, Moves.Count + 1);
        }

        public bool IsEligibleMove(int column, PlayerType player)
        {
            int[,] board = GetMatrix();
            // Check if the column is valid
            if (column < 0 || column >= board.GetLength(1)) // use Board size
            {
                throw new ArgumentOutOfRangeException("Column must be within the Board size");
            }

            // Find the lowest empty row in the specified column
            for (int row = board.GetLength(0) - 1; row >= 0; row--) // use Board size
            {
                if (board[row, column] == 0) // If the cell is empty
                {
                    board[row, column] = player == PlayerType.Human ? 1 : 2; ; // Make the move if Human put 1 if Ai put 2
                    return true;
                }
            }
            return false;
        }



    }
}