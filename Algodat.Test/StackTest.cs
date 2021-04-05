using Algodat.Stacks;
using NUnit.Framework;

namespace Algodat.Test
{
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
    }
}