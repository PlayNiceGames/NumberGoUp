using System;
using Sirenix.OdinInspector;
using Tile;
using UnityEngine;

namespace Board
{
    public class BoardDebugger : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private TileFactory _factory;

        private void Awake()
        {
            CreateBoard(7);
        }

        [Button, DisableInEditorMode]
        public void CreateBoard(int size)
        {
            _board.CreateBoard(size);
        }

        [Button, DisableInEditorMode]
        public void PlaceTile(TileType type, int x, int y)
        {
            TileBase tile = _factory.InstantiateTile(type);
            
            _board.PlaceTile(tile, new Vector2Int(x, y));
        }
        
        [Button, DisableInEditorMode]
        public void PlaceRegularTile(int x, int y, int value, int color)
        {
            RegularTile tile = _factory.InstantiateTile<RegularTile>();
            tile.SetNumber(value);
            tile.SetColor(color);

            _board.PlaceTile(tile, new Vector2Int(x, y));
        }
    }
}