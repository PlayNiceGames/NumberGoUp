using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class ListExtensions
    {
        public static T RandomItem<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
        
        public static int RandomIndex<T>(this List<T> list)
        {
            return Random.Range(0, list.Count);
        }
    }
}