using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Utils
{
    public static class DoTweenExtensions
    {
        public static async UniTask PlayAsync(this Tween tween)
        {
            tween.Play();

            await tween.AsyncWaitForCompletion();
        }
    }
}