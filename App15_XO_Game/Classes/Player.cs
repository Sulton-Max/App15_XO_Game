namespace App15_XO_Game
{
    public class Player
    {
        public char OwnSign { get; set; }

        public string Username { get; set; }

        public Player(GameSigns sign)
        {
            OwnSign = (char)sign;
        }
    }
}