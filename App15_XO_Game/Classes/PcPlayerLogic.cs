using System;
using System.Collections.Generic;

namespace App15_XO_Game
{
    public static class PcPlayerLogic
    {
        private static char[,] _gameTable;
        private static char _pcSign;

        public static PCorr MakeMove(char[,] gameTable, char pcSign, PCorr previousMove, bool isPCShouldMoveFirst)
        {
            int xLen = gameTable.GetLength(1);
            int yLen = gameTable.GetLength(0);
            // If PC should move first, then no need to calculate anything
            if (isPCShouldMoveFirst)
                if (xLen == 3 && yLen == 3)
                    return new PCorr(1, 1);
                else
                {
                    Random random = new Random();
                    return new PCorr(random.Next(0, xLen), random.Next(0, yLen));
                }

            _pcSign = pcSign;
            _gameTable = gameTable;
            int xCorr = previousMove.X;
            int yCorr = previousMove.Y;

            List<PCorr> dangerousMoves = new List<PCorr>();
            List<PCorr> importantMoves = new List<PCorr>();

            // TODO : Make the logic of pc move

            // Enumerate the dangerous moves of Player1
            dangerousMoves = EnumerateDIMoves(xCorr, yCorr, false);

            // Enumerate the important moves of the PC
            for (int indexY = 0; indexY < yLen; indexY++)
                for (int indexX = 0; indexX < xLen; indexX++)
                    if (_gameTable[indexY, indexX].Equals(_pcSign))
                    {
                        List<PCorr> temp = EnumerateDIMoves(indexX, indexY, true);
                        if (temp.Count > 0)
                            AddUnion(temp, importantMoves);
                    }

            // If there is any important movevs, make a move
            if (importantMoves.Count > 0)
                return importantMoves[0];

            // If there is any dangerous moves, then prevent them
            if (dangerousMoves.Count > 0)
                return dangerousMoves[0];

            // Make clever move if there is no dangerous or important moves
            PCorr pCorr = MakeCleverMove(xCorr, yCorr);
            return pCorr;
        }

        public static bool IsRightSign(int x, int y, bool IsChecktoPcSign)
        {
            char checkSign = (_pcSign.Equals((char)GameSigns.OSign))
                ? ((IsChecktoPcSign) ? _pcSign : (char)GameSigns.XSign)
                : ((IsChecktoPcSign) ? _pcSign : (char)GameSigns.OSign);
            bool result = _gameTable[y, x].Equals(checkSign);
            return result;
        }

        public static bool IsDIMove(int x1, int y1, int x2, int y2, int x3, int y3, bool IsImportant, out PCorr dIMove)
        {
            bool hasDIMove = false;
            dIMove = new PCorr(x1, y1);
            // Define if there is empty cell in this line
            if (_gameTable[y1, x1].Equals('\0'))
            {
                if (IsRightSign(x2, y2, IsImportant) && IsRightSign(x3, y3, IsImportant))
                {
                    hasDIMove = true;
                    dIMove = new PCorr(x1, y1);
                }
            }
            else if (_gameTable[y2, x2].Equals('\0'))
            {
                if (IsRightSign(x1, y1, IsImportant) && IsRightSign(x3, y3, IsImportant))
                {
                    hasDIMove = true;
                    dIMove = new PCorr(x2, y2);
                }
            }
            else if (_gameTable[y3, x3].Equals('\0'))
            {
                if (IsRightSign(x1, y1, IsImportant) && IsRightSign(x2, y2, IsImportant))
                {
                    hasDIMove = true;
                    dIMove = new PCorr(x3, y3);
                }
            }

            // If there is no empty cell, or two empty cells it is not dangerous or important move, so return false
            if (!hasDIMove)
                return false;
            else
                return true;
        }

