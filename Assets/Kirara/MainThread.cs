using System;
using System.Collections.Concurrent;

namespace Kirara
{
    public class MainThread : MonoSingleton<MainThread>
    {
        private readonly ConcurrentQueue<Action> queue = new();

        public void Enqueue(Action action)
        {
            queue.Enqueue(action);
        }

        private void Update()
        {
            while (queue.TryDequeue(out Action action))
            {
                action();
            }
        }
    }
}