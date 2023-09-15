using Cysharp.Threading.Tasks;

namespace Utils
{
    public static class UniTaskExtensions
    {
        public static bool IsCompleted(this UniTask task)
        {
            return task.Status == UniTaskStatus.Succeeded;
        }
    }
}