namespace GameLogicClient.Models
{
    public class Move
    {
        public int columnNumber { get; private set; }
        public PlayerType Player { get; private set; }
        public int id { get; private set; }
        public int GameId { get; set; } // Add GameId property
        public Game Game { get; set; } // Add navigation property for Game

        public Move(int columnNumber, PlayerType player, int id)
        {
            this.columnNumber = columnNumber;
            this.Player = player;
            this.id = id;
        }

    }
}
