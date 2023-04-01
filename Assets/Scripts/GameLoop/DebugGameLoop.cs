using GameBoard;
using GameLoop.GameRules;
using TileQueue;
using Tiles;
using UnityEngine;

namespace GameLoop
{
    public class DebugGameLoop : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private Queue _queue;
        [SerializeField] private RulesData _initialRules;
        
        private void Start()
        {
            _initialRules.Setup(null);

            _board.Setup(7);
            _queue.Setup(_initialRules);
        }
    }
}