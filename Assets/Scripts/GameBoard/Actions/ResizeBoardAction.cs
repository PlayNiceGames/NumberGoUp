using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameAudio;
using Tiles;

namespace GameBoard.Actions
{
    public class ResizeBoardAction : BoardAction
    {
        private int _newSize;
        private TileFactory _factory;
        private Audio _audio;
        private int currentScore;

        public ResizeBoardAction(int newSize, TileFactory factory, Board board, Audio audio, int score) : base(board)
        {
            _newSize = newSize;
            _factory = factory;
            _audio = audio;
            currentScore = score;
        }

        public override async UniTask Run()
        {
            if (Board.Size == _newSize)
                return;

            _audio.PlayBoardResize();
            Board.UpdateBoardSize(_newSize);
            await Board.Grid.WaitForGridUpdate();
            await PlaceEmptyTiles();
        }

        private async UniTask PlaceEmptyTiles()
        {
            List<UniTask> placeTileTasks = new List<UniTask>();
            foreach (VoidTile voidTile in Board.GetAllTiles<VoidTile>())
            {
                EmptyTile emptyTile = _factory.InstantiateTile<EmptyTile>();
                PlaceTileAction placeTileAction = new PlaceTileAction(emptyTile, voidTile.BoardPosition, Board);

                UniTask placeTileTask = placeTileAction.Run();
                placeTileTasks.Add(placeTileTask);
                await emptyTile.AppearAnimation.WaitForTilesAppearDelay();
            }
            await UniTask.WhenAll(placeTileTasks);
            if (_newSize > 5 && _newSize <= 7)
            {
                Board.interstitialAd.ShowInterstitialAd();
                if (_newSize == 7) Board.AdScore = currentScore;
            }
            else if (_newSize == 3) Board.AdScore = 0;


        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}