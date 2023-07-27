﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameAudio;
using GameBoard.Actions;
using GameScore;
using ServiceLocator;
using Tiles.Containers;

namespace GameBoard.Turns.Merge
{
    public class CenterMergeBoardTurn : MergeBoardTurn
    {
        private MergeContainer _tileContainer;
        private IEnumerable<MergeContainer> _mergeTileContainers;

        public CenterMergeBoardTurn(MergeContainer tileContainer, IEnumerable<MergeContainer> mergeTileContainers, Board board, ScoreSystem scoreSystem) : base(board, scoreSystem)
        {
            _tileContainer = tileContainer;
            _mergeTileContainers = mergeTileContainers;
            _board = board;
        }

        public override async UniTask Run()
        {
            Audio audio = GlobalServices.Get<Audio>();
            
            int containersCount = _mergeTileContainers.Count();
            int newValue = _tileContainer.GetValue() + containersCount;

            PlayMergeSound(newValue);

            int scoreDelta = ScoreSystem.GetScoreForMerge(_tileContainer, containersCount);
            UniTask scoreTask = ScoreSystem.IncrementScore(scoreDelta);

            IEnumerable<BoardAction> mergeActions = GetMergeActions(_mergeTileContainers, _board);
            UniTask mergeTask = RunMergeActions(mergeActions);

            await UniTask.WhenAll(mergeTask, scoreTask);

            _tileContainer.IncrementValue(containersCount);
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}