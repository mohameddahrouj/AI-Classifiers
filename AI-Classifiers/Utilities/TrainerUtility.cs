using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifiers.Model
{
    public static class TrainerUtility
    {
        public const int CrossValidationTimes = 5;

        public static List<ConfusionMatrix> TestAndTrainData(IEnumerable<Class> classes, IEnumerable<Classifier> classifiers)
        {
            List<ConfusionMatrix> matrices = new List<ConfusionMatrix>();

            for (int i = 0; i < CrossValidationTimes; i++)
            {
                TrainData(i, classes, classifiers);
                var testingData = GetTestData(i, classes);
                matrices.Add(TestData(testingData, classifiers));
            }

            return matrices;
        }

        private static ConfusionMatrix TestData(IEnumerable<double[]> trainingData, IEnumerable<Classifier> classifiers)
        {
            var confusionMatrix = new ConfusionMatrix(classifiers.Count());

            foreach (var data in trainingData)
            {
                int classification = ClassifyData(data, classifiers);
                int realClassification = (int)data[0];
                confusionMatrix.increaseElement(realClassification - 1, classification - 1);
            }

            return confusionMatrix;
        }

        private static int ClassifyData(double[] vector, IEnumerable<Classifier> classifiers)
        {
            var classId = 1;
            var maxProbability = Double.MinValue;

            foreach (var classifier in classifiers)
            {
                var probability = classifier.FindProbability(vector);

                if (probability > maxProbability)
                {
                    classId = classifier.GetId();
                    maxProbability = probability;
                }
            }

            return classId;
        }

        private static IEnumerable<double[]> GetTestData(int fold, IEnumerable<Class> classes)
        {
            var testData = new List<double[]>();

            foreach (var c in classes)
            {
                testData.AddRange(c.GetTestData(fold));
            }

            return testData;
        }

        private static void TrainData(int fold, IEnumerable<Class> classes, IEnumerable<Classifier> classifiers)
        {

            if (classifiers.First() is DecisionTreeClassifier)
            {
                TrainDecisionClassifier(fold, classes, classifiers);
            }
            else
            {
                foreach (var classifier in classifiers)
                {
                    var c = classes.First(x => x.classId == classifier.GetId());

                    if (c != null)
                        classifier.Train(c.GetTrainingData(fold));
                }
            }

        }

        private static void TrainDecisionClassifier(int fold, IEnumerable<Class> classes, IEnumerable<Classifier> classifiers)
        {
            List<double[]> trainingData = new List<double[]>();

            foreach (var cl in classes)
            {
                trainingData.AddRange(cl.GetTrainingData(fold));
            }

            foreach (var classifier in classifiers)
            {
                classifier.Train(trainingData);
            }
        }
    }
}
