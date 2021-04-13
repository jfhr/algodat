using Algodat.Stacks;
using NUnit.Framework;

namespace Algodat.Test
{
    [TestFixture(typeof(ArrayStack<int>))]
    [TestFixture(typeof(LinkedStack<int>))]
    public class StackTest<T> where T : IStack<int>, new()
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
                instance.Push(elements[i]);
                Assert.AreEqual(elements[i], instance.Peek());
            }

            for (int i = elements.Length - 1; i >= 0; i--)
            {
                Assert.AreEqual(elements[i], instance.Peek());
                Assert.AreEqual(elements[i], instance.Pop());
            }
        }


        [Test]
        public void TestComplexSequence()
        {
            var instance = new T();

            instance.Push(100);
            Assert.AreEqual(100, instance.Peek());

            instance.Push(200);
            Assert.AreEqual(200, instance.Peek());

            Assert.AreEqual(200, instance.Pop());

            Assert.AreEqual(100, instance.Peek());

            instance.Push(300);
            Assert.AreEqual(300, instance.Peek());

            instance.Push(400);
            Assert.AreEqual(400, instance.Peek());

            Assert.AreEqual(400, instance.Pop());

            Assert.AreEqual(300, instance.Pop());

            Assert.AreEqual(100, instance.Pop());
        }
    }
}