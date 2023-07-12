using UnityEngine;

namespace GameAudio
{
    public static class GameSounds //TODO (hack) should be in Zenject as service, not static
    {
        private const string SoundDatabaseAssetName = "SoundsDatabase";

        private static SoundsDatabase _sounds;

        private static AudioSource _musicSource;

        static GameSounds()
        {
            LoadDatabase();
        }

        private static void LoadDatabase()
        {
            _sounds = Resources.Load<SoundsDatabase>(SoundDatabaseAssetName);

            if (_sounds == null)
                Debug.LogError("Sounds database is missing");
        }

        public static void PlayMusic()
        {
            if (_musicSource == null)
            {
                _musicSource = AudioPlayer.Play(_sounds.Music, true);
                Object.DontDestroyOnLoad(_musicSource.gameObject);
            }
            else
            {
                _musicSource.UnPause();
            }
        }

        public static void PauseMusic()
        {
            if (_musicSource != null)
                _musicSource.Pause();
        }

        public static void PlayClick()
        {
            AudioPlayer.Play(_sounds.Click);
        }

        public static void PlayEraser()
        {
            AudioPlayer.Play(_sounds.Eraser);
        }

        public static void PlayGameOver()
        {
            AudioPlayer.Play(_sounds.GameOver);
        }

        public static void PlayBoardResize()
        {
            AudioPlayer.Play(_sounds.BoardResize);
        }

        public static void PlayMerge(int newValue)
        {
            AudioClip clip = _sounds.GetMergeSound(newValue);
            AudioPlayer.Play(clip);
        }
    }
}