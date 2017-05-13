using Classifiers.Utilities;
using GraphSharp;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph.Algorithms;
using System.Text;
using System.Threading.Tasks;

namespace Classifiers.Model
{
    public class DecisionTree
    {
        private const int MaxDepth = 5;

        public IBidirectionalGraph<Node, IEdge<Node>> GraphToVisualize
        {
            get;
            private set;
        }

        public DecisionTree(IEnumerable<double[]> vectors, int numberOfClasses)
        {
            var root = CreateTree(vectors, numberOfClasses);
            this.GraphToVisualize = CreateGraph(root);
        }

        private IBidirectionalGraph<Node, IEdge<Node>> CreateGraph(Node root)
        {
            var graph = new BidirectionalGraph<Node, IEdge<Node>>();
            var nodes = new Queue<Node>();
            graph.AddVertex(root);
            nodes.Enqueue(root);

            while(nodes.Count>0)
            {
                var node = nodes.Dequeue();

                if (node.left != null)
                {
                    var edge = new WeightedEdge<Node>(node, node.left, 0);

                    graph.AddVertex(node.left);
                    graph.AddEdge(edge);
                    nodes.Enqueue(node.left);
                }

                if (node.right != null)
                {
                    var edge = new WeightedEdge<Node>(node, node.right, 1);

                    graph.AddVertex(node.right);
                    graph.AddEdge(edge);
                    nodes.Enqueue(node.right);
                }

            }

            return graph;
        }

        public Node CreateTree(IEnumerable<double[]> features, int numberOfClasses)
        {
            var entropy = CalculateTotalEntropy(features, numberOfClasses);

            return CreateTree(features, numberOfClasses, entropy, 0);
        }

        public Node CreateTree(IEnumerable<double[]> vectors, int numberOfClasses, double entropy, int depth)
        {
            if(entropy == 0 || depth >= MaxDepth)
            {
                var id = GetMostFrequentClassId(vectors, numberOfClasses);
                return new Node(id, $"C: {id}");
            }

            var column = FindLargestInfoGain(vectors,numberOfClasses, entropy);
            var columnEqualToZero = GetVectorsWithColumnEqual(vectors, column, 0);
            var columnEqualToOne = GetVectorsWithColumnEqual(vectors, column, 1);

            var node = new Node(column);
            var newDepth = depth + 1;

            node.left = CreateTree(columnEqualToZero, numberOfClasses, CalculateTotalEntropy(columnEqualToZero, numberOfClasses), newDepth);
            node.right = CreateTree(columnEqualToOne, numberOfClasses, CalculateTotalEntropy(columnEqualToOne, numberOfClasses), newDepth);

            return node;
        }

        public Node GetRootNode()
        {
            return this.GraphToVisualize.Vertices.Where(node => GraphToVisualize.InDegree(node) == 0).First();
        }

        private int GetMostFrequentClassId(IEnumerable<double[]> vectors, int numberOfClasses)
        {
            var max = Int32.MinValue;
            var classId = -1;

            for(int i = 1; i<= numberOfClasses; i++)
            {
                int countOfClass = vectors.Where(vector => vector[0] == i).Count();

                if(countOfClass > max)
                {
                    max = countOfClass;
                    classId = i;
                }
            }

            return classId;
        }

        private IEnumerable<double[]> GetVectorsWithColumnEqual(IEnumerable<double[]> vectors,int column, int value)
        {
            return vectors.Where(vector => vector[column] == value);
        }

        private int FindLargestInfoGain(IEnumerable<double[]> vectors, int numberOfClasses, double entropy)
        {
            double maxInformationGain = Double.MinValue;
            int column = 1;
            var columns = vectors.Any() ? vectors.First()?.Count(): 0;
            

            for (int i = 1; i < columns; i++)
            {
                var informationGain = CalculateInformationGain(vectors, numberOfClasses, i);

                if(informationGain > maxInformationGain)
                {
                    maxInformationGain = informationGain;
                    column = i;
                }
            }

            return column;
        }

        private double CalculateInformationGain(IEnumerable<double[]> vectors, int numberOfClasses, int column)
        {
            double gain = 0;

            for(int i = 1; i<= numberOfClasses; i++)
            {
                //False values
                gain += CalculateEntropy(vectors, i, column, 0);
                //True values
                gain += CalculateEntropy(vectors, i, column, 1);
            }

            return gain;
        }

        private double CalculateEntropy(IEnumerable<double[]> vectors, int cl, int column, int value)
        {
            int numberOfTimesEqual = vectors.Where(feature => feature[0] == cl && feature[column] == value).Count();
            int numberOfTimesNotEqual = vectors.Count() - numberOfTimesEqual;
            var proportion = (double)numberOfTimesEqual / vectors.Count();
            var proportion2 = (double)numberOfTimesNotEqual / vectors.Count();

            if (proportion == 0 && proportion2 == 0)
                return 0;
            else if (proportion == 0)
                return (-proportion2 * Math.Log(proportion2, 2));
            else if (proportion2 == 0)
                return (-proportion * Math.Log(proportion, 2));
            else
                return (-proportion * Math.Log(proportion, 2)) + (-proportion2 * Math.Log(proportion2, 2)); ;

        }

        private double CalculateTotalEntropy(IEnumerable<double[]> vectors, int numberOfClasses)
        {
            double entropy = 0;

            for(int i= 1; i<= numberOfClasses; i++)
            {
                int numberOfClass = vectors.Where(vector => vector[0] == i).Count();
                var proportion = (double)numberOfClass / vectors.Count();

                if (proportion != 0)
                    entropy+= (-proportion * Math.Log(proportion, 2));
            }

            return entropy;
        }
    }
}
