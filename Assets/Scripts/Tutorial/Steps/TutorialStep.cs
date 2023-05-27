using Cysharp.Threading.Tasks;

namespace Tutorial.Steps
{
    public abstract class TutorialStep
    {
        public abstract UniTask Run();
    }
}