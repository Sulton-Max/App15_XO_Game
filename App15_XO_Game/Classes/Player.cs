namespace App15_XO_Game
{
    public class Player
    {
        public char OwnSign { get; set; }
        public string Username { get; set; }
        public int Score { get; private set; }
        public Player(string uname, GameSigns sign)
        {
            Username = uname;
            OwnSign = (char)sign;
        }

        public void IncrementScore() => Score++;
    }
}