using System.Linq;
using Algodat.Graphs;
using NUnit.Framework;

namespace Algodat.Test
{
    [TestFixture(typeof(AdjacencyListGraph))]
    [TestFixture(typeof(AdjacencyMatrixGraph))]
    public class GraphTest<T> where T : IWeightedDirectedGraph, new()
    {
        [Test]
        public void TestSingleNodeGraph()
        {
            var graph = new T();
            graph.Initialize(1);
            
            Assert.AreEqual(1, graph.Size);
            Assert.AreEqual(0, graph.Edges.Count());
            
            graph.SetEdge(0, 0, 1.5);
            Assert.AreEqual(1, graph.Edges.Count());
            Assert.AreEqual(1.5, graph.GetEdge(0, 0));
        }
    }
}