        private static List<PCorr> EnumerateDIMoves(int xCorr, int yCorr, bool IsImportant)
        {
            int xLen = _gameTable.GetLength(1);
            int yLen = _gameTable.GetLength(0);
            List<PCorr> dIMoves = new List<PCorr>();

            // Enumerate by vertical direction
            if (yCorr - 1 >= 0)
            {
                if (yCorr - 2 >= 0)
                    if (IsDIMove(xCorr, yCorr, xCorr, (yCorr - 1), xCorr, (yCorr - 2), IsImportant, out PCorr dIMove))
                        dIMoves.Add(dIMove);

                if (yCorr + 1 < yLen)
                    if (IsDIMove(xCorr, (yCorr - 1), xCorr, yCorr, xCorr, (yCorr + 1), IsImportant, out PCorr dIMove))
                        dIMoves.Add(dIMove);
            }
            if ((yCorr + 1 < yLen) && (yCorr + 2 < yLen))
                if (IsDIMove(xCorr, yCorr, xCorr, (yCorr + 1), xCorr, (yCorr + 2), IsImportant, out PCorr dIMove))
                    dIMoves.Add(dIMove);

            // Enumerate all the horizontal crossings
            if (xCorr + 1 < xLen)
            {
                if (xCorr + 2 < xLen)
                    if (IsDIMove(xCorr, yCorr, (xCorr + 1), yCorr, (xCorr + 2), yCorr, IsImportant, out PCorr dIMove))
                        dIMoves.Add(dIMove);

                if (xCorr - 1 >= 0)
                    if (IsDIMove((xCorr - 1), yCorr, xCorr, yCorr, (xCorr + 1), yCorr, IsImportant, out PCorr dIMove))
                        dIMoves.Add(dIMove);
            }
            if ((xCorr - 1 >= 0) && (xCorr - 2 >= 0))
                if (IsDIMove((xCorr - 2), yCorr, (xCorr - 1), yCorr, xCorr, yCorr, IsImportant, out PCorr dIMove))
                    dIMoves.Add(dIMove);

            // Enumerate by left diagonal direction
            if ((yCorr - 1 >= 0) && (xCorr + 1 < xLen))
            {
                if ((yCorr - 2 >= 0) && (xCorr + 2 < xLen))
                    if (IsDIMove(xCorr, yCorr, (xCorr + 1), (yCorr - 1), (xCorr + 2), (yCorr - 2), IsImportant, out PCorr dIMove))
                        dIMoves.Add(dIMove);

                if ((yCorr + 1 < yLen) && (xCorr - 1 >= 0))
                    if (IsDIMove((xCorr + 1), (yCorr - 1), xCorr, yCorr, (xCorr - 1), (yCorr + 1), IsImportant, out PCorr dIMove))
                        dIMoves.Add(dIMove);
            }
            if ((yCorr + 1 < yLen) && (xCorr - 1 >= 0) && (yCorr + 2 < yLen) && (xCorr - 2 >= 0))
                if (IsDIMove(xCorr, yCorr, (xCorr - 1), (yCorr + 1), (xCorr - 2), (yCorr + 2), IsImportant, out PCorr dIMove))
                    dIMoves.Add(dIMove);

            // Enumerate by right diagonal direction
            if ((yCorr + 1 < yLen) && (xCorr + 1 < xLen))
            {
                if ((yCorr + 2 < yLen) && (xCorr + 2 < xLen))
                    if (IsDIMove(xCorr, yCorr, (xCorr + 1), (yCorr + 1), (xCorr + 2), (yCorr + 2), IsImportant, out PCorr dIMove))
                        dIMoves.Add(dIMove);

                if ((yCorr - 1 >= 0) && (xCorr - 1 >= 0))
                    if (IsDIMove((xCorr - 1), (yCorr - 1), xCorr, yCorr, (xCorr + 1), (yCorr + 1), IsImportant, out PCorr dIMove))
                        dIMoves.Add(dIMove);
            }
            if ((yCorr - 2 >= 0) && (xCorr - 2 >= 0) && (yCorr - 1 >= 0) && (xCorr - 1 >= 0))
                if (IsDIMove((xCorr - 2), (yCorr - 2), (xCorr - 1), (yCorr - 1), xCorr, yCorr, IsImportant, out PCorr dIMove))
                    dIMoves.Add(dIMove);

            return dIMoves;
        }

        private static bool IsEmptyCell(int xCorr, int yCorr) => _gameTable[yCorr, xCorr].Equals('\0');

