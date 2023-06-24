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
        public bool[,] board;
        public GameStatus gameStatus { get; private set; }
        public PlayerType currentPlayer { get; private set; }
        public int gameId { get; private set; }

        public ICollection<Move>? moves { get; set; }

        public Game(int rows, int columns, int gameId)
        {
            this.gameId = gameId;
            board = new bool[rows, columns];
            gameStatus = GameStatus.OnGoing;
            moves = new List<Move>();
        }
        
        public void MakeMove(int columns, bool isAi = false)
        {

        }



    }
}