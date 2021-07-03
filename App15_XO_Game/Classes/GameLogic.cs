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
        private PCorr _previousMove;

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
                string[] values = button.Tag.ToString().Split(' ');
                int xCorr = Convert.ToInt32(values[0]);
                int yCorr = Convert.ToInt32(values[1]);

                MakeMove(new PCorr(xCorr, yCorr));
                GameProcess(xCorr, yCorr);
            }
        }

        private void MakeMove(PCorr pCorr)
        {
            char sign = _presentPlayer.OwnSign;
            Image image = new Image();
            image.Source = new BitmapImage(new Uri($"pack://application:,,,/App15_XO_Game;component/Icons/{sign}.png"));
            image.Height = 25;
            image.Width = 25;

            _gameEngine.SetImage(pCorr, image);
            GameTable[pCorr.Y, pCorr.X] = sign;

            _gameEngine.HightlightLastMove(new PCorr(pCorr.X, pCorr.Y));
            if (_player2.IsPc && !_presentPlayer.IsPc)
                _previousMove = new PCorr(pCorr.X, pCorr.Y);
        }

        private void ChangingTurn()
        {
            IsNowPlayer1 = !IsNowPlayer1;
            if (IsNowPlayer1)
                _presentPlayer = _player1;
            else
                _presentPlayer = _player2;
            _username.Text = _presentPlayer.Username;

            // Give turn to PC if another player is PC
            if (_presentPlayer.IsPc)
            {
                PCorr PCMove = PcPlayerLogic.MakeMove(GameTable, _presentPlayer.OwnSign, _previousMove);
                MakeMove(PCMove);
                GameProcess(PCMove.X, PCMove.Y);
            }
        }

        private bool CheckIfWon(int xCorr, int yCorr, ref string winner)
        {
            // If another player is PC, then write down the previous move
            if (_player2.IsPc && !_presentPlayer.IsPc)
                _previousMove = new PCorr(xCorr, yCorr);

            // Check in all 8 directions
            int yLen = GameTable.GetLength(0);
            int xLen = GameTable.GetLength(1);
            winner = _presentPlayer.Username;

            // TODO : Make all crossing checks

            // Checking the all verticals crossings
            if ((yCorr - 1) >= 0)
            {
                if ((yCorr - 2) >= 0)
                    if (IsRightSign(xCorr, yCorr) && IsRightSign(xCorr, yCorr - 1) && IsRightSign(xCorr, yCorr - 2))
                    {
                        _lineCorr = CreateLine(xCorr, yCorr - 2, xCorr, yCorr - 1, xCorr, yCorr);
                        return true;
                    }
                if ((yCorr + 1) < yLen)
                    if (IsRightSign(xCorr, yCorr - 1) && IsRightSign(xCorr, yCorr) && IsRightSign(xCorr, yCorr + 1))
                    {
                        _lineCorr = CreateLine(xCorr, yCorr - 1, xCorr, yCorr, xCorr, yCorr + 1);
                        return true;
                    }
            }
            if ((yCorr + 1 < yLen) && (yCorr + 2 < yLen))
                if (IsRightSign(xCorr, yCorr) && IsRightSign(xCorr, yCorr + 1) && IsRightSign(xCorr, yCorr + 2))
                {
                    _lineCorr = CreateLine(xCorr, yCorr, xCorr, yCorr + 1, xCorr, yCorr + 2);
                    return true;
                }

            // Checking all the horizontal crossings
            if (xCorr + 1 < xLen)
            {
                if (xCorr + 2 < xLen)
                    if (IsRightSign(xCorr, yCorr) && IsRightSign(xCorr + 1, yCorr) && IsRightSign(xCorr + 2, yCorr))
                    {
                        _lineCorr = CreateLine(xCorr, yCorr, xCorr + 1, yCorr, xCorr + 2, yCorr);
                        return true;
                    }
                if (xCorr - 1 >= 0)
                    if (IsRightSign(xCorr - 1, yCorr) && IsRightSign(xCorr, yCorr) && IsRightSign(xCorr + 1, yCorr))
                    {
                        _lineCorr = CreateLine(xCorr - 1, yCorr, xCorr, yCorr, xCorr + 1, yCorr);
                        return true;
                    }
            }
            if ((xCorr - 1 >= 0) && (xCorr - 2 >= 0))
                if (IsRightSign(xCorr, yCorr) && IsRightSign(xCorr - 1, yCorr) && IsRightSign(xCorr - 2, yCorr))
                {
                    _lineCorr = CreateLine(xCorr - 2, yCorr, xCorr - 1, yCorr, xCorr, yCorr);
                    return true;
                }


            // Checking all the left diagonal crossings
            if ((yCorr - 1 >= 0) && (xCorr + 1 < xLen))
            {
                if ((yCorr - 2 >= 0) && (xCorr + 2 < xLen))
                {
                    if (IsRightSign(xCorr, yCorr) && IsRightSign(xCorr + 1, yCorr - 1) && IsRightSign(xCorr + 2, yCorr - 2))
                    {
                        _lineCorr = CreateLine(xCorr, yCorr, xCorr + 1, yCorr - 1, xCorr + 2, yCorr - 2);
                        return true;
                    }
                }
                if ((yCorr + 1 < yLen) && (xCorr - 1 >= 0))
                    if (IsRightSign(xCorr + 1, yCorr - 1) && IsRightSign(xCorr, yCorr) && IsRightSign(xCorr - 1, yCorr + 1))
                    {
                        _lineCorr = CreateLine(xCorr - 1, yCorr + 1, xCorr, yCorr, xCorr + 1, yCorr - 1);
                        return true;
                    }
            }
            if ((yCorr + 1 < yLen) && (xCorr - 1 >= 0) && (yCorr + 2 < yLen) && (xCorr - 2 >= 0))
                if (IsRightSign(xCorr, yCorr) && IsRightSign(xCorr - 1, yCorr + 1) && IsRightSign(xCorr - 2, yCorr + 2))
                {
                    _lineCorr = CreateLine(xCorr, yCorr, xCorr - 1, yCorr + 1, xCorr - 2, yCorr + 2);
                    return true;
                }

            // Checking right diagonal crossings
            if ((yCorr + 1 < yLen) && (xCorr + 1 < xLen))
            {
                if ((yCorr + 2 < yLen) && (xCorr + 2 < xLen))
                {
                    if (IsRightSign(xCorr, yCorr) && IsRightSign(xCorr + 1, yCorr + 1) && IsRightSign(xCorr + 2, yCorr + 2))
                    {
                        _lineCorr = CreateLine(xCorr, yCorr, xCorr + 1, yCorr + 1, xCorr + 2, yCorr + 2);
                        return true;
                    }
                }
                if ((yCorr - 1 >= 0) && (xCorr - 1 >= 0))
                    if (IsRightSign(xCorr - 1, yCorr - 1) && IsRightSign(xCorr, yCorr) && IsRightSign(xCorr + 1, yCorr + 1))
                    {
                        _lineCorr = CreateLine(xCorr - 1, yCorr - 1, xCorr, yCorr, xCorr + 1, yCorr + 1);
                        return true;
                    }
            }
            if ((yCorr - 2 >= 0) && (xCorr - 2 >= 0) && (yCorr - 1 >= 0) && (xCorr - 1 >= 0))
                if (IsRightSign(xCorr - 2, yCorr - 2) && IsRightSign(xCorr - 1, yCorr - 1) && IsRightSign(xCorr, yCorr))
                {
                    _lineCorr = CreateLine(xCorr - 2, yCorr - 2, xCorr - 1, yCorr - 1, xCorr, yCorr);
                    return true;
                }

            // Check if all cells are full
            bool isTableFull = true;
            for (int indexY = 0; indexY < _gameEngine.GameArea.YCells; indexY++)
                for (int indexX = 0; indexX < _gameEngine.GameArea.YCells; indexX++)
                    if (GameTable[indexY, indexX] == '\0')
                        isTableFull = false;

            // Table is full so we should finish the game
            if (isTableFull)
            {
                winner = null;
                return true;
            }

            winner = null;
            return false;
        }

        private LCorr CreateLine(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            return new LCorr(new PCorr(x1, y1), new PCorr(x2, y2), new PCorr(x3, y3));
        }

        private bool IsRightSign(int xCorr, int yCorr)
        {
            var result = (GameTable[yCorr, xCorr] == _presentPlayer.OwnSign);
            return result;
        }

        private void Finish(string winnername = null)
        {
            bool IsDraw = (winnername == null) ? true : false;
            if (!IsDraw)
            {
                _status.Text = "Winner : ";
                _username.Text = _presentPlayer.Username;
                _presentPlayer.IncrementScore();
            }
            else
            {
                _status.Text = string.Empty;
                _username.Text = "Draw";
            }

            _gameEngine.Finish(IsDraw, _lineCorr, _player1, _player2, ContinueBtnClick);
        }

        public void ContinueBtnClick(object sender, RoutedEventArgs e)
        {
            Run();
        }
    }
}