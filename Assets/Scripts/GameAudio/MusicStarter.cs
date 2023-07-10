using UnityEngine;

namespace GameAudio
{
    public class MusicStarter : MonoBehaviour
    {
        private void Start()
        {
            GameSounds.PlayMusic();
        }
    }
}