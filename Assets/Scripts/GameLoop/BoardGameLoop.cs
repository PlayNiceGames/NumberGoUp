using Cysharp.Threading.Tasks;
using GameBoard;
using GameBoard.Actions;
using GameBoard.Rules;
using GameBoard.Turns;
using GameDebug;
using GameScore;
using GameTileQueue;
using Tiles;
using UnityEngine;

namespace GameLoop
{
    public class BoardGameLoop : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private TileFactory _factory;

        [SerializeField] private DebugTilePlacer _debugTilePlacer;

        private BoardRules _boardRules;

        private void Awake()
        {
            _boardRules = new BoardRules(_board, _scoreSystem, _factory);
        }

        public async UniTask ProcessBoard()
        {
            await _boardRules.ProcessRules();

            await AgeTiles();
        }

        public async UniTask<bool> ProcessInput(Tile clickedTile)
        {
            TileType nextTileType = GetNextTileType();

            bool canReplaceTile = CanReplaceTile(clickedTile.Type, nextTileType);

            if (!canReplaceTile)
                return false;

            Vector2Int clickTilePosition = clickedTile.BoardPosition;

            Tile nextTileQueueTile = PopNextTile();

            PlaceTileAction placeTileAction = new PlaceTileAction(nextTileQueueTile, clickTilePosition, _board);

            UniTask placeTileTask = placeTileAction.Run();
            UniTask tileQueueAnimationTask = _tileQueue.WaitUntilTileAdvances();

            await UniTask.WhenAll(placeTileTask, tileQueueAnimationTask);

            return true;
        }

        private bool CanReplaceTile(TileType originalTileType, TileType newTileType)
        {
            if (IsDebugPlaceTiles())
                return true;

            switch (newTileType)
            {
                case TileType.Eraser:
                    return originalTileType is TileType.Empty or TileType.Regular or TileType.Mixed;
                default:
                    return originalTileType is TileType.Empty;
            }
        }

        private TileType GetNextTileType()
        {
            if (IsDebugPlaceTiles())
                return _debugTilePlacer.PeekNextTileType();

            Tile nextTileQueueTile = _tileQueue.PeekNextTile();
            return nextTileQueueTile.Type;
        }

        private Tile PopNextTile()
        {
            if (IsDebugPlaceTiles())
                return _debugTilePlacer.PopNextTile();

            return _tileQueue.PopNextTile();
        }

        private UniTask AgeTiles()
        {
            AgeTilesBoardTurn turn = new AgeTilesBoardTurn(_board);
            return turn.Run();
        }

        private bool IsDebugPlaceTiles()
        {
            return DebugController.IsDebug && _debugTilePlacer.DebugPlaceTiles; //TODO rewrite debug
        }
    }
}