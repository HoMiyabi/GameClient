using System;
using System.Collections.Concurrent;

namespace Kirara
{
    public class MainThread : MonoSingleton<MainThread>
    {
        private readonly int mainThreadId = Environment.CurrentManagedThreadId;
        private readonly ConcurrentQueue<Action> queue = new();

        public void Enqueue(Action action)
        {
            if (action == null) return;

            if (mainThreadId == Environment.CurrentManagedThreadId)
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