using System;

namespace Algodat.SortAlgorithms
{
    /// <summary>
    /// Counting sort doesn't use the normal test cases bc it
    /// simply isn't practical to sort the entire range of
    /// integers. Instead, it gets its own little test limited
    /// to values between 0 and 100.
    /// </summary>
    public static class CountingSort
    {
        public static void SortAscending(Span<int> array, int minValue, int maxValue)
        {
            var count = new int[maxValue - minValue];
            foreach (var s in array)
            {
                int index = s - minValue;
                count[index]++;
            }

            int destIndex = 0;
            for (int i = 0; i < count.Length; i++)
            {
                int value = i + minValue;
                for (int j = 0; j < count[i]; j++)
                {
                    array[destIndex] = value;
                    destIndex++;
                }
            }
        }
    }
}
