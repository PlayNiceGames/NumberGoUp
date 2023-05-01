﻿using System;
using Cysharp.Threading.Tasks;
using Tiles;

namespace GameBoard.Actions.Merge
{
    public class MergeRegularTileBoardAction : BoardAction
    {
        private RegularTile _tile;
        private Board _board;

        public MergeRegularTileBoardAction(RegularTile tile, Board board)
        {
            _board = board;
            _tile = tile;
        }

        public override UniTask Run()
        {
            _board.CreateEmptyTile(_tile.BoardPosition); //TODO free tile from board and animate it
            
            return UniTask.CompletedTask;
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}