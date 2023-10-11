using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;

namespace GameAudio.Mixer
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class AudioPlayerMixer
    {
        public delegate void VolumeChangedDelegate(float volume, bool isMuted);
        public event VolumeChangedDelegate OnVolumeChanged;

        public float DefaultVolume = 0.5f;

        public string Path;
        public string VolumeParameterName;
        
        private AudioMixer _mixer;

        public AudioMixerGroup MixerGroup { get; private set; }

        [JsonProperty]
        private float _volume;

        public float Volume
        {
            get => _volume;
            set => SetVolume(value);
        }

        [JsonProperty]
        private bool _isMuted;

        public bool IsMuted
        {
            get => _isMuted;
            set => SetMuted(value);
        }

        public void Setup(AudioMixer mixer)
        {
            _mixer = mixer;
            _volume = DefaultVolume;
            
            MixerGroup = GetGroup(Path);
            
            UpdateVolume();
        }

        public void UpdateVolume()
        {
            if (_isMuted)
                SetMuted(_isMuted);
            else
                SetVolume(_volume);
        }

        private AudioMixerGroup GetGroup(string path)
        {
            return _mixer.FindMatchingGroups(path)[0];
        }

        private void SetVolume(float volume)
        {
            float dbVolume = LinearToDecibel(volume);
            _mixer.SetFloat(VolumeParameterName, dbVolume);

            _isMuted = volume == 0;
            _volume = volume;

            OnVolumeChanged?.Invoke(_volume, _isMuted);
        }

        public void SetMuted(bool isMuted)
        {
            float mixerVolume = isMuted ? 0 : _volume;

            if (_isMuted && _volume == 0)
                mixerVolume = DefaultVolume;

            float dbVolume = LinearToDecibel(mixerVolume);
            _mixer.SetFloat(VolumeParameterName, dbVolume);

            _isMuted = isMuted;

            OnVolumeChanged?.Invoke(_volume, _isMuted);
        }

        private float LinearToDecibel(float linear)
        {
            float dB;

            if (linear != 0)
                dB = 20.0f * Mathf.Log10(linear);
            else
                dB = -144.0f;

            return dB;
        }
    }
}