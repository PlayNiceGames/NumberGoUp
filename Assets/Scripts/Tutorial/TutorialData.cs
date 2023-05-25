using System.Collections.Generic;
using Tiles.Data;
using Tutorial.Data;
using UnityEngine;

namespace Tutorial
{
    public class TutorialData : ScriptableObject
    {
        [field: SerializeReference] public TileData DefaultTileInQueue { get; private set; }

        [SerializeReference] private List<ITutorialStepData> _steps;

        public IEnumerable<ITutorialStepData> Steps => _steps;
    }
}