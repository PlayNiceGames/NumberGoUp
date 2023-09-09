using System;
using Tiles.Data;
using UnityEngine;

namespace GameBoard
{
    [Serializable]
    public class BoardData : ISerializationCallbackReceiver
    {
        [SerializeReference] public TileData[,] Tiles;

        [SerializeReference, HideInInspector] private TileData[] _tilesFlatten;
        [SerializeField, HideInInspector] private int _sizeX;
        [SerializeField, HideInInspector] private int _sizeY;

        public BoardData(TileData[,] tiles)
        {
            Tiles = tiles;
        }

        public static BoardData EmptySquare(int size)
        {
            TileData[,] data = new TileData[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    data[i, j] = new EmptyTileData();
                }
            }

            return new BoardData(data);
        }

        public void OnBeforeSerialize()
        {
            if (Tiles == null)
                return;

            _sizeX = Tiles.GetLength(0);
            _sizeY = Tiles.GetLength(1);

            _tilesFlatten = new TileData[_sizeX * _sizeY];

            int index = 0;
            foreach (TileData tile in Tiles)
            {
                _tilesFlatten[index] = tile;

                index++;
            }
        }

        public void OnAfterDeserialize()
        {
            if (_tilesFlatten == null)
                return;

            Tiles = new TileData[_sizeX, _sizeY];

            for (int i = 0; i < _sizeX; i++)
            {
                for (int j = 0; j < _sizeY; j++)
                {
                    int index = i * _sizeY + j;
                    Tiles[i, j] = _tilesFlatten[index];
                }
            }
        }
    }
}