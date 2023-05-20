using Cysharp.Threading.Tasks;
using GameBoard;
using GameBoard.Rules;
using GameBoard.Turns;
using GameDebug;
using GameLoop.Rules;
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
        [SerializeField] private GameRules _gameRules;
        [SerializeField] private ScoreSystem _scoreSystem;

        [SerializeField] private DebugTilePlacer _debugTilePlacer;

        private BoardRules _boardRules;

        public void Setup()
        {
            _boardRules = new BoardRules(_board, _scoreSystem);

            _gameRules.Setup();
            _tileQueue.Setup();

            int initialBoardSize = _gameRules.CurrentRules.BoardSize;
            _board.Setup(initialBoardSize);
        }

        public async UniTask Run()
        {
            await ProcessUserInput();

            await _boardRules.ProcessRules();
            _gameRules.UpdateCurrentRules();

            AgeTiles();

            UpdateBoardSize();
        }

        private void UpdateBoardSize()
        {
            int boardSize = _gameRules.CurrentRules.BoardSize;
            _board.UpdateBoardSize(boardSize);
        }

        private async UniTask ProcessUserInput()
        {
            Tile clickedTile = await _boardInput.WaitUntilTileClicked(TileType.Empty);

            Tile newTile = GetNextTile();
            newTile.transform.position = new Vector3(clickedTile.transform.position.x, clickedTile.transform.position.y, 0); //TODO temp

            _board.PlaceTile(newTile, clickedTile.BoardPosition);
        }

        private Tile GetNextTile()
        {
            if (IsDebugPlaceTiles())
                return _debugTilePlacer.GetNextTile();

            return _tileQueue.GetNextTile();
        }

        private void AgeTiles()
        {
            AgeTilesBoardTurn turn = new AgeTilesBoardTurn(_board);
            turn.Run().Forget();
        }

        private bool IsDebugPlaceTiles()
        {
            return DebugController.IsDebug && _debugTilePlacer.DebugPlaceTiles; //TODO rewrite debug
        }
    }
}