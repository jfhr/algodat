using System;

namespace Algodat.SortAlgorithms
{
    // Counting sort is not tested directly because it wouldn't be 
    // practical when our values are the range of all integers
    public class CountingSort
    {
        public void SortAscending(Span<int> array, int minValue, int maxValue)
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
