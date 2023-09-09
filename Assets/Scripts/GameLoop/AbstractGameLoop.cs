using Cysharp.Threading.Tasks;
using GameSave;
using UnityEngine;

namespace GameLoop
{
    public abstract class AbstractGameLoop : MonoBehaviour
    {
        public abstract void Setup();
        public abstract UniTask RunNewGame();
        public abstract UniTask RunSavedGame(GameData save);
        protected abstract UniTask Run();
    }
}