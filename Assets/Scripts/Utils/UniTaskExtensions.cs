using System.Threading;
using Cysharp.Threading.Tasks;

namespace Utils
{
    public static class UniTaskExtensions
    {
        public static bool IsCompleted(this UniTask task)
        {
            return task.Status == UniTaskStatus.Succeeded;
        }

        public static UniTaskCompletionSource AttachCancellationToken(this UniTaskCompletionSource completionSource, CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => completionSource.TrySetCanceled());

            return completionSource;
        }
        
        public static UniTaskCompletionSource<T> AttachCancellationToken<T>(this UniTaskCompletionSource<T> completionSource, CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => completionSource.TrySetCanceled());

            return completionSource;
        }
    }
}