﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Connect4Game
{
    public class Game
    {
        private Board board;
        public bool gameEnded { get; private set; }
        public DateTime startTime { get; private set; }
        public TimeSpan gameDuration { get; private set; }
        private List<Move> _movesRecord = new List<Move>();

        public ReadOnlyCollection<Move> movesRecord => _movesRecord.AsReadOnly();

        public Game(int rows, int columns)
        {
            board = new Board(rows, columns);
            gameEnded = false;
            startTime = DateTime.Now;
        }

        private void EndGame()
        {
            gameEnded = true;
            gameDuration = DateTime.Now - startTime;
        }
        public bool[,] GetGameBoard()
        {
            return board.matrix;
        }

        public bool PlayerMove(int column)
        {
            if (gameEnded)
                return false;
            if (board.IsValidMove(column))
            {
                board.MakeMove(column);
                if (board.IsWinningMove(column))
                {
                    EndGame();
                }
                _movesRecord.Add(new Move(column, true));
                return true;
            }
            else
            {
                return false;
            }
        }
        public void AiMove()
        {
            if (gameEnded)
                return;

            int columns = board.matrix.GetLength(1);
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
                _movesRecord.Add(new Move(winningMove, false));
                EndGame();
                return;
            }

            // If the opponent has a winning move available next turn, block it
            if (blockMove != -1)
            {
                board.MakeMove(blockMove);
                _movesRecord.Add(new Move(blockMove, false));
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
            _movesRecord.Add(new Move(column, false));
            board.MakeMove(column);
        }
    }


}
