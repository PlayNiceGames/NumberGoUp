using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameLoop
{
    public abstract class AbstractGameLoop : MonoBehaviour
    {
        public abstract UniTask Run();
    }
}