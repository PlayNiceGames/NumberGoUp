using Cysharp.Threading.Tasks;

namespace Tutorial
{
    public abstract class TutorialStep
    {
        public abstract UniTask Run();
    }
}