using System.Collections.Generic;

namespace Algodat.Graphs
{
    public interface IWeightedDirectedGraph
    {
        int Size { get; }
        IEnumerable<(int From, int To, double Weight)> Edges { get; }

        void Initialize(int size);
        IEnumerable<(int To, double Weight)> GetNeighbors(int from);
        double GetEdge(int from, int to);
        void SetEdge(int from, int to, double weight);
    }
}