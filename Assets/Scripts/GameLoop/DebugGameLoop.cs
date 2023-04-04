using Cysharp.Threading.Tasks;
using GameBoard;
using GameLoop.GameRules;
using GameTileQueue;
using Tiles;
using UnityEngine;

namespace GameLoop
{
    public class DebugGameLoop : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private Rules _rules;

        private void Start()
        {
            _rules.Setup();
            _tileQueue.Setup();

            _board.Setup(7);
            _board.OnTileClick += OnTileClicked;
        }

        private UniTask GameLoop()
        {
            return UniTask.CompletedTask;
        }

        private void OnTileClicked(Tile tile)
        {
            if (tile.Type == TileType.Empty)
            {
                Tile newTile = _tileQueue.PlaceTileOnBoard(tile.transform.position);

                _board.PlaceTile(newTile, tile.BoardPosition);
            }
        }
    }
}