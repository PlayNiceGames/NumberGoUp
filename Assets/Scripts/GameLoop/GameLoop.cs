using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameLoop
{
    public abstract class GameLoop : MonoBehaviour
    {
        public abstract UniTask Run();
    }
}