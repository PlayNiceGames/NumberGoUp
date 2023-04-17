using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameBoard.Actions;

namespace GameBoard.Turns
{
    public abstract class BoardTurn
    {
        public abstract UniTask Run();
        public abstract UniTask Undo();

        protected UniTask RunParallelActions(List<BoardAction> actions)
        {
            List<UniTask> runTasks = new List<UniTask>();

            foreach (BoardAction action in actions)
            {
                runTasks.Add(action.Run());
            }

            return UniTask.WhenAll(runTasks);
        }

        protected UniTask UndoParallelActions(List<BoardAction> actions)
        {
            List<UniTask> undoTasks = new List<UniTask>();

            foreach (BoardAction action in actions)
            {
                undoTasks.Add(action.Undo());
            }

            return UniTask.WhenAll(undoTasks);
        }
    }
}