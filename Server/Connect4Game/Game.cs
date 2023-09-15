using System;
using System.Collections.Generic;

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
        public Board Board { get; set; }
        public GameStatus Status { get; set; } = GameStatus.OnGoing;
        public Player CurrentPlayer { get; set; } = Player.Human;
        public DateTime StartTime { get; set; } = DateTime.Now;
        public TimeSpan GameDuration { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public ICollection<Move> Moves { get; set; } = new List<Move>();

        public Game(int rows, int columns, int playerId)
        {
            Board = new Board(rows, columns);
            this.PlayerId = playerId;
        }

        public Game() { }

        public void EndGame(GameStatus status)
        {
            if (Status != GameStatus.OnGoing) return;

            Status = status;
            GameDuration = DateTime.Now - StartTime;
        }

        public Move PlayerMove(int column)
        {
            ValidateCanMakeMove();

            if (Board.IsMatrixFull())
            {
                EndGame(GameStatus.Draw);
                throw new InvalidOperationException("The game Board is full");
            }

            return MakeMove(column, Player.Human);
        }

        public void AiMove()
        {
            ValidateCanMakeMove();

            if (Board.IsMatrixFull())
            {
                EndGame(GameStatus.Draw);
                throw new InvalidOperationException("The game Board is full");
            }

            int? move = FindWinningMove() ?? FindBlockingMove();

            if (!move.HasValue)
            {
                move = MakeRandomMove();
            }

            MakeMove(move.Value, Player.Ai);
        }

        private Move MakeMove(int column, Player player)
        {
            if (!Board.IsValidMove(column))
                throw new InvalidOperationException("The move isn't valid");

            Board.MakeMove(column, player);
            
            if (Board.IsWinningMove(column) != null)
            {
                EndGame(player == Player.Human ? GameStatus.Won : GameStatus.Lost);
            }

            var move = new Move(column, player);
            Moves.Add(move);
            ToggleCurrentPlayer();
            return move;
        }

        private void ToggleCurrentPlayer()
        {
            CurrentPlayer = CurrentPlayer == Player.Human ? Player.Ai : Player.Human;
        }

        private void ValidateCanMakeMove()
        {
            if (Status != GameStatus.OnGoing || (CurrentPlayer == Player.Human && Board.IsMatrixFull()))
            {
                throw new InvalidOperationException("Can't make move!");
            }
        }

        private int? FindWinningMove()
        {
            int winningMove = -1;
            for (int col = 0; col < Board.GetMatrix().GetLength(1); col++)
            {
                // Check if making a move in this column would result in a win
                Board.MakeMove(col, Player.Ai);
                if (Board.IsWinningMove(col) == Player.Ai)
                {
                    winningMove = col;
                }
                Board.UndoMove(col);
            }

            return winningMove == -1 ? null : (int?)winningMove;
        }

        private int? FindBlockingMove()
        {
            int blockMove = -1;
            for (int col = 0; col < Board.GetMatrix().GetLength(1); col++)
            {
                // Check if the opponent could win in the next turn by making a move in this column
                Board.MakeMove(col, Player.Ai);
                Board.MakeMove(col, Player.Human);  // Pretend the opponent also makes a move in the same column
                if (Board.IsWinningMove(col) == Player.Human)
                {
                    blockMove = col;
                }
                Board.UndoMove(col);
                Board.UndoMove(col);
            }
            return blockMove == -1 ? null : (int?)blockMove;
        }

        private int MakeRandomMove()
        {
            Random rnd = new Random();
            int columns = Board.GetMatrix().GetLength(1);
            int column;
            
            do
            {
                column = rnd.Next(columns);
            }
            while (!Board.IsValidMove(column));

            return column;
        }
    }
}
