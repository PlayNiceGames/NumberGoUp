﻿using System;
using Cysharp.Threading.Tasks;
using GameBoard;
using GameRules;
using GameTileQueue;
using Tiles;
using UnityEngine;

namespace GameLoop
{
    public class EndlessModeGameLoop : GameLoop
    {
        [SerializeField] private Board _board;
        [SerializeField] private TileQueue _tileQueue;
        [SerializeField] private Rules _rules;

        private void Start()
        {
            _rules.Setup();
            _tileQueue.Setup();

            _board.Setup(7);
            _board.OnTileClick += OnTileClicked;

            Run().Forget();
        }

        public override UniTask Run()
        {
            return UniTask.CompletedTask;
        }

        private void OnTileClicked(Tile tile)
        {
            if (tile.Type == TileType.Empty)
            {
                Tile newTile = _tileQueue.PlaceTileOnBoard(tile.transform.position);

                _board.PlaceTile(newTile, tile.BoardPosition);
            }
        }
    }
}