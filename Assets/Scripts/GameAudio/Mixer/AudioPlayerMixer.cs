using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;

namespace GameAudio.Mixer
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AudioPlayerMixer
    {
        private const float DefaultVolume = 0.5f;

        public delegate void VolumeChangedDelegate(float volume, bool isMuted);
        public event VolumeChangedDelegate OnVolumeChanged;

        private string _volumeParameterName;
        private AudioMixer _mixer;

        public string Path { get; private set; }
        public AudioMixerGroup MixerGroup { get; private set; }

        [JsonProperty]
        private float _volume = DefaultVolume;

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

        public AudioPlayerMixer(string path, string volumeParameterName, AudioMixer mixer)
        {
            Path = path;
            _mixer = mixer;
            _volumeParameterName = volumeParameterName;
            MixerGroup = GetGroup(path);
        }

        public void Setup()
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
            _mixer.SetFloat(_volumeParameterName, dbVolume);

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
            _mixer.SetFloat(_volumeParameterName, dbVolume);

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