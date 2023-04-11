namespace GameBoard.Actions
{
    public abstract class BoardAction
    {
        public abstract void Run();
        public abstract void Undo();
    }
}