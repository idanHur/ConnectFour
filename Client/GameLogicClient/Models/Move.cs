namespace GameLogic.Models
{
    public class Move
    {
        public int columnNumber { get; private set; }
        public PlayerType Player { get; private set; }
        public int id { get; private set; }

        public Move(int columnNumber, PlayerType player, int id)
        {
            this.columnNumber = columnNumber;
            this.Player = player;
            this.id = id;
        }

    }
}
