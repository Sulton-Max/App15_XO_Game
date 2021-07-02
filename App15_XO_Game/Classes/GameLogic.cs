namespace App15_XO_Game
{

    public class GameLogic
    {
        private GameEngine gameEngine;
        private Player player1;
        private Player player2;

        public GameLogic(GameEngine gameEngine, Player player1, Player player2)
        {
            this.gameEngine = gameEngine;
            this.player1 = player1;
            this.player2 = player2;
        }

        public void Run()
        {
            gameEngine.Render();
        }

        public void GameProcess()
        {

        }
    }
}