namespace Algodat.SortAlgorithms
{
    public class InsertionSort : ISortAlgorithm
    {
        public void SortAscending(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (array[j] < array[j - 1])
                    {
                        ArrayUtil.Swap(array, j, j - 1);
                    }
                }
            }
        }
    }
}
