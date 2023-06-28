using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4Game
{
    public class Board
    {
        private const int EmptySlot = 0;
        public int[,] matrix
        {
            get
            {
                return (int[,])_matrix.Clone();
            }
        }

        private int[,] _matrix;

        public Board(int rows, int columns)
        {
            _matrix = new int[rows, columns]; // Creating a private boolean board
        }

        public bool IsValidMove(int column)
        {
            // Check if the column is within the valid range
            if (column < 0 || column >= _matrix.GetLength(1))
            {
                return false;
            }
            // Check if the topmost slot in the column is free
            return _matrix[0, column] == EmptySlot;
        }
        public bool IsMatrixFull()
        {
            // Check if the topmost slot in each column is already taken
            for(int i = 0; i < _matrix.GetLength(1); i++)
            {
                if(_matrix[0, i] == EmptySlot)
                    return false;
            }
            return true;
        }

        public void MakeMove(int column, Player player) 
        {
            // Find the lowest free row in the column and fill it
            int numberOfRows = _matrix.GetLength(0); 
            for (int row = numberOfRows - 1; row >= 0; row--)
            {
                if (_matrix[row, column] == EmptySlot)
                {
                    _matrix[row, column] = ((int)player);
                    break;
                }
            }
        }

        public void UndoMove(int column)
        {
            // Find the highest filled row in the column and empty it
            int numberOfRows = _matrix.GetLength(0);
            for (int row = 0; row < numberOfRows; row++)
            {
                if (_matrix[row, column] != EmptySlot)
                {
                    _matrix[row, column] = EmptySlot;
                    break;
                }
            }
        }

        public Player? IsWinningMove(int column)
        {
            // Get the board size
            int numRows = _matrix.GetLength(0);
            int numColumns = _matrix.GetLength(1);

            // Find the row for the last move in the column
            int row = numRows - 1;
            while (row >= 0 && _matrix[row, column] != 0)
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
                    if (r >= 0 && r < numRows && c >= 0 && c < numColumns && _matrix[r, c] == _matrix[row, column])
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
                    if (r >= 0 && r < numRows && c >= 0 && c < numColumns && _matrix[r, c] == _matrix[row, column])
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
                    return _matrix[row, column] == 1 ? Player.Human : Player.Ai;
                }
            }

            // If no player has won, return null
            return null;
        }


    }
}
