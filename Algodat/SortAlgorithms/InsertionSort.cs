namespace Algodat.SortAlgorithms
{
    public class InsertionSort : ISortAlgorithm
    {
        public void SortAscending(int[] array)
        {
            CreateInstance(array).SortAscending();
        }

        private Instance CreateInstance(int[] array) => new(array);

        private class Instance
        {
            private readonly int[] array;

            public Instance(int[] array)
            {
                this.array = array;
            }

            public void SortAscending()
            {
                for (int i = 1; i < array.Length; i++)
                {
                    for (int j = i; j > 0; j--)
                    {
                        if (array[j] < array[j - 1])
                        {
                            Swap(j, j - 1);
                        }
                    }
                }
            }

            private void Swap(int a, int b)
            {
                int temp = array[a];
                array[a] = array[b];
                array[b] = temp;
            }
        }
    }
}
