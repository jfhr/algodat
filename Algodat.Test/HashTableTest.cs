using Algodat.HashTables;
using Algodat.Trees;
using NUnit.Framework;

namespace Algodat.Test
{
    [TestFixture(typeof(LinkedHashTable<int, string>))]
    [TestFixture(typeof(OpenAddressingWithDoubleHashingHashTable<string>))]
    [TestFixture(typeof(OpenAddressingWithLinearProbingHashTable<int, string>))]
    [TestFixture(typeof(OpenAddressingWithQuadraticProbingHashTable<int, string>))]

    [TestFixture(typeof(BinarySearchTree<int, string>))]
    [TestFixture(typeof(IterativeBinarySearchTree<int, string>))]
    [TestFixture(typeof(RedBlackTree<int, string>))]
    public class HashTableTest<T> where T : IHashTable<int, string>, new()
    {
        [DatapointSource]
        private int[][] _arrays = new[]
        {
            new[] { 100 },
            new[] { 100, 200, 300 },
            new[] { 300, 200, 300, 200 },
            new[] { 300, 200, 100, 150, 250, 350, 125, 225 }
        };

        private void AssertSearchResult(IHashTable<int, string> instance, int key, string expectedValue)
        {
            if (expectedValue == null)
            {
                Assert.IsNull(instance.Search(key));
            }
            else
            {
                Assert.AreEqual(expectedValue, instance.Search(key));
            }
        }

        [Theory]
        public void TestElementSequence(int[] elements)
        {
            var instance = new T();

            foreach (var i in elements)
            {
                instance.Insert(i, $"{i}");
                AssertSearchResult(instance, i, $"{i}");
            }

            foreach (var i in elements)
            {
                instance.Remove(i);
                AssertSearchResult(instance, i, null);
            }
        }

        [Test]
        public void TestMultipleOverrides()
        {
            var instance = new T();

            foreach (var s in new[] { "a", "b", "c" })
            {
                instance.Insert(100, s);
                AssertSearchResult(instance, 100, s);
            }

            instance.Remove(100);
            AssertSearchResult(instance, 100, null);
        }

        [Test]
        public void TestComplexSequence()
        {
            var instance = new T();

            instance.Insert(100, "one-hundred");
            AssertSearchResult(instance, 100, "one-hundred");

            instance.Insert(200, "two-hundred");
            AssertSearchResult(instance, 200, "two-hundred");

            instance.Insert(100, "hundred");
            AssertSearchResult(instance, 100, "hundred");

            instance.Remove(100);
            AssertSearchResult(instance, 100, null);

            for (int i = 0; i < 400; i++)
            {
                instance.Insert(i, $"{i}-value");
                AssertSearchResult(instance, i, $"{i}-value");
            }
        }
    }
}