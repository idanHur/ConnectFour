using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4Game
{
    class Board
    {
        private bool[,] board;

        public Board()
        {
            board = new bool[6, 7]; // Creating a private 6x7 boolean board
        }

        public bool IsValidMove(int column)
        {
            // Check if the topmost slot in the column is free
            return !board[5, column];
        }

        public void MakeMove(bool[,] board, int column, bool isAI)
        {
           
        }

        public void UndoMove(bool[,] board, int column)
        {
           
        }

        public bool IsWinningMove(bool[,] board, int column)
        {
            
            throw new NotImplementedException();
        }



    }
}
