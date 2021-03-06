using Algodat.SortAlgorithms;
using NUnit.Framework;
using System;

namespace Algodat.Test
{
    [TestFixture(typeof(HeapSort))]
    [TestFixture(typeof(InsertionSort))]
    [TestFixture(typeof(BubbleSort))]
    [TestFixture(typeof(MergeSort))]
    [TestFixture(typeof(QuickSort))]
    [TestFixture(typeof(RadixSort))]
    public class SortAlgorithmTest<T> where T : ISortAlgorithm, new()
    {
        [DatapointSource]
        private int[][] _arrays = new[]
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
            new[] { int.MaxValue, 1, int.MaxValue, 0 },
            new[] { int.MaxValue, 0, int.MinValue, 0, 1 },
        };

        [Theory]
        public void TestSort(int[] array)
        {
            var expected = new int[array.Length];
            array.CopyTo(expected, 0);
            Array.Sort(expected);

            var instance = new T();
            instance.SortAscending(array);

            CollectionAssert.AreEqual(expected, array);
        }
    }
}