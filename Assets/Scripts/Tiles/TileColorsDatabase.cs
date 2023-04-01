using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tiles
{
    public class TileColorsDatabase : ScriptableObject
    {
        [SerializeField] private List<Color> Colors;

        public Color GetColor(int colorIndex)
        {
            return Colors[colorIndex];
        }

        public int GetRandomColor()
        {
            return Random.Range(0, Colors.Count);
        }

        public List<int> GetRandomColors()
        {
            return Enumerable.Range(0, Colors.Count).OrderBy(_ => Random.value).ToList();
        }
    }
}