namespace App15_XO_Game
{
    public static class PcPlayerLogic
    {
        public static PCorr MakeMove(char[,] gameTable, char pcSign, PCorr previousMove)
        {
            for (int indexY = 0; indexY < gameTable.GetLength(0); indexY++)
                for (int indexX = 0; indexX < gameTable.GetLength(1); indexX++)
                    if (gameTable[indexY, indexX] == '\0')
                        return new PCorr(indexX, indexY);
            return new PCorr(1, 1);
        }
    }
}