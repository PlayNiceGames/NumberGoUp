using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameAudio
{
    public class AudioPlayer : MonoBehaviour
    {
        public AudioSource Play(AudioClip clip, bool loop = false)
        {
            AudioSource source = InstantiateAudioSource(clip);
            source.loop = loop;
            source.Play();

            WaitAndDispose(source).Forget();

            return source;
        }

        public UniTask PlayAsync(AudioClip clip)
        {
            AudioSource source = InstantiateAudioSource(clip);
            source.Play();

            return WaitAndDispose(source);
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
            await UniTask.WaitWhile(() => source != null && source.isPlaying);

            if (source != null)
                Destroy(source.gameObject);
        }
    }
}