using System;

namespace Connect4Game
{
    public class Game
    {
        private Board board;
        public bool gameEnded;

        public Game(int rows, int columns)
        {
            board = new Board(rows, columns);
            gameEnded = false;
        }



        public bool PlayerMove(int column)
        {
            if (board.IsValidMove(column))
            {
                board.MakeMove(column);
                if (board.IsWinningMove(column))
                {
                    Console.WriteLine("Player has won!");
                    gameEnded = true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public void AiMove()
        {
            int columns = board.GetBoard().GetLength(1);
            int winningMove = -1;
            int blockMove = -1;

            // Check for a winning move or a move to block the opponent's win
            for (int col = 0; col < columns; col++)
            {
                if (board.IsValidMove(col))
                {
                    // Check if making a move in this column would result in a win
                    board.MakeMove(col);
                    if (board.IsWinningMove(col))
                    {
                        winningMove = col;
                    }
                    board.UndoMove(col);

                    // Check if the opponent could win in the next turn by making a move in this column
                    board.MakeMove(col);
                    board.MakeMove(col);  // Pretend the opponent also makes a move in the same column
                    if (board.IsWinningMove(col))
                    {
                        blockMove = col;
                    }
                    board.UndoMove(col);
                    board.UndoMove(col);
                }
            }

            // If there's a winning move available, take it
            if (winningMove != -1)
            {
                board.MakeMove(winningMove);
                gameEnded = true;
                return;
            }

            // If the opponent has a winning move available next turn, block it
            if (blockMove != -1)
            {
                board.MakeMove(blockMove);
                return;
            }

            // If neither of those apply, make a random move
            Random rnd = new Random();
            int column;
            do
            {
                column = rnd.Next(columns);
            }
            while (!board.IsValidMove(column));

            board.MakeMove(column);
        }


        public bool GetBoard()
        {
            return board;
        }



    }
}
