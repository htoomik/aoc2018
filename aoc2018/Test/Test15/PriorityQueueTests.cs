using aoc2018.Code.Day15;
using Xunit;

namespace aoc2018.Test.Test15
{
    public class PriorityQueueTests
    {
        [Fact]
        public void GetsHighestPriorityItem()
        {
            var pq = new PriorityQueue<string>();
            pq.Enqueue("t", 1);
            pq.Enqueue("s", 0);

            var item = pq.Dequeue();
            Assert.Equal("s", item);
        }
    }
}
