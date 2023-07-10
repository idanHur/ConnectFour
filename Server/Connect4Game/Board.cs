using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4Game
{
    public class Board
    {
        public int id {  get; set; }
        private const int EmptySlot = 0;

        public string matrix; // EF Core dosnt supports int[,]

        public Board(int rows, int columns)
        {
            matrix = ""; // Creating a private boolean board
        }
        // Default constructor (required by EF Core)
        private Board() { }
        public int[,] GetMatrix() // New method
        {
            var rows = this.matrix.Split(';');
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
            this.matrix = sb.ToString();
        }
        public bool IsValidMove(int column)
        {
            int[,] matrix = GetMatrix();
            // Check if the column is within the valid range
            if (column < 0 || column >= matrix.GetLength(1))
            {
                return false;
            }
            // Check if the topmost slot in the column is free
            return matrix[0, column] == EmptySlot;
        }
        public bool IsMatrixFull()
        {
            int[,] matrix = GetMatrix();
            // Check if the topmost slot in each column is already taken
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if(matrix[0, i] == EmptySlot)
                    return false;
            }
            return true;
        }

        public void MakeMove(int column, Player player) 
        {
            int[,] matrix = GetMatrix();
            // Find the lowest free row in the column and fill it
            int numberOfRows = matrix.GetLength(0); 
            for (int row = numberOfRows - 1; row >= 0; row--)
            {
                if (matrix[row, column] == EmptySlot)
                {
                    matrix[row, column] = ((int)player);
                    break;
                }
            }
            UpdateMatrix(matrix);
        }

        public void UndoMove(int column)
        {
            int[,] matrix = GetMatrix();
            // Find the highest filled row in the column and empty it
            int numberOfRows = matrix.GetLength(0);
            for (int row = 0; row < numberOfRows; row++)
            {
                if (matrix[row, column] != EmptySlot)
                {
                    matrix[row, column] = EmptySlot;
                    break;
                }
            }
            UpdateMatrix(matrix);
        }

        public Player? IsWinningMove(int column)
        {
            int[,] matrix = GetMatrix();
            // Get the board size
            int numRows = matrix.GetLength(0);
            int numColumns = matrix.GetLength(1);

            // Find the row for the last move in the column
            int row = numRows - 1;
            while (row >= 0 && matrix[row, column] != 0)
            {
                row--;
            }
            row++;

            // Define directions as tuples (dr, dc) for vertical, horizontal, and both diagonals
            var directions = new List<(int, int)> { (-1, 0), (0, 1), (-1, 1), (-1, -1) };

            foreach (var direction in directions)
            {
                int count = 1;

                // Check in the forward direction
                for (int i = 1; i < 4; i++)
                {
                    int r = row + i * direction.Item1;
                    int c = column + i * direction.Item2;
                    if (r >= 0 && r < numRows && c >= 0 && c < numColumns && matrix[r, c] == matrix[row, column])
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                // Check in the backward direction
                for (int i = 1; i < 4; i++)
                {
                    int r = row - i * direction.Item1;
                    int c = column - i * direction.Item2;
                    if (r >= 0 && r < numRows && c >= 0 && c < numColumns && matrix[r, c] == matrix[row, column])
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                // If we have found 4 in a row, return the player who won
                if (count >= 4)
                {
                    return matrix[row, column] == 1 ? Player.Human : Player.Ai;
                }
            }

            // If no player has won, return null
            return null;
        }


    }
}
