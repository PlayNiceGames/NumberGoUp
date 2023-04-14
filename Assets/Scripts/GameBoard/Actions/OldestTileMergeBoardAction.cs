using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameBoard.Actions
{
    public class OldestTileMergeBoardAction : BoardAction
    {
        public override async UniTask Run()
        {
            Debug.Log("RUN ACTION START");

            await UniTask.Delay(TimeSpan.FromSeconds(3));
            
            Debug.Log("RUN ACTION FINISH");
        }

        public override UniTask Undo()
        {
            throw new NotImplementedException();
        }
    }
}