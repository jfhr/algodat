using Algodat.Queues;
using NUnit.Framework;

namespace Algodat.Test
{
    [TestFixture(typeof(ArrayQueue<int>))]
    public class QueueTest<T> where T : IQueue<int>, new()
    {
        [DatapointSource]
        public int[][] Arrays = new[]
        {
            new[] { 100 },
            new[] { 100, 200, 300 },
            new[] { 300, 200, 300, 200 }
        };

        [Theory]
        public void TestElementSequence(int[] elements)
        {
            var instance = new T();

            for (int i = 0; i < elements.Length; i++)
            {
                instance.Enqueue(elements[i]);
                Assert.AreEqual(elements[0], instance.Peek());
            }

            for (int i = 0; i < elements.Length; i++)
            {
                Assert.AreEqual(elements[i], instance.Peek());
                Assert.AreEqual(elements[i], instance.Dequeue());
            }
        }
    }
}