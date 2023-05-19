using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameBoard;
using GameBoard.Rules;
using GameBoard.Turns;
using GameDebug;
using GameLoop;
using GameLoop.Rules;
using GameScore;
using GameTileQueue;
using Tiles;
using UnityEngine;

namespace Tutorial
{
    public class TutorialLoop : AbstractGameLoop
    {
        [SerializeField] private TutorialData _data;
        [SerializeField] private Board _board;
        [SerializeField] private BoardInput _boardInput;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private GameRules _gameRules;
        [SerializeField] private ScoreSystem _scoreSystem;

        [SerializeField] private DebugController _debugController;

        private BoardRules _boardRules;

        private void Setup()
        {
            _boardRules = new BoardRules(_board, _scoreSystem);

            _gameRules.Setup();
            _tileQueue.Setup();

            int initialBoardSize = _gameRules.CurrentRules.BoardSize;
            _board.Setup(initialBoardSize);

            if (_debugController != null)
                _debugController.Setup();
        }

        public override async UniTask Run()
        {
            while (true)
            {
                await ProcessUserInput();

                await _boardRules.ProcessRules();

                await AgeTiles();

                _gameRules.UpdateCurrentRules();

                UpdateBoardSize();
            }
        }

        private void UpdateBoardSize()
        {
            int boardSize = _gameRules.CurrentRules.BoardSize;
            _board.UpdateBoardSize(boardSize);
        }

        private async Task ProcessUserInput()
        {
            Tile clickedTile = await _boardInput.WaitUntilTileClicked(TileType.Empty);

            Tile newTile = GetNextTile();
            newTile.transform.position = new Vector3(clickedTile.transform.position.x, clickedTile.transform.position.y, 0); //TODO temp

            _board.PlaceTile(newTile, clickedTile.BoardPosition);
        }

        private Tile GetNextTile()
        {
            if (IsDebugPlaceTiles())
                return _debugController.GetTestTile();

            return _tileQueue.GetNextTile();
        }

        private UniTask AgeTiles()
        {
            AgeTilesBoardTurn turn = new AgeTilesBoardTurn(_board);
            return turn.Run();
        }

        private bool IsDebugPlaceTiles()
        {
            return DebugController.IsDebug && _debugController.DebugPlaceTiles; //TODO rewrite debug
        }
    }
}