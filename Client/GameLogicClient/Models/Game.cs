using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Models
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
        public string board { get; set; } // EF Core dosnt supports int[,]
        public GameStatus gameStatus { get; set; }
        public int gameId { get;  set; }

        public ICollection<Move> moves { get; set; }

        public Game(int rows, int columns, int gameId)
        {
            this.gameId = gameId;
            board = "";
            gameStatus = GameStatus.OnGoing;
            moves = new List<Move>();
        }
        public Game()
        {
            moves = new List<Move>();
        }
        

        public int[,] GetMatrix() // New method
        {
            var rows = this.board.Split(';');
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
            this.board = sb.ToString();
        }

        public void UpdateGameState(string board, GameStatus gameStatus, int lastMoveCol, PlayerType whichPlayer)
        {
            this.board = board;
            this.gameStatus = gameStatus;
            Move newMove = new Move(lastMoveCol, whichPlayer, moves.Count + 1);
        }

        public bool IsEligibleMove(int column, PlayerType player)
        {
            int[,] board = GetMatrix();
            // Check if the column is valid
            if (column < 0 || column >= board.GetLength(1)) // use board size
            {
                throw new ArgumentOutOfRangeException("Column must be within the board size");
            }

            // Find the lowest empty row in the specified column
            for (int row = board.GetLength(0) - 1; row >= 0; row--) // use board size
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