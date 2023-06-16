using System;

namespace Connect4Game
{
    public class Game
    {
        private Board board;
        public bool gameEnded;

        public Game()
        {
            board = new Board();
            gameEnded = false;
        }
        

        
        public void SetBoardValue(int row, int column, bool value)
        {
            board[row, column] = value;
        }

        
        public bool GetBoard(int row, int column)
        {
            return board[row, column];
        }



    }
}