        private static PCorr GetEmptyCellCorr(int xCorr, int yCorr, bool isToPushNew = false)
        {
            int xLen = _gameTable.GetLength(1);
            int yLen = _gameTable.GetLength(0);
            Random random = new Random();

            // Checking by vertical direction
            if (yCorr - 1 >= 0)
            {
                if (yCorr - 2 >= 0 && IsEmptyCell(xCorr, (yCorr - 1)) && IsEmptyCell(xCorr, (yCorr - 2)))
                    return (random.Next(0, 2) == 0)
                        ? new PCorr(xCorr, yCorr - 1)
                        : new PCorr(xCorr, yCorr - 2);

                if (yCorr + 1 < yLen && IsEmptyCell(xCorr, (yCorr - 1)) && IsEmptyCell(xCorr, (yCorr + 1)))
                    return (random.Next(0, 2) == 0)
                        ? new PCorr(xCorr, yCorr - 1)
                        : new PCorr(xCorr, yCorr + 1);
            }
            if ((yCorr + 1 < yLen) && (yCorr + 2 < yLen) && IsEmptyCell(xCorr, (yCorr + 1)) && IsEmptyCell(xCorr, (yCorr + 2)))
                return (random.Next(0, 2) == 0)
                        ? new PCorr(xCorr, yCorr + 1)
                        : new PCorr(xCorr, yCorr + 2);

            //  Checking by horizontal direction
            if (xCorr + 1 < xLen)
            {
                if (xCorr + 2 < xLen && IsEmptyCell((xCorr + 1), yCorr) && IsEmptyCell((xCorr + 2), yCorr))
                    return (random.Next(0, 2) == 0)
                        ? new PCorr(xCorr + 1, yCorr)
                        : new PCorr(xCorr + 2, yCorr);

                if (xCorr - 1 >= 0 && IsEmptyCell((xCorr - 1), yCorr) && IsEmptyCell((xCorr + 1), yCorr))
                    return (random.Next(0, 2) == 0)
                       ? new PCorr(xCorr + 1, yCorr)
                       : new PCorr(xCorr - 1, yCorr);
            }
            if ((xCorr - 1 >= 0) && (xCorr - 2 >= 0) && IsEmptyCell((xCorr - 1), yCorr) && IsEmptyCell((xCorr - 2), yCorr))
                return (random.Next(0, 2) == 0)
                      ? new PCorr(xCorr - 1, yCorr)
                      : new PCorr(xCorr - 2, yCorr);

            // Checking by left diagonal direction
            if ((yCorr - 1 >= 0) && (xCorr + 1 < xLen))
            {
                if ((yCorr - 2 >= 0) && (xCorr + 2 < xLen) && IsEmptyCell((xCorr + 1), (yCorr - 1)) && IsEmptyCell((xCorr + 2), (yCorr - 2)))
                    return (random.Next(0, 2) == 0)
                      ? new PCorr(xCorr + 1, yCorr - 1)
                      : new PCorr(xCorr + 2, yCorr - 2);

                if ((yCorr + 1 < yLen) && (xCorr - 1 >= 0) && IsEmptyCell((xCorr + 1), (yCorr - 1)) && IsEmptyCell((xCorr - 1), (yCorr + 1)))
                    return (random.Next(0, 2) == 0)
                       ? new PCorr(xCorr + 1, yCorr - 1)
                       : new PCorr(xCorr -1, yCorr + 1);
            }
            if ((yCorr + 1 < yLen) && (xCorr - 1 >= 0) && (yCorr + 2 < yLen) && (xCorr - 2 >= 0) && IsEmptyCell((xCorr - 1), (yCorr + 1)) && IsEmptyCell((xCorr - 2), (yCorr + 2)))
                return (random.Next(0, 2) == 0)
                      ? new PCorr(xCorr - 1, yCorr + 1)
                      : new PCorr(xCorr - 2, yCorr + 2);

            // Checking by right diagonal direction
            if ((yCorr + 1 < yLen) && (xCorr + 1 < xLen))
            {
                if ((yCorr + 2 < yLen) && (xCorr + 2 < xLen) && IsEmptyCell((xCorr + 1), (yCorr + 1)) && IsEmptyCell((xCorr + 2), (yCorr + 2)))
                    return (random.Next(0, 2) == 0)
                      ? new PCorr(xCorr + 1, yCorr + 1)
                      : new PCorr(xCorr + 2, yCorr + 2);

                if ((yCorr - 1 >= 0) && (xCorr - 1 >= 0) && IsEmptyCell((xCorr + 1), (yCorr + 1)) && IsEmptyCell((xCorr - 1), (yCorr - 1)))
                    return (random.Next(0, 2) == 0)
                       ? new PCorr(xCorr - 1, yCorr - 1)
                       : new PCorr(xCorr + 1, yCorr + 1);
            }
            if ((yCorr - 2 >= 0) && (xCorr - 2 >= 0) && (yCorr - 1 >= 0) && (xCorr - 1 >= 0) && IsEmptyCell((xCorr - 1), (yCorr - 1)) && IsEmptyCell((xCorr - 2), (yCorr - 2)))
                return (random.Next(0, 2) == 0)
                      ? new PCorr(xCorr - 1, yCorr - 1)
                      : new PCorr(xCorr - 2, yCorr - 2);

            return new PCorr(-1, -1);  // If not return fake coordinates
        }

