using System.Collections.Generic;

namespace Algodat.Graphs
{
    public class AdjacencyMatrixGraph : IWeightedDirectedGraph
    {
        private double[,] _adjMatrix;
        
        public int Size { get; private set; }

        public IEnumerable<(int From, int To, double Weight)> Edges
        {
            get
            {
                for (int from = 0; from < Size; from++)
                {
                    foreach ((int to, double weight) in GetNeighbors(from))
                    {
                        yield return (from, to, weight);
                    }
                }
            }
        }

        public void Initialize(int size)
        {
            Size = size;
            _adjMatrix = new double[size, size];
            for (int from = 0; from < Size; from++)
            {
                for (int to = 0; to < Size; to++)
                {
                    _adjMatrix[from, to] = double.PositiveInfinity;
                }

                _adjMatrix[from, from] = 0;
            }
        }

        public IEnumerable<(int To, double Weight)> GetNeighbors(int from)
        {
            for (int to = 0; to < Size; to++)
            {
                if (!double.IsPositiveInfinity(_adjMatrix[from, to]))
                {
                    yield return (to, _adjMatrix[from, to]);
                }
            }
        }

        public double GetEdge(int from, int to)
        {
            return _adjMatrix[from, to];
        }

        public void SetEdge(int from, int to, double weight)
        {
            _adjMatrix[from, to] = weight;
        }
    }
}