using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameAudio
{
    public static class AudioPlayer
    {
        public static AudioSource Play(AudioClip clip, bool loop = false)
        {
            AudioSource source = InstantiateAudioSource(clip);
            source.loop = loop;
            source.Play();

            WaitAndDispose(source).Forget();

            return source;
        }

        public static UniTask PlayAsync(AudioClip clip)
        {
            AudioSource source = InstantiateAudioSource(clip);
            source.Play();

            return WaitAndDispose(source);
        }

        private static AudioSource InstantiateAudioSource(AudioClip clip)
        {
            string name = $"(Sound) {clip.name}";
            GameObject audioObject = new GameObject(name);

            AudioSource source = audioObject.AddComponent<AudioSource>();
            source.clip = clip;

            return source;
        }

        private static async UniTask WaitAndDispose(AudioSource source)
        {
            await UniTask.WaitWhile(() => source != null && source.isPlaying);

            if (source != null)
                Object.Destroy(source.gameObject);
        }
    }
}