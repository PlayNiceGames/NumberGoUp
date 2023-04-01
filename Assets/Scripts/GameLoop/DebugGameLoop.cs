using System.Collections.Generic;
using GameBoard;
using GameLoop.GameRules;
using GameTileQueue;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLoop
{
    public class DebugGameLoop : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private Rules _rules;

        private void Start()
        {
            _rules.Setup();
            _board.Setup(7);
            _tileQueue.Setup();
        }
    }
}