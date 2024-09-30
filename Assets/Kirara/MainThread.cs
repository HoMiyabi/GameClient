using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Kirara
{
    public class MainThread : MonoSingleton<MainThread>
    {
        private readonly int mainThreadId = Environment.CurrentManagedThreadId;
        private readonly ConcurrentQueue<Action> queue = new();

        public void Enqueue(Action action)
        {
            if (action == null) return;

            if (Environment.CurrentManagedThreadId == mainThreadId)
            {
                action();
            }
            else
            {
                queue.Enqueue(action);
            }
        }

        private void Update()
        {
            while (queue.TryDequeue(out var action))
            {
                action();
            }
        }

        private void LateUpdate()
        {
            while (queue.TryDequeue(out var action))
            {
                action();
            }
        }
    }
}