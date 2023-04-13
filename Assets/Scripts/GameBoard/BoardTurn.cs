using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameBoard.Actions;
using UnityEngine;

namespace GameBoard
{
    public class BoardTurn
    {
        private List<BoardAction> _actions;

        public BoardTurn(List<BoardAction> actions)
        {
            _actions = actions;
        }

        public UniTask Run()
        {
            Debug.Log("RUN TURN");
            
            List<UniTask> runTasks = new List<UniTask>();

            foreach (BoardAction action in _actions)
            {
                runTasks.Add(action.Run());
            }

            return UniTask.WhenAll(runTasks);
        }

        public UniTask Undo()
        {
            List<UniTask> undoTasks = new List<UniTask>();

            foreach (BoardAction action in _actions)
            {
                undoTasks.Add(action.Undo());
            }

            return UniTask.WhenAll(undoTasks);
        }
    }
}