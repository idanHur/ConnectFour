namespace GameLogic.Models
{
    public enum PlayerType
    {
        Human,
        Ai
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
        public int[,] board;
        public GameStatus gameStatus { get; private set; }
        public int gameId { get; private set; }

        public ICollection<Move> moves { get; set; }

        public Game(int rows, int columns, int gameId)
        {
            this.gameId = gameId;
            board = new int[rows, columns];
            gameStatus = GameStatus.OnGoing;
            moves = new List<Move>();
        }

        public void UpdateGameState(int[,] board, GameStatus gameStatus, int lastMoveCol, PlayerType whichPlayer)
        {
            this.board = (int[,])board.Clone();
            this.gameStatus = gameStatus;
            Move newMove = new Move(lastMoveCol, whichPlayer, moves.Count + 1);
        }

        public bool IsEligibleMove(int column, PlayerType player)
        {
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