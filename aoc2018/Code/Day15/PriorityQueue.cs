using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code.Day15
{
    public class PriorityQueue<T>
    {
        private readonly Dictionary<int, Queue<T>> _queues = new Dictionary<int, Queue<T>>();

        public bool IsEmpty()
        {
            return !_queues.Any();
        }

        public void Enqueue(T item, int priority)
        {
            if (!_queues.ContainsKey(priority))
            {
                _queues.Add(priority, new Queue<T>());
            }
            _queues[priority].Enqueue(item);
        }

        public T Dequeue()
        {
            var highestPriority = _queues.Keys.Min();
            var queue = _queues[highestPriority];
            var returnValue = queue.Dequeue();
            if (queue.Count == 0)
                _queues.Remove(highestPriority);
            return returnValue;
        }
    }
}
