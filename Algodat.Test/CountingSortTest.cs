using Algodat.SortAlgorithms;
using NUnit.Framework;

namespace Algodat.Test
{
    public class CountingSortTest
    {
        [Test]
        public void CountingSort100()
        {
            var input = new[] {51, 97, 66, 47, 47, 76, 83, 5};
            var expected = new[] {5, 47, 47, 51, 66, 76, 83, 97};
            
            CountingSort.SortAscending(input, 0, 100);
            
            CollectionAssert.AreEqual(expected, input);
        }
    }
}