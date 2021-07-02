using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace App15_XO_Game
{
    public enum GameSigns
    {
        XSign = 'x', 
        OSign = 'o'
    }


    public class GameEngine
    {
        private int leftOffset;
        private int topOffset;
        private GameArea _gArea;
        private Canvas _mArea;

        public char[,] GameTable { get; private set;  }

        private const int MAX_CELL_SIZE = 40;

        public GameEngine(GameArea gArea, Canvas mArea)
        {
            _gArea = gArea;
            _mArea = mArea;

            leftOffset = ((Convert.ToInt32(_mArea.Width) - _gArea.XLength) / 2);
            topOffset = ((Convert.ToInt32(_mArea.Height) - _gArea.YLength) / 2);
        }

        public void Render()
        {
            // Create Elements
            CreateLines((_gArea.YCells + 1), false);        // horizontal lines
            CreateLines((_gArea.XCells + 1), true);         // vertical lines
            CreateButtons(_gArea.XCells, _gArea.YCells);    // buttons
           
        }

        private int CalcPos(int offset, int index) => (offset + index * MAX_CELL_SIZE);

        private void CreateLines(int number, bool vertical = false)
        {   
            // Create Lines
            Line[] lines = new Line[number];
            int offset = (vertical) ? topOffset : leftOffset;

            // Set each parameter for Lines
            for (int index = 0; index < number; index++)
            {
                lines[index] = new Line();
                lines[index].Style = (Style)Application.Current.Resources["Cell_Line"];
                if (vertical)
                {
                    lines[index].X1 = _gArea.YLength;
                    Canvas.SetLeft(lines[index], CalcPos(leftOffset, index));
                    Canvas.SetTop(lines[index], topOffset);
                    RotateTransform rotation = new RotateTransform();
                    rotation.Angle = 90;
                    lines[index].RenderTransform = rotation;
                }
                else
                {
                    lines[index].X1 = _gArea.XLength;
                    Canvas.SetLeft(lines[index], leftOffset);
                    Canvas.SetTop(lines[index], CalcPos(topOffset, index));
                }
                _mArea.Children.Add(lines[index]);
            }
        }

        private Button[,] CreateButtons(int xLen, int yLen)
        {
            // Create buttons 
            Button[,] buttons = new Button[yLen, xLen];
            for (int indexY = 0; indexY < yLen; indexY++)
                for (int indexX = 0; indexX < xLen; indexX++)
                {
                    buttons[indexY, indexX] = new Button();
                    buttons[indexY, indexX].Click += CellButtonClick;
                    buttons[indexY, indexX].Tag = $"{indexX} {indexY}";
                    buttons[indexY, indexX].Width = (MAX_CELL_SIZE - 2);
                    buttons[indexY, indexX].Height = (MAX_CELL_SIZE - 2);
                    buttons[indexY, indexX].Style = (Style)Application.Current.Resources["Cell_Button"];
                    Canvas.SetLeft(buttons[indexY, indexX], CalcPos((leftOffset + 1), indexX));
                    Canvas.SetTop(buttons[indexY, indexX], CalcPos((topOffset + 1), indexY));
                    _mArea.Children.Add(buttons[indexY, indexX]);
                }
            return buttons;
        }

        private void CellButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content == null)
            {
                // Filling with Image
                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://application:,,,/App15_XO_Game;component/Icons/x.png"));
                image.Height = 25;
                image.Width = 25;
                button.Content = image;
            }
        }
    }
}