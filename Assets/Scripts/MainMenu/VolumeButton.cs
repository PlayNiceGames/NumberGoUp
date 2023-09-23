using System;
using GameAudio;
using GameAudio.Mixer;
using JetBrains.Annotations;
using ServiceLocator;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class VolumeButton : MonoBehaviour
    {
        [SerializeField] private AudioPlayerMixerType _mixerType;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _mutedImage;
        [SerializeField] private Sprite _unMutedImage;

        private AudioPlayer _player;
        private AudioPlayerMixer _mixer;

        private void Start()
        {
            _player = GlobalServices.Get<AudioPlayer>();

            _mixer = _player.GetMixer(_mixerType);
            _mixer.OnVolumeChanged += OnVolumeChanged;
            
            UpdateImage(_mixer.IsMuted);
        }

        private void OnDestroy()
        {
            _mixer.OnVolumeChanged -= OnVolumeChanged;
        }

        private void OnVolumeChanged(float volume, bool isMuted)
        {
            UpdateImage(isMuted);
        }

        private void UpdateImage(bool isMuted)
        {
            _image.sprite = isMuted ? _mutedImage : _unMutedImage;
        }

        [UsedImplicitly]
        public void ClickButton()
        {
            _mixer.IsMuted = !_mixer.IsMuted;
            
            _player.Save();
        }
    }
}