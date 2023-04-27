using System;
using Tiles;

namespace GameBoard
{
    public class BoardResizer
    {
        private Board _board;
        private TileFactory _factory;

        public BoardResizer(Board board, TileFactory factory)
        {
            _board = board;
            _factory = factory;
        }

        public Tile[,] ResizeBoard(int newSize, Tile[,] prevTiles)
        {
            int prevSize = _board.Size;

            if (newSize < prevSize)
                throw new ArgumentException("Sizing the board down is not implemented");

            Tile[,] newTiles = new Tile[newSize, newSize];

            int sizeDelta = newSize - prevSize;
            int topOrBottomAlignment = prevSize % 2;
            int topRightSizeDelta = (sizeDelta + topOrBottomAlignment) / 2;
            int bottomLeftSizeDelta = sizeDelta - topRightSizeDelta;

            CopyAndShiftTiles(prevTiles, prevSize, ref newTiles, bottomLeftSizeDelta);

            return newTiles;
        }

        private void CopyAndShiftTiles(Tile[,] prevTiles, int prevSize, ref Tile[,] newTiles, int bottomLeftSizeDelta)
        {
            for (int i = 0; i < prevSize; i++)
            {
                int iNew = i + bottomLeftSizeDelta;

                for (int j = 0; j < prevSize; j++)
                {
                    int jNew = j + bottomLeftSizeDelta;

                    newTiles[iNew, jNew] = prevTiles[i, j];
                }
            }
        }
    }
}