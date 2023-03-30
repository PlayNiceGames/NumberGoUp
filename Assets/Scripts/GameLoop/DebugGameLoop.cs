using System;
using GameLoop.Rules;
using UnityEngine;

namespace GameLoop
{
    public class DebugGameLoop : MonoBehaviour
    {
        [SerializeField] private GameBoard.Board _board;
        
        [SerializeField] private GameRules _initialRules;

        private void Awake()
        {
            
        }
    }
}