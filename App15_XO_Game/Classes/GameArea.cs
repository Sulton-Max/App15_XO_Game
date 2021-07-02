namespace App15_XO_Game
{
    public class GameArea
    {
        private const int MAX_CELL_SIZE = 40;

        public int XLength { get; set; }
        public int YLength { get; set; }

        public int XCells { get; set; }
        public int YCells { get; set; }

        public GameArea(int xCells, int yCells)
        {
            XCells = xCells;
            YCells = yCells;
            XLength = (XCells * MAX_CELL_SIZE);
            YLength = (YCells * MAX_CELL_SIZE);
        }
    }

}