        private static PCorr MakeCleverMove(int xCorr, int yCorr)
        {
            int xLen = _gameTable.GetLength(1);
            int yLen = _gameTable.GetLength(0);
            List<PCorr> dIMoves = new List<PCorr>();

            bool simpleGame = ((xLen == 3) && (yLen == 3));

            // If this is simple 3 x 3 board game
            if (simpleGame)
            {
                // If previous move was in the center
                //-if (xCorr == 1 && yCorr == 1)
                {
                    PCorr[] corners = new PCorr[4] { new PCorr(0, 0), new PCorr(xLen-1,0), new PCorr(0, yLen-1), new PCorr(xLen-1, yLen-1) };
                    foreach (PCorr pCorr in corners)
                        if (_gameTable[pCorr.Y, pCorr.X].Equals('\0'))
                            return pCorr;
                }

                // If the previous move was in the corner
                if ((xCorr == 0 || xCorr == xLen) && (yCorr == 0 || yCorr == (yLen - 1)) && _gameTable[1,1].Equals('\0'))
                    return new PCorr(1, 1);

                // Push the cell with nearest PC's sign
                for (int indexY = 0; indexY < yLen; indexY++)
                    for (int indexX = 0; indexX < xLen; indexX++)
                        if (_gameTable[indexY, indexX].Equals(_pcSign))
                        {
                            PCorr pCorr = GetEmptyCellCorr(indexX, indexY);
                            if (pCorr.X != -1)
                                return pCorr;
                        }
            }
            else // If this is not simple board game
            {
                // Push the cell with nearest PC's sign
                for (int indexY = 0; indexY < yLen; indexY++)
                    for (int indexX = 0; indexX < xLen; indexX++)
                        if (_gameTable[indexY, indexX].Equals(_pcSign))
                        {
                            PCorr pCorr = GetEmptyCellCorr(indexX, indexY);
                            if (pCorr.X != -1)
                                return pCorr;
                        }

                // If there is no such cells, then push the nearest empty cell
                for (int indexY = 0; indexY < yLen; indexY++)
                    for (int indexX = 0; indexX < xLen; indexX++)
                        if (_gameTable[indexY, indexX].Equals('\0'))
                        {
                            PCorr pCorr = GetEmptyCellCorr(indexX, indexY, true);
                            if (pCorr.X != -1)
                                return pCorr;
                        }
            }

            // If there is no clever movees to make, just make move to random empty cell
            for (int indexY = 0; indexY < yLen; indexY++)
                for (int indexX = 0; indexX < xLen; indexX++)
                    if (_gameTable[indexY, indexX].Equals('\0'))
                        return new PCorr(indexX, indexY);

            return new PCorr(1, 1);
        }

        private static bool IsSamePoint(PCorr p1, PCorr p2) => ((p1.X == p2.X) && (p1.Y == p2.Y));

        private static void AddUnion(List<PCorr> source, List<PCorr> destination)
        {
            if (source.Count > 0 && destination.Count == 0)
                destination.Add(source[0]);

            bool doesContain;
            foreach (PCorr pCorr in source)
            {
                doesContain = false;
                if (destination.Count > 0)
                {
                    foreach (PCorr pCorr1 in destination)
                        if (IsSamePoint(pCorr, pCorr1))
                            doesContain = true;
                    if (!doesContain)
                        destination.Add(pCorr);
                }
            }
        }
    }
}