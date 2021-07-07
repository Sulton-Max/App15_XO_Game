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
    public enum LineType
    {
        Vertical,
        Horizontal,
        LeftDiagonal,
        RightDiagonal
    }

    public class GameEngine
    {
        private int leftOffset;
        private int topOffset;
        private const int MAX_CELL_SIZE = 40;
        private Canvas _mArea;
        private Button[,] _buttons;

        private Player _player1;
        private Player _player2;
        private RoutedEventHandler _conHandler;
        public PCorr PreviousMove { get; private set; } 

        public RoutedEventHandler HandlerBinder { get; set; }
        public GameArea GameArea { get; private set; }

        public GameEngine(GameArea gArea, Canvas mArea)
        {
            GameArea = gArea;
            _mArea = mArea;

            leftOffset = ((Convert.ToInt32(_mArea.Width) - GameArea.XLength) / 2);
            topOffset = ((Convert.ToInt32(_mArea.Height) - GameArea.YLength) / 2);
        }

        public void Render()
        {
            // Clear the canvas
            _mArea.Children.Clear();
            // Create Elements
            CreateLines((GameArea.YCells + 1), false);        // horizontal lines
            CreateLines((GameArea.XCells + 1), true);         // vertical lines
            _buttons = CreateButtons(GameArea.XCells, GameArea.YCells);    // buttons
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
                    lines[index].X1 = GameArea.YLength;
                    Canvas.SetLeft(lines[index], CalcPos(leftOffset, index));
                    Canvas.SetTop(lines[index], topOffset);
                    RotateTransform rotation = new RotateTransform();
                    rotation.Angle = 90;
                    lines[index].RenderTransform = rotation;
                }
                else
                {
                    lines[index].X1 = GameArea.XLength;
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
                    buttons[indexY, indexX].Click += HandlerBinder;
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

        internal void SetImage(PCorr pCorr, Image image)
        {
            _buttons[pCorr.Y, pCorr.X].Content = image;
            _buttons[pCorr.Y, pCorr.X].IsEnabled = false;
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

        public void ShowScores()
        {
            // Creating textblocks
            TextBlock p1name = new TextBlock();
            TextBlock p1score = new TextBlock();
            TextBlock p2name = new TextBlock();
            TextBlock p2score = new TextBlock();
            SetTextBlock(p1name);
            SetTextBlock(p1score);
            SetTextBlock(p2name);
            SetTextBlock(p2score);
            p1name.Text = _player1.Username + " : ";
            p1score.Text = _player1.Score.ToString();
            p2name.Text = _player2.Username + " : ";
            p2score.Text = _player2.Score.ToString();

            Button button = new Button();
            button.Content = "Continue";
            button.Width = 100;
            button.Click += _conHandler;
            button.Padding = new Thickness(4);
            button.FontSize = 18;
            button.Margin = new Thickness(0, 10, 0, 0);

            StackPanel stack1 = new StackPanel();
            StackPanel stack2 = new StackPanel();
            stack1.Orientation = Orientation.Horizontal;
            stack2.Orientation = Orientation.Horizontal;
            stack1.Children.Add(p1name);
            stack1.Children.Add(p1score);
            stack2.Children.Add(p2name);
            stack2.Children.Add(p2score);

            StackPanel mainStack = new StackPanel();
            mainStack.Children.Add(stack1);
            mainStack.Children.Add(stack2);
            mainStack.Children.Add(button);

            // Fixing the position
            Canvas.SetLeft(mainStack, ((_mArea.Width - 200) / 2));
            Canvas.SetTop(mainStack, ((_mArea.Height - 50) / 2));

            // Adding to Canvas
            _mArea.Children.Clear();
            _mArea.Children.Add(mainStack);
        }

        public void Finish(bool isDraw, LCorr lCorr, Player p1, Player p2, RoutedEventHandler conH)
        {
            // Disable all buttons
            DisableAllBtns();

            // Show how the player won if it is now draw
            if (!isDraw)
                ShowCrossedSignsLine(lCorr);

            // Saving data temporarily
            _player1 = p1;
            _player2 = p2;
            _conHandler = conH;

            // Button for showing scores
            Button button = new Button();
            button.Content = "Next";
            button.Width = 100;
            button.Click += ShowScoresBtnClick;
            button.FontSize = 18;

            // Fixing the position
            Canvas.SetLeft(button, ((_mArea.Width - 100) / 2));
            Canvas.SetBottom(button, 20);

            _mArea.Children.Add(button);
        }

        private void SetTextBlock(TextBlock textBlock)
        {
            textBlock.Foreground = new SolidColorBrush(Colors.Gray);
            textBlock.FontSize = 18;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.Width = 150;
        }

        private void ShowScoresBtnClick(object sender, RoutedEventArgs e)
        {
            ShowScores();
        }

        public void ClearData()
        {
            _player1 = null;
            _player2 = null;
            _conHandler = null;
        }

        private void ShowCrossedSignsLine(LCorr lCorr)
        {
            // Check the type of the line
            LineType lineType = LineType.Horizontal;
            if (lCorr.P1.X == lCorr.P2.X)
                lineType = LineType.Vertical;
            else if (lCorr.P1.Y == lCorr.P2.Y)
                lineType = LineType.Horizontal;
            else if ((lCorr.P1.X < lCorr.P2.X) && (lCorr.P1.Y > lCorr.P2.Y))
                lineType = LineType.LeftDiagonal;
            else if ((lCorr.P1.X < lCorr.P2.X) && (lCorr.P1.Y < lCorr.P2.Y))
                lineType = LineType.RightDiagonal;

            // Create the line
            Line line = new Line();
            line.X1 = 120;
            line.StrokeThickness = 4;
            line.Stroke = new SolidColorBrush(Colors.Red);

            int leftSide = 0;
            int topSide = 0;

            if(lineType == LineType.Horizontal)
                topSide = (MAX_CELL_SIZE / 2);
            else if (lineType == LineType.Vertical)
            {
                RotateTransform rotation = new RotateTransform();
                rotation.Angle = 90;
                line.RenderTransform = rotation;
                leftSide = (MAX_CELL_SIZE / 2);
            }
            else if(lineType == LineType.LeftDiagonal)
            {
                RotateTransform rotation = new RotateTransform();
                rotation.Angle = -45;
                line.RenderTransform = rotation;
                topSide = MAX_CELL_SIZE / 2;
                leftSide = MAX_CELL_SIZE / 2;
            }
            else if(lineType == LineType.RightDiagonal)
            {
                RotateTransform rotation = new RotateTransform();
                rotation.Angle = 45;
                line.RenderTransform = rotation;
                topSide = MAX_CELL_SIZE / 2;
                leftSide = MAX_CELL_SIZE / 2;
            }

            // Fixing the position

            Canvas.SetLeft(line, CalcPos((leftOffset + 1), lCorr.P1.X) + leftSide);
            Canvas.SetTop(line, CalcPos((topOffset + 1), lCorr.P1.Y) + topSide);
            _mArea.Children.Add(line);
        }

        private void DisableAllBtns()
        {
            for (int indexY = 0; indexY < GameArea.YCells; indexY++)
                for (int indexX = 0; indexX < GameArea.XCells; indexX++)
                    _buttons[indexY, indexX].IsEnabled = false;
        }

        public void HightlightLastMove(PCorr currentMove)
        {
            _buttons[PreviousMove.Y, PreviousMove.X].BorderThickness = new Thickness(0);
            _buttons[currentMove.Y, currentMove.X].BorderThickness = new Thickness(3);
            PreviousMove = new PCorr(currentMove.X, currentMove.Y);
        }
    }
}