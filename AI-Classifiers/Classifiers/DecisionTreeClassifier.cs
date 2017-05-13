using System.Collections.Generic;

namespace Classifiers.Model
{
    public class DecisionTreeClassifier : Classifier
    {
        private int id;
        private int numClasses;
        private DecisionTree decisionTree;

        public DecisionTreeClassifier(int id, int numClasses)
        {
            this.id = id;
            this.numClasses = numClasses;
        }

        public override double FindProbability(double[] vector)
        {
            var root = decisionTree.GetRootNode();

            while(root != null && root.right != null && root.left != null)
            {
                var value = vector[root.Id];

                if (value == 0)
                    root = root.left;
                else
                    root = root.right;
            }

            return root.Id == this.id ? 1 : 0;
        }

        public override void Train(List<double[]> vectors)
        {
            this.decisionTree = new DecisionTree(vectors, this.numClasses);
        }

        public override int GetId()
        {
            return id;
        }
    }
}
