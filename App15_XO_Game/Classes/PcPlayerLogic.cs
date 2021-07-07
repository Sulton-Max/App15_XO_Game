using System;
using System.Collections.Generic;

namespace App15_XO_Game
{
    public static class PcPlayerLogic
    {
        private static char[,] _gameTable;
        private static char _pcSign;
        private static bool _isPCsFirstMove = true;

        public static PCorr MakeMove(char[,] gameTable, char pcSign, PCorr previousMove)
        {
            _pcSign = pcSign;
            _gameTable = gameTable;
            int xCorr = previousMove.X;
            int yCorr = previousMove.Y;
            int xLen = gameTable.GetLength(1);
            int yLen = gameTable.GetLength(0);

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
                            AddUnion(importantMoves, temp);
                    }

            // If there is any important movevs, make a move
            if (importantMoves.Count > 0)
                return importantMoves[0];

            // If there is any dangerous moves, then prevent them
            if (dangerousMoves.Count > 0)
                return dangerousMoves[0];

            // Check 1) Is PC hasn't moved yet 2) PC should move first
            bool isPCShouldFirstMove = true;
            if (_isPCsFirstMove)
                for (int indexY = 0; indexY < yLen; indexY++)
                    for (int indexX = 0; indexX < xLen; indexX++)
                    {
                        if (_gameTable[indexY, indexX].Equals(_pcSign))
                            _isPCsFirstMove = false;
                        if (_gameTable[indexY, indexX].Equals() 


                    }


            int x = 0;
            int y = 0;
            Random random = new Random();
            while (true)
            {
                x = random.Next(0, xLen);
                y = random.Next(0, yLen);
                if (_gameTable[y, x].Equals('\0'))
                    return new PCorr(x, y);
            }

            //return new PCorr(1, 1);
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

            // Checking by vertical direction
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

            // Checking all the horizontal crossings
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

            // Checking all the left diagonal crossings
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

            // Checking right diagonal crossings
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

        private static void AddUnion(List<PCorr> source, List<PCorr> destionation)
        {

        }
    }
}