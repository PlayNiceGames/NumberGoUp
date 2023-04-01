using System.Collections.Generic;
using GameBoard;
using GameLoop.GameRules;
using TileQueue;
using UnityEngine;

namespace GameLoop
{
    public class DebugGameLoop : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private Queue _queue;
        [SerializeField] private Rules _rules;

        private void Start()
        {
            _rules.Setup();
            _board.Setup(7);
            _queue.Setup();
        }
    }
}