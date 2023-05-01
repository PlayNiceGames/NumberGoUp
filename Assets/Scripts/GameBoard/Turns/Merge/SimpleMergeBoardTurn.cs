﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tiles.Containers;
using UnityEngine;

namespace GameBoard.Turns.Merge
{
    public class SimpleMergeBoardTurn : MergeBoardTurn
    {
        private IValueTileContainer _tileContainer;
        private IEnumerable<IValueTileContainer> _mergeTileContainers;
        private Board _board;

        public SimpleMergeBoardTurn(IValueTileContainer tileContainer, IEnumerable<IValueTileContainer> mergeTileContainers, Board board)
        {
            _tileContainer = tileContainer;
            _mergeTileContainers = mergeTileContainers;
            _board = board;
        }

        public override async UniTask Run()
        {
            Debug.Log($"{GetType()} turn");

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            IEnumerable<UniTask> mergeTasks = RunMergeTasks(_mergeTileContainers, _board);
            await UniTask.WhenAll(mergeTasks);

            _tileContainer.IncrementValue();
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}