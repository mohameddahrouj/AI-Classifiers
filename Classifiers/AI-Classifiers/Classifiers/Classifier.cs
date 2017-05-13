using System.Collections.Generic;

namespace Classifiers.Model
{
    public abstract class Classifier
    {
        private int id;

        public abstract void Train(List<double[]> vectors);
        public abstract double FindProbability(double[] vector);
        public abstract int GetId();
    }
}
