using System;
using Cysharp.Threading.Tasks;
using Tutorial.Steps.Data;

namespace Tutorial.Steps
{
    public class DelayTutorialStep : TutorialStep
    {
        private DelayTutorialStepData _data;

        public DelayTutorialStep(DelayTutorialStepData data)
        {
            _data = data;
        }

        public override async UniTask<bool> Run()
        {
            await UniTask.WaitForSeconds(_data.DelaySeconds);

            return false;
        }
    }
}