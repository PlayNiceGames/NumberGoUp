using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class TileColors : ScriptableObject
    {
        [SerializeField] private List<Color> Colors;

        public Color GetColor(int colorIndex)
        {
            return Colors[colorIndex];
        }

        public int GetRandomColorIndex()
        {
            return Random.Range(0, Colors.Count);
        }
    }
}