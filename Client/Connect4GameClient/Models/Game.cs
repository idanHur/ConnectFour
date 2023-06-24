namespace GameLogic.Models
{
    public enum Player
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
        public bool[,] board;
        public GameStatus gameStatus { get; private set; }
        public Player currentPlayer { get; private set; }
        public int gameId { get; private set; }

        public ICollection<Move>? Moves { get; set; }
        public DateTime startTime { get; private set; }
        public TimeSpan gameDuration { get; private set; }
        public Game(int rows, int columns, int gameId)
        {
            this.gameId = gameId;
            board = new bool[rows, columns];
            gameStatus = GameStatus.OnGoing;
            startTime = DateTime.Now;
        }
        
        public void MakeMove(int columns, bool isAi = false)
        {

        }



    }
}