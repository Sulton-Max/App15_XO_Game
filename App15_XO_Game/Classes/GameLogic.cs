using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace App15_XO_Game
{
    public enum GameStatus
    {
        Player1,
        Draw,
        Player2
    }

    public struct PCorr
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public PCorr(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public struct LCorr
    {
        public PCorr P1 { get; private set; }
        public PCorr P2 { get; private set; }
        public PCorr P3 { get; private set; }

        public LCorr(PCorr p1, PCorr p2, PCorr p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }
    }


    public class GameLogic
    {
        private GameEngine _gameEngine;
        private Player _player1;
        private Player _player2;
        private TextBlock _status;
        private TextBlock _username;
        private bool IsNowPlayer1 = true;
        private Player _presentPlayer;
        private RoutedEventHandler _showBack;
        private LCorr _lineCorr;

        public char[,] GameTable { get; private set; }

        public GameLogic(GameEngine gameEngine, Player player1, Player player2, TextBlock status, TextBlock username, RoutedEventHandler showBack)
        {
            // Save Game Engines, Players, Exit Handlers
            _gameEngine = gameEngine;
            _player1 = player1;
            _player2 = player2;
            _status = status;
            _username = username;
            _showBack = showBack;

            // Setting button's click handler
            _gameEngine.HandlerBinder = ButtonClickHandler;

            // Setting the present player
            _presentPlayer = player1;
        }

        public void Run()
        {
            // Setting the table
            GameTable = new char[_gameEngine.GameArea.YCells, _gameEngine.GameArea.XCells];

            _gameEngine.ClearData();
            _gameEngine.Render();
            _status.Text = "Now playing : ";
            _username.Text = _presentPlayer.Username;
        }

        public void GameProcess(int xCorr, int yCorr)
        {
            // Check if the player won
            string winner = null;
            if (CheckIfWon(xCorr, yCorr, ref winner))
                Finish(winner);
            else
                ChangingTurn(); // Give turn to the other player
        }

        private void ButtonClickHandler(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content == null)
            {
                // Filling with Image
                char sign = _presentPlayer.OwnSign;
                Image image = new Image();
                image.Source = new BitmapImage(new Uri($"pack://application:,,,/App15_XO_Game;component/Icons/{sign}.png"));
                image.Height = 25;
                image.Width = 25;
                button.Content = image;
                button.IsEnabled = false;

                // Further processing of the game
                string[] values = button.Tag.ToString().Split(' ');
                int xCorr = Convert.ToInt32(values[0]);
                int yCorr = Convert.ToInt32(values[1]);

                GameTable[yCorr, xCorr] = sign;
                GameProcess(xCorr, yCorr);
            }
        }

        private void ChangingTurn()
        {
            IsNowPlayer1 = !IsNowPlayer1;
            if (IsNowPlayer1)
                _presentPlayer = _player1;
            else
                _presentPlayer = _player2;

            _username.Text = _presentPlayer.Username;
        }

        private bool CheckIfWon(int xCorr, int yCorr, ref string winner)
        {
            // Check in all 8 directions
            int yLen = GameTable.GetLength(0);
            int xLen = GameTable.GetLength(1);
            winner = _presentPlayer.Username;


            // Checking by vertical direction and center
            if ((yCorr - 1) >= 0)
            {
                if ((yCorr - 2) >= 0)
                {
                    if (IsRightSign(xCorr, yCorr) && IsRightSign(xCorr, yCorr - 1) && IsRightSign(xCorr, yCorr - 2))
                    {
                        PCorr p1 = new PCorr(xCorr, yCorr - 2);
                        PCorr p2 = new PCorr(xCorr, yCorr - 1);
                        PCorr p3 = new PCorr(xCorr, yCorr);
                        _lineCorr = new LCorr(p1, p2, p3);
                        return true;
                    }
                }
                else if ((yCorr + 1) <= yLen)
                    if (IsRightSign(xCorr, yCorr - 1) && IsRightSign(xCorr, yCorr) && IsRightSign(xCorr, yCorr + 1))
                        return true;
            }

            winner = null;
            return false;
        }

        private bool IsRightSign(int xCorr, int yCorr)
        {
            var result = (GameTable[yCorr, xCorr] == _presentPlayer.OwnSign);
            return result;
        }

        private void Finish(string winnername = null)
        {
            bool IsDraw = (winnername == null) ? true : false;
            if(!IsDraw)
                _presentPlayer.IncrementScore();

            _status.Text = string.Empty;
            _username.Text = string.Empty;
            _gameEngine.Finish(IsDraw, _lineCorr, _player1, _player2, ContinueBtnClick);
        }

        public void ContinueBtnClick(object sender, RoutedEventArgs e)
        {
            Run();
        }
    }
}