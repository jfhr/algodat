namespace Algodat.SortAlgorithms
{
    public class BubbleSort : ISortAlgorithm
    {
        public void SortAscending(int[] array)
        {
            if (array.Length <= 1)
            {
                return;
            }

            for (int end = array.Length; end > 0; end--)
            {
                for (int i = 1; i < end; i++)
                {
                    if (array[i - 1] > array[i])
                    {
                        ArrayUtil.Swap(array, i, i - 1);
                    }
                }
            }
        }
    }
}
