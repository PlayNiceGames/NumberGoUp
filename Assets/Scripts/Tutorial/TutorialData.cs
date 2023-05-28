using System.Collections.Generic;
using Tiles.Data;
using Tutorial.Steps.Data;
using UnityEngine;
using Utils;

namespace Tutorial
{
    public class TutorialData : ScriptableObject
    {
        [field: SerializeReference] public TileData DefaultTileInQueue { get; private set; }
        [field: SerializeField] public SerializedReferenceDictionary<int, TileData> TileQueueOverrides { get; private set; }

        [SerializeReference] private List<ITutorialStepData> _steps;

        public IEnumerable<ITutorialStepData> Steps => _steps;
    }
}