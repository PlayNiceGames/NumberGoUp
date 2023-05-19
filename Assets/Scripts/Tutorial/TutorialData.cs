using System.Collections.Generic;
using Tutorial.Data;
using UnityEngine;

namespace Tutorial
{
    public class TutorialData : ScriptableObject
    {
        [SerializeReference] private List<ITutorialStepData> _steps;

        public IEnumerable<ITutorialStepData> Steps => _steps;
    }
}