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
    public class HashTableTest<T> where T : IHashTable<int, string>, new()
    {
        [DatapointSource]
        public int[][] Arrays = new[]
        {
            new[] { 100 },
            new[] { 100, 200, 300 },
            new[] { 300, 200, 300, 200 }
        };

        private void AssertSearchResult(IHashTable<int, string> instance, int key, string expectedValue)
        {
            if (expectedValue == null)
            {
                Assert.IsFalse(instance.Search(key, out var value));
            }
            else
            {
                Assert.IsTrue(instance.Search(key, out var value));
                Assert.AreEqual(expectedValue, value);
            }
        }

        [Theory]
        public void TestElementSequence(int[] elements)
        {
            var instance = new T();

            for (int i = 0; i < elements.Length; i++)
            {
                instance.Insert(i, $"{i}");
                AssertSearchResult(instance, i, $"{i}");
            }

            for (int i = 0; i < elements.Length; i++)
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
            }

            for (int i = 0; i < 400; i++)
            {
                AssertSearchResult(instance, i, $"{i}-value");
            }
        }
    }
}