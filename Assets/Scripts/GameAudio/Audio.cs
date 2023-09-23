using ServiceLocator;
using UnityEngine;

namespace GameAudio
{
    public class Audio : MonoBehaviour
    {
        [SerializeField] private SoundsDatabase _sounds;

        private AudioPlayer _player;

        private AudioSource _musicSource;

        private void Start()
        {
            _player = GlobalServices.Get<AudioPlayer>();

            PlayMusic();
        }

        public void PlayMusic()
        {
            if (_musicSource == null)
            {
                _musicSource = _player.PlayMusic(_sounds.Music);
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
            _player.PlaySound(_sounds.Click);
        }

        public void PlayEraser()
        {
            _player.PlaySound(_sounds.Eraser);
        }

        public void PlayGameOver()
        {
            _player.PlaySound(_sounds.GameOver);
        }

        public void PlayBoardResize()
        {
            _player.PlaySound(_sounds.BoardResize);
        }

        public void PlayMerge(int newValue)
        {
            AudioClip clip = _sounds.GetMergeSound(newValue);
            _player.PlaySound(clip);
        }
    }
}