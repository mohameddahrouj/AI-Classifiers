using Classifiers.Utilities;
using GraphSharp;
using QuickGraph;
using QuickGraph.Algorithms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Classifiers.Model
{
    public class DependenceTree 
    {
        public IBidirectionalGraph<Node, IEdge<Node>> GraphToVisualize
        {
            get;
            private set;
        }

        public DependenceTree(List<double[]> vectors)
        {
            var graph = this.CreateGraph(vectors);
            this.GraphToVisualize = this.GetMaximumSpanningTree(graph);
        }

        private IBidirectionalGraph<Node, IEdge<Node>> GetMaximumSpanningTree(IUndirectedGraph<Node, IEdge<Node>> graph)
        {
            var newGraph = new BidirectionalGraph<Node, IEdge<Node>>();
            var visistedNodes = new HashSet<Node>();
            var edges = graph.Edges.OrderByDescending(edge => ((WeightedEdge<Node>)edge).Weight);
 
            foreach (WeightedEdge<Node> edge in edges)
            {
                if (!visistedNodes.Contains(edge.Target) && !visistedNodes.Contains(edge.Source))
                {
                    newGraph.AddVertex(edge.Target);
                    newGraph.AddVertex(edge.Source);
                    newGraph.AddEdge(edge);
                    visistedNodes.Add(edge.Target);
                    visistedNodes.Add(edge.Target);
                }
            }

            return newGraph;
        }

        public IUndirectedGraph<Node, IEdge<Node>> CreateGraph(List<double[]> vectors)
        {
            var graph = new UndirectedGraph<Node, IEdge<Node>>();
            int columnLength = vectors[0].Length;
            Node[] vertices = new Node[columnLength];

            for (int i = 1; i < vertices.Length; i++)
            {
                vertices[i] = new Node(i);
                graph.AddVertex(vertices[i]);
            }

            for (int i = 1; i < vertices.Length; i++)
            {
                for (int j = i+1; j < vertices.Length; j++)
                {
                    if (i != j)
                        graph.AddEdge(GetWeightedEdge(vertices[i], vertices[j], vectors));
                }
            }

            return graph;
        }

        private WeightedEdge<Node> GetWeightedEdge(Node source, Node target, List<double[]> vectors)
        {
            double weight = 0;
            double rows = vectors.Count;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var targetId = target.Id;
                    var sourceId = source.Id;
                    double probIJ = VecotorUtility.CountNumberOfTimesColumnsEquals(vectors, sourceId, targetId, i, j) / rows;
                    double probI = VecotorUtility.CountNumberOfTimesColumnEquals(vectors, sourceId, i) / rows;
                    double probJ = VecotorUtility.CountNumberOfTimesColumnEquals(vectors, targetId, j) / rows;
                    weight += probIJ * Math.Log(probIJ / (probI * probJ), 2);
                }
            }

            return new WeightedEdge<Node>(source, target, weight);
        }
    }
}
