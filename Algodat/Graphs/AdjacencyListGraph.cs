using System.Collections.Generic;

namespace Algodat.Graphs
{
    public class AdjacencyListGraph : IWeightedDirectedGraph
    {
        private List<(int To, double Weight)>[] _adjList;

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
            _adjList = new List<(int To, double Weight)>[size];
            for (int i = 0; i < Size; i++)
            {
                _adjList[i] = new List<(int To, double Weight)>();
            }
        }

        public IEnumerable<(int To, double Weight)> GetNeighbors(int from)
        {
            return _adjList[from];
        }

        public double GetEdge(int from, int to)
        {
            foreach (var entry in _adjList[from])
            {
                if (entry.To == to)
                {
                    return entry.Weight;
                }
            }

            return double.PositiveInfinity;
        }

        public void SetEdge(int from, int to, double weight)
        {
            for (int i = 0; i < _adjList[from].Count; i++)
            {
                var entry = _adjList[from][i];
                if (entry.To == to)
                {
                    entry.Weight = weight;
                    return;
                }
            }

            _adjList[from].Add((to, weight));
        }
    }
}