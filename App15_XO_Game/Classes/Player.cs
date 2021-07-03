namespace App15_XO_Game
{

    public class Player
    {
        public char OwnSign { get; set; }
        public string Username { get; set; }
        public int Score { get; private set; }
        public Player(string uname, GameSigns sign, bool isPc)
        {
            Username = uname;
            OwnSign = (char)sign;
            IsPc = isPc;
        }

        public bool IsPc { get; private set; }

        public void IncrementScore() => Score++;
    }
}