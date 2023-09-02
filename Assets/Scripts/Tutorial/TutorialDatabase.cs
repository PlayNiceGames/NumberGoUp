using System.Collections.Generic;
using Tiles.Data;
using Tutorial.Steps.Data;
using UnityEngine;
using Utils;

namespace Tutorial
{
    public class TutorialDatabase : ScriptableObject
    {
        [field: SerializeReference] public TileData DefaultTileInQueue { get; private set; }
        [field: SerializeField] public SerializedReferenceDictionary<int, TileData> TileQueueOverrides { get; private set; }

        [field: SerializeReference] private List<TutorialStepData> _steps;

        public IEnumerable<TutorialStepData> Steps => _steps;
    }
}