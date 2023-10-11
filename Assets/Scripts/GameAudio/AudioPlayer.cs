using System;
using Cysharp.Threading.Tasks;
using GameAudio.Mixer;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;

namespace GameAudio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;

        [SerializeField] private AudioPlayerMixer _masterMixer;
        [SerializeField] private AudioPlayerMixer _soundsMixer;
        [SerializeField] private AudioPlayerMixer _musicMixer;

        private AudioSource _musicSource;

        private void Start()
        {
            SetupMixers();

            Load();
        }

        private void SetupMixers()
        {
            _masterMixer.Setup(_mixer);
            _soundsMixer.Setup(_mixer);
            _musicMixer.Setup(_mixer);
        }

        public AudioSource PlaySound(AudioClip clip, bool loop = false)
        {
            AudioSource source = InstantiateAudioSource(clip);
            source.outputAudioMixerGroup = _soundsMixer.MixerGroup;
            source.loop = loop;
            source.Play();
            WaitAndDispose(source).Forget();

            return source;
        }

        public UniTask PlaySoundAsync(AudioClip clip)
        {
            AudioSource source = InstantiateAudioSource(clip);
            source.Play();

            return WaitAndDispose(source);
        }

        public AudioSource PlayMusic(AudioClip clip)
        {
            if (_musicSource != null)
                Destroy(_musicSource.gameObject);

            AudioSource source = InstantiateAudioSource(clip);
            source.outputAudioMixerGroup = _musicMixer.MixerGroup;
            source.loop = true;
            source.Play();

            _musicSource = source;

            return source;
        }

        private AudioSource InstantiateAudioSource(AudioClip clip)
        {
            string name = $"(Sound) {clip.name}";
            GameObject audioObject = new GameObject(name);

            AudioSource source = audioObject.AddComponent<AudioSource>();
            source.clip = clip;

            return source;
        }

        private async UniTask WaitAndDispose(AudioSource source)
        {
            await UniTask.WaitWhile(() => source != null && (source.isPlaying || source.loop));

            if (source != null)
                Destroy(source.gameObject);
        }

        public AudioPlayerMixer GetMixer(AudioPlayerMixerType type)
        {
            switch (type)
            {
                case AudioPlayerMixerType.Master:
                    return _masterMixer;
                case AudioPlayerMixerType.Sounds:
                    return _soundsMixer;
                case AudioPlayerMixerType.Music:
                    return _musicMixer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void Save()
        {
            SaveMixer(_masterMixer);
            SaveMixer(_soundsMixer);
            SaveMixer(_musicMixer);
        }

        private void SaveMixer(AudioPlayerMixer mixer)
        {
            string jsonData = JsonConvert.SerializeObject(mixer);

            string key = GetMixerSaveKey(mixer);
            PlayerPrefs.SetString(key, jsonData);
        }

        private void Load()
        {
            LoadMixer(_masterMixer);
            LoadMixer(_soundsMixer);
            LoadMixer(_musicMixer);
        }

        private void LoadMixer(AudioPlayerMixer mixer)
        {
            string key = GetMixerSaveKey(mixer);

            if (!PlayerPrefs.HasKey(key))
                return;

            string jsonData = PlayerPrefs.GetString(key);

            try
            {
                JsonConvert.PopulateObject(jsonData, mixer);
                
                mixer.UpdateVolume();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private string GetMixerSaveKey(AudioPlayerMixer mixer)
        {
            return $"audio_player_mixer_{mixer.Path}";
        }
    }
}