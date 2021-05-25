using Algodat.Trees;
using NUnit.Framework;
using System.Linq;

namespace Algodat.Test
{
    // Insert, Search, Remove are tested in HashTableTest already.
    // Here we only need to test Minimum and Maximum.
    [TestFixture(typeof(BinarySearchTree<int, string>))]
    [TestFixture(typeof(IterativeBinarySearchTree<int, string>))]
    [TestFixture(typeof(RedBlackTree<int, string>))]
    public class TreeTest<T> where T : ITree<int, string>, new()
    {
        [DatapointSource]
        public int[][] Arrays = new[]
        {
            new[] { 100 },
            new[] { 100, 200, 300 },
            new[] { 300, 200, 300, 200 },
            new[] { 300, 200, int.MinValue },
            new[] { 300, 200, int.MinValue, int.MaxValue, 200, int.MinValue },
            new[] { 300, 200, int.MinValue, int.MinValue + 1 },
        };

        [Theory]
        public void TestMinMax(int[] values)
        {
            int expectedMin = values.Min();
            int expectedMax = values.Max();

            var instance = new T();
            foreach (int value in values)
            {
                instance.Insert(value, value.ToString());
            }

            Assert.AreEqual(expectedMin, instance.Minimum()?.Key);
            Assert.AreEqual(expectedMin.ToString(), instance.Minimum()?.Value);

            Assert.AreEqual(expectedMax, instance.Maximum()?.Key);
            Assert.AreEqual(expectedMax.ToString(), instance.Maximum()?.Value);
        }
    }
}
