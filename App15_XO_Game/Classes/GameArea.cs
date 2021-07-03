namespace App15_XO_Game
{
    public class GameArea
    {
        private const int MAX_CELL_SIZE = 40;

        public int XLength { get; private set; }
        public int YLength { get; private set; }
        public int XCells { get; private set; }
        public int YCells { get; private set; }

        public GameArea(int xCells, int yCells)
        {
            XCells = xCells;
            YCells = yCells;
            XLength = (XCells * MAX_CELL_SIZE);
            YLength = (YCells * MAX_CELL_SIZE);
        }
    }

}