using Cysharp.Threading.Tasks;

namespace GameBoard.Actions
{
    public abstract class BoardAction
    {
        public abstract UniTask Run();
        public abstract UniTask Undo();
    }
}