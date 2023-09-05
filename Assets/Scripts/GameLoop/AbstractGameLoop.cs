using Cysharp.Threading.Tasks;
using GameSave;
using UnityEngine;

namespace GameLoop
{
    public abstract class AbstractGameLoop : MonoBehaviour
    {
        public abstract void SetupEmptyGame();
        public abstract void SetupFromSavedGame(GameData currentSaveToLoad);
        public abstract UniTask Run();
    }
}