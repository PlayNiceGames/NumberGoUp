using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameBoard;
using GameBoard.Rules;
using GameBoard.Turns;
using GameDebug;
using GameLoop.Rules;
using GameTileQueue;
using Tiles;
using UnityEngine;

namespace GameLoop
{
    public class EndlessModeGameLoop : GameLoop
    {
        [SerializeField] private Board _board;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private GameRules _gameRules;
        [SerializeField] private BoardRules _boardRules;

        [SerializeField] private bool IsDebugMode;
        [SerializeField] private DebugController _debugController;

        private UniTaskCompletionSource<Tile> _emptyTileClicked;

        private void Start()
        {
            _gameRules.Setup();
            _tileQueue.Setup();

            _board.Setup(7);
            _board.OnTileClick += OnTileClicked;

            _boardRules = new BoardRules(_board);

            Run().Forget();
        }

        public override async UniTask Run()
        {
            while (true)
            {
                await ProcessUserInput();
                await ProcessRules();
                AgeTiles();
            }
        }

        private async UniTask ProcessRules()
        {
            while (true)
            {
                BoardTurn turn = _boardRules.GetFirstAvailableTurn();

                if (turn == null)
                    return;

                await turn.Run();
                await UniTask.WaitForEndOfFrame(this);

                Debug.Log($"RAN turn {turn.GetType()}");
            }
        }

        private async Task ProcessUserInput()
        {
            Tile clickedTile = await WaitTileClicked();

            Tile newTile = GetNextTile();
            newTile.transform.position = new Vector3(clickedTile.transform.position.x, clickedTile.transform.position.y, 0); //TODO temp

            _board.PlaceTile(newTile, clickedTile.BoardPosition);
        }

        private Tile GetNextTile()
        {
            if (DebugController.IsDebug && _debugController.DebugPlaceTiles)
                return _debugController.GetTestTile();

            return _tileQueue.GetNextTile();
        }

        private UniTask<Tile> WaitTileClicked()
        {
            _emptyTileClicked = new UniTaskCompletionSource<Tile>();
            return _emptyTileClicked.Task;
        }

        private void OnTileClicked(Tile tile)
        {
            if (tile.Type == TileType.Empty)
            {
                _emptyTileClicked?.TrySetResult(tile);
            }
        }

        private void AgeTiles()
        {
            AgeTilesBoardTurn turn = new AgeTilesBoardTurn(_board);
            turn.Run().Forget();
        }
    }
}