using Cysharp.Threading.Tasks;
using GameLoop.Tutorial;

namespace Tutorial.Steps
{
    public abstract class TutorialStep
    {
        public abstract UniTask<TutorialStepResult> Run();
    }
}