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
        [SerializeField] private BoardInput _boardInput;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private ScoreSystem _scoreSystem;

        [SerializeField] private DebugTilePlacer _debugTilePlacer;

        private BoardRules _boardRules;

        public void Setup()
        {
            _boardRules = new BoardRules(_board, _scoreSystem);

            _debugTilePlacer.Setup();
        }

        public async UniTask Run()
        {
            await ProcessUserInput();

            await _boardRules.ProcessRules();

            await AgeTiles();
        }

        private async UniTask ProcessUserInput()
        {
            TileType nextTileType = GetNextTileType();

            Tile clickedTile = await WaitTileClicked(nextTileType);
            Vector2Int clickTilePosition = clickedTile.BoardPosition;

            Tile nextTileQueueTile = PopNextTile();

            PlaceTileAction placeTileAction = new PlaceTileAction(_board, nextTileQueueTile, clickTilePosition);
            await placeTileAction.Run();
        }

        private async UniTask<Tile> WaitTileClicked(TileType nextTileType)
        {
            while (true)
            {
                Tile clickedTile = await _boardInput.WaitUntilTileClicked();

                bool canReplaceTile = CanReplaceTile(clickedTile.Type, nextTileType);

                if (canReplaceTile)
                    return clickedTile;
            }
        }

        private bool CanReplaceTile(TileType originalTileType, TileType newTileType)
        {
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