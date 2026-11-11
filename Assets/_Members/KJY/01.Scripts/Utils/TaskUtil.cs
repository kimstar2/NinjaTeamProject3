using System.Threading;

namespace _Members.KJY._01.Scripts.Utils
{
    public static class TaskUtil
    {
        public static void KillToken(this CancellationTokenSource cts)
        {
            cts.Cancel();
            cts.Dispose();
        }   
    }
}