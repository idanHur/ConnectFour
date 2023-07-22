using System;
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
        OnGoing,
        DNF
    }
    public class Game
    {

        public Board Board{ get; set; }
        public GameStatus Status { get; set; }
        public Player CurrentPlayer { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan GameDuration { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; }

        // EF Core will automatically load the related Move entities when accessing this property.
        public ICollection<Move> Moves { get; set; }

        public Game(int rows, int columns, int playerId)
        {
            Board = new Board(rows, columns);
            Status = GameStatus.OnGoing;
            StartTime = DateTime.Now;
            CurrentPlayer = Player.Human;
            this.PlayerId = playerId;
            Moves = new List<Move>();
        }
        public Game() { }

        public void EndGame(bool draw = false, bool didntFinish = false)
        {
            if (Status != GameStatus.OnGoing) return;
            if(draw)
                Status = GameStatus.Draw;
            else if(didntFinish)
                Status = GameStatus.DNF;
            else if(CurrentPlayer == Player.Human)
                Status = GameStatus.Won;
            else
                Status = GameStatus.Lost;
            GameDuration = DateTime.Now - StartTime;
        }

        public Move PlayerMove(int column)
        {
            if ((Status != GameStatus.OnGoing) || (CurrentPlayer != Player.Human))
                throw new InvalidOperationException("Cant make move!");
            if (Board.IsMatrixFull())
            {
                EndGame(true);
                throw new InvalidOperationException("The game Board is full");
            }
            if (Board.IsValidMove(column)) // Check if the move is possible
            {
                Board.MakeMove(column, Player.Human);
                if (Board.IsWinningMove(column) != null) // Check if the game ended
                {
                    EndGame();
                }
                Move newMove = new Move(column, CurrentPlayer);
                Moves.Add(newMove);
                CurrentPlayer = Player.Ai;
                return newMove;
            }
            else
            {
                throw new InvalidOperationException("The move isnt valid"); // The move wasnt made
            }
        }
        public void AiMove()
        {
            if ((Status != GameStatus.OnGoing) || CurrentPlayer != Player.Ai)
                throw new InvalidOperationException("Cant make move!");
            if (Board.IsMatrixFull())
            {
                EndGame(true);
                throw new InvalidOperationException("The game Board is full");
            }
            int columns = Board.GetMatrix().GetLength(1);
            int winningMove = -1;
            int blockMove = -1;

            // Check for a winning move or a move to block the opponent's win
            for (int col = 0; col < columns; col++)
            {
                if (Board.IsValidMove(col))
                {
                    // Check if making a move in this column would result in a win
                    Board.MakeMove(col, Player.Ai);
                    if (Board.IsWinningMove(col) == Player.Ai)
                    {
                        winningMove = col;
                    }
                    Board.UndoMove(col);

                    // Check if the opponent could win in the next turn by making a move in this column
                    Board.MakeMove(col, Player.Ai);
                    Board.MakeMove(col, Player.Human);  // Pretend the opponent also makes a move in the same column
                    if (Board.IsWinningMove(col) == Player.Human)
                    {
                        blockMove = col;
                    }
                    Board.UndoMove(col);
                    Board.UndoMove(col);

                    Board.MakeMove(col, Player.Human);  // Pretend the opponent makes a move in this column and ai is in diffrent unrelated col
                    if (Board.IsWinningMove(col) == Player.Human)
                    {
                        blockMove = col;
                    }
                    Board.UndoMove(col);
                }
            }

            // If there's a winning move available, take it
            if (winningMove != -1)
            {
                Board.MakeMove(winningMove, Player.Ai);
                Moves.Add(new Move(winningMove, CurrentPlayer));
                EndGame();
                return;
            }

            // If the opponent has a winning move available next turn, block it
            if (blockMove != -1)
            {
                Board.MakeMove(blockMove, Player.Ai);
                Moves.Add(new Move(blockMove, CurrentPlayer));
                CurrentPlayer = Player.Human;
                return;
            }

            // If neither of those apply, make a random move
            Random rnd = new Random();
            int column;
            do
            {
                column = rnd.Next(columns);
            }
            while (!Board.IsValidMove(column));
            Moves.Add(new Move(column, CurrentPlayer));
            Board.MakeMove(column, Player.Ai);
            CurrentPlayer = Player.Human;
        }
    }
}
