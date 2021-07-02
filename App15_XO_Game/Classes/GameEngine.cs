using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace App15_XO_Game
{
    public class GameEngine
    {
        private int xCellsNum;
        private int yCellsNum;
        private int leftOffset;
        private int topOffset;

        private const int MAX_CELL_SIZE = 40;

        public GameEngine(int cX, int cY, int spX, int spY)
        {
            xCellsNum = cX;
            yCellsNum = cY;
            leftOffset = ((spX - (xCellsNum * MAX_CELL_SIZE)) / 2);
            topOffset = ((spY - (yCellsNum * MAX_CELL_SIZE)) / 2);
        }

        public void Calculate(System.Windows.Controls.Canvas mainArea)
        {
            int calcPos(int offset, int index) => (offset + index * MAX_CELL_SIZE);

            // Create Lines 
            Line[] horizontalLines = new Line[xCellsNum + 1];
            for (int index = 0; index < horizontalLines.Length; index++)
            {
                horizontalLines[index] = new Line();
                Canvas.SetLeft(horizontalLines[index], leftOffset);
                Canvas.SetTop(horizontalLines[index], calcPos(topOffset, index));
                horizontalLines[index].Style = (Style)Application.Current.Resources["Cell_Line"];
                horizontalLines[index].X1 = (xCellsNum * MAX_CELL_SIZE);
                mainArea.Children.Add(horizontalLines[index]);
            }

            Line[] verticalLines = new Line[yCellsNum + 1];
            for (int index = 0; index < verticalLines.Length; index++)
            {
                verticalLines[index] = new Line();
                RotateTransform rotation = new RotateTransform();
                rotation.Angle = 90;
                verticalLines[index].RenderTransform = rotation;
                Canvas.SetLeft(verticalLines[index], calcPos(leftOffset, index));
                Canvas.SetTop(verticalLines[index], topOffset);
                verticalLines[index].Style = (Style)Application.Current.Resources["Cell_Line"];
                verticalLines[index].X1 = (xCellsNum * MAX_CELL_SIZE);
                mainArea.Children.Add(verticalLines[index]);
            }

            // Create buttons 
            Button[,] buttons = new Button[yCellsNum, xCellsNum];
            for (int indexY = 0; indexY < yCellsNum; indexY++)
                for (int indexX = 0; indexX < xCellsNum; indexX++)
                {
                    buttons[indexY, indexX] = new Button();
                    buttons[indexY, indexX].Width = (MAX_CELL_SIZE - 2);
                    buttons[indexY, indexX].Height = (MAX_CELL_SIZE - 2);
                    buttons[indexY, indexX].Style = (Style)Application.Current.Resources["Cell_Button"];
                    Canvas.SetLeft(buttons[indexY, indexX], calcPos((leftOffset + 1), indexX));
                    Canvas.SetTop(buttons[indexY, indexX], calcPos((topOffset + 1), indexY));
                    mainArea.Children.Add(buttons[indexY, indexX]);
                }
        }
    }
}