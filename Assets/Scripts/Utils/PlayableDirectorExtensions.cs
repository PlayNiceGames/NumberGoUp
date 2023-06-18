using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

namespace Utils
{
    public static class PlayableDirectorExtensions
    {
        public static UniTask PlayAsync(this PlayableDirector director, PlayableAsset asset)
        {
            director.Play(asset);

            return director.WaitForCompletion();
        }

        public static UniTask PlayAsync(this PlayableDirector director, PlayableAsset asset, DirectorWrapMode mode)
        {
            director.Play(asset, mode);

            return director.WaitForCompletion();
        }

        public static UniTask WaitForCompletion(this PlayableDirector director)
        {
            return UniTask.WaitWhile(director.IsPlaying);
        }

        public static bool IsPlaying(this PlayableDirector director)
        {
            return director.state == PlayState.Playing;
        }

        public static bool IsPlaying(this PlayableDirector director, PlayableAsset asset)
        {
            return director.IsPlaying() && director.playableAsset == asset;
        }
    }
}