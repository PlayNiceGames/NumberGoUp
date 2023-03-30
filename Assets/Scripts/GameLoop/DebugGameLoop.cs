using System;
using GameLoop.Rules;
using UnityEngine;

namespace GameLoop
{
    public class DebugGameLoop : MonoBehaviour
    {
        [SerializeField] private Board.Board _board;
        
        [SerializeField] private GameRules _initialRules;

        private void Awake()
        {
            
        }
    }
}