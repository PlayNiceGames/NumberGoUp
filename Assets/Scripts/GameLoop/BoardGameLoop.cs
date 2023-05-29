using Cysharp.Threading.Tasks;
using GameBoard;
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

            AgeTiles();
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