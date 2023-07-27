using ServiceLocator;
using UnityEngine;

namespace GameAudio
{
    public class Audio : MonoBehaviour
    {
        [SerializeField] private SoundsDatabase _sounds;
        [SerializeField] private bool _isMusicEnabled_TEST;

        private AudioPlayer _player;

        private AudioSource _musicSource;

        private void Awake()
        {
            _player = GlobalServices.Get<AudioPlayer>();

            if (_isMusicEnabled_TEST)
                PlayMusic();
        }

        public void PlayMusic()
        {
            if (_musicSource == null)
            {
                _musicSource = _player.Play(_sounds.Music, true);
                DontDestroyOnLoad(_musicSource.gameObject);
            }
            else
            {
                _musicSource.UnPause();
            }
        }

        public void PauseMusic()
        {
            if (_musicSource != null)
                _musicSource.Pause();
        }

        public void PlayClick()
        {
            _player.Play(_sounds.Click);
        }

        public void PlayEraser()
        {
            _player.Play(_sounds.Eraser);
        }

        public void PlayGameOver()
        {
            _player.Play(_sounds.GameOver);
        }

        public void PlayBoardResize()
        {
            _player.Play(_sounds.BoardResize);
        }

        public void PlayMerge(int newValue)
        {
            AudioClip clip = _sounds.GetMergeSound(newValue);
            _player.Play(clip);
        }
    }
}