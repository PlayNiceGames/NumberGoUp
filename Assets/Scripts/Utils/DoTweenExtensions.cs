using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Utils
{
    public static class DoTweenExtensions
    {
        public static UniTask PlayAsync(this Tween tween)
        {
            tween.Play();

            return tween.AsyncWaitForCompletion().AsUniTask();
        }
    }
}