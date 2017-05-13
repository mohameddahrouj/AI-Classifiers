using Classifiers.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Classifiers.Model
{
    public class NaiveBayesClassifier: Classifier
    {
        private int id;
        private List<double> featuresProbabilityIfZero;

        public NaiveBayesClassifier(int id)
        {
            this.id = id;
            this.featuresProbabilityIfZero = new List<double>();
        }

        public override void Train(List<double[]> vectors)
        {
            foreach(var column in VecotorUtility.GetColumns(vectors))
            {
                int numberOfZeros = column.Where(num => num == 0).Count();
                double probability = (double) numberOfZeros / column.Count;
                this.featuresProbabilityIfZero.Add(probability);
            }
        }

        public override int GetId()
        {
            return this.id;
        }

        public override double FindProbability(double[] vector)
        {
            double probability = 1;

            for(var i = 1; i < vector.Length; i++)
            {
                if(vector[i] == 0)
                {
                    probability *= this.featuresProbabilityIfZero[i -1];
                }
                else
                {
                    probability *= (1 - this.featuresProbabilityIfZero[i -1]);
                }
            }

            return probability;
        }
    }
}
