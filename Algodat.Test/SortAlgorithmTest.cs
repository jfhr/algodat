using Algodat.SortAlgorithms;
using NUnit.Framework;
using System;

namespace Algodat.Test
{
    [TestFixture(typeof(HeapSort))]
    [TestFixture(typeof(InsertionSort))]
    [TestFixture(typeof(BubbleSort))]
    [TestFixture(typeof(MergeSort))]
    public class SortAlgorithmTest<T> where T : ISortAlgorithm, new()
    {
        [DatapointSource]
        public int[][] Arrays = new[]
        {
            Array.Empty<int>(),
            new[] { 1 },
            new[] { 1, 2 },
            new[] { 2, 1 },
            new[] { 1, 2, 2, 3 },
            new[] { 1, 2, 3, 2 },
            new[] { 1, 1, 1 },
            new[] { 1, -1, 0 },
            new[] { 3, 2, 4, 1, 5 },
        };

        [Theory]
        public void TestSort(int[] array)
        {
            var instance = new T();
            instance.SortAscending(array);

            Assert.That(array, Is.Ordered.Ascending);
        }
    }
}