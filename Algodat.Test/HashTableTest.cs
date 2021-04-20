using Algodat.HashTables;
using NUnit.Framework;

namespace Algodat.Test
{
    public class HashTableTest<T> where T : IHashTable<int, string>, new()
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
                instance.Insert(i, $"{i}");
                Assert.AreEqual($"{i}", instance.Search(i));
            }

            for (int i = 0; i < elements.Length; i++)
            {
                instance.Remove(i);
                Assert.IsNull(instance.Search(i));
            }
        }

        [Test]
        public void TestComplexSequence()
        {
            var instance = new T();

            instance.Insert(100, "one-hundred");
            Assert.AreEqual("one-hundred", instance.Search(100));

            instance.Insert(200, "two-hundred");
            Assert.AreEqual("two-hundred", instance.Search(200));

            instance.Insert(100, "hunded");
            Assert.AreEqual("hundred", instance.Search(100));

            instance.Remove(100);
            Assert.IsNull(instance.Search(100));

            for (int i = 0; i < 400; i++)
            {
                instance.Insert(i, $"{i}-value");
            }

            for (int i = 0; i < 400; i++)
            {
                Assert.AreEqual($"{i}-value", instance.Search(i));
            }
        }
    }
}