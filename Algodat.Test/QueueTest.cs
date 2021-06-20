using Algodat.Queues;
using NUnit.Framework;

namespace Algodat.Test
{
    [TestFixture(typeof(ArrayQueue<int>))]
    [TestFixture(typeof(LinkedQueue<int>))]
    public class QueueTest<T> where T : IQueue<int>, new()
    {
        [DatapointSource]
        private int[][] _arrays = new[]
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

        [Test]
        public void TestComplexSequence()
        {
            var instance = new T();

            instance.Enqueue(100);
            Assert.AreEqual(100, instance.Peek());

            instance.Enqueue(200);
            Assert.AreEqual(100, instance.Peek());

            Assert.AreEqual(100, instance.Dequeue());

            Assert.AreEqual(200, instance.Peek());

            instance.Enqueue(300);
            Assert.AreEqual(200, instance.Peek());

            instance.Enqueue(400);
            Assert.AreEqual(200, instance.Peek());
            
            Assert.AreEqual(200, instance.Dequeue());

            Assert.AreEqual(300, instance.Dequeue());

            Assert.AreEqual(400, instance.Dequeue());
        }
    }
}