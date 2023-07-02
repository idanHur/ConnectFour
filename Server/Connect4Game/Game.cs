﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Connect4Game
{
    public enum Player
    {
        Human = 1,
        Ai = 2
    }
    public enum GameStatus
    {
        Won,
        Lost,
        Draw,
        OnGoing
    }
    public class Game
    {

        private Board board;
        public GameStatus gameStatus { get; private set; }
        public Player currentPlayer { get; private set; }

        public DateTime startTime { get; private set; }
        public TimeSpan gameDuration { get; private set; }
        public int gameId { get; private set; }
        public int playerId { get; private set; }

        // EF Core will automatically load the related Move entities when accessing this property.
        public ICollection<Move> Moves { get; set; }

        public Game(int rows, int columns, int gameId, int playerId)
        {
            this.gameId = gameId;
            board = new Board(rows, columns);
            gameStatus = GameStatus.OnGoing;
            startTime = DateTime.Now;
            this.playerId = playerId;
        }
        private Game() { }

        private void EndGame(bool draw = false)
        {
            if(draw)
                gameStatus = GameStatus.Draw;
            else if(currentPlayer == Player.Human)
                gameStatus = GameStatus.Won;
            else
                gameStatus = GameStatus.Lost;
            gameDuration = DateTime.Now - startTime;
        }
        public int[,] GetGameBoard()
        {
            return board.matrix;
        }

        public bool PlayerMove(int column)
        {
            if ((gameStatus != GameStatus.OnGoing) || currentPlayer != Player.Human)
                return false;
            if(board.IsMatrixFull())
            {
                EndGame(true);
                return false;
            }
            if (board.IsValidMove(column)) // Check if the move is possible
            {
                board.MakeMove(column, Player.Human);
                if (board.IsWinningMove(column) != null) // Check if the game ended
                {
                    EndGame();
                }
                Moves.Add(new Move(column, currentPlayer, Moves.Count+1));
                currentPlayer = Player.Ai;
                return true;
            }
            else
            {
                return false; // The move wasnt made
            }
        }
        public void AiMove()
        {
            if ((gameStatus != GameStatus.OnGoing) || currentPlayer != Player.Ai)
                return;
            if (board.IsMatrixFull())
            {
                EndGame(true);
                return;
            }
            int columns = board.matrix.GetLength(1);
            int winningMove = -1;
            int blockMove = -1;

            // Check for a winning move or a move to block the opponent's win
            for (int col = 0; col < columns; col++)
            {
                if (board.IsValidMove(col))
                {
                    // Check if making a move in this column would result in a win
                    board.MakeMove(col, Player.Ai);
                    if (board.IsWinningMove(col) == Player.Ai)
                    {
                        winningMove = col;
                    }
                    board.UndoMove(col);

                    // Check if the opponent could win in the next turn by making a move in this column
                    board.MakeMove(col, Player.Ai);
                    board.MakeMove(col, Player.Human);  // Pretend the opponent also makes a move in the same column
                    if (board.IsWinningMove(col) == Player.Human)
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
                board.MakeMove(winningMove, Player.Ai);
                Moves.Add(new Move(winningMove, currentPlayer, Moves.Count + 1));
                EndGame();
                return;
            }

            // If the opponent has a winning move available next turn, block it
            if (blockMove != -1)
            {
                board.MakeMove(blockMove, Player.Ai);
                Moves.Add(new Move(blockMove, currentPlayer, Moves.Count + 1));
                currentPlayer = Player.Human;
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
            Moves.Add(new Move(column, currentPlayer, Moves.Count + 1));
            board.MakeMove(column, Player.Ai);
            currentPlayer = Player.Human;
        }
    }
}