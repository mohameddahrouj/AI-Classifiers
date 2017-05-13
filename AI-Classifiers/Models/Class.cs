using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifiers.Model
{
    public class Class
    {
        private List<double[]> features;
        public int classId;

        public Class(int classId)
        {
            this.features = new List<double[]>();
            this.classId = classId;
        }

        public List<double[]> GetFeatures()
        {
            return this.features;
        }

        public void AddFeatures(double[] feature)
        {            
            this.features.Add(feature);
        }

        public List<double[]> GetTestData(int fold)
        {
            int count = this.features.Count / TrainerUtility.CrossValidationTimes;
            var startIndex = count * fold;

            if (fold == TrainerUtility.CrossValidationTimes - 1)
                count = this.features.Count - startIndex;

            return this.features.GetRange(startIndex, count);
        }

        public List<double[]> GetTrainingData(int fold)
        {
            var trainData = new List<double[]>();
            int count = this.features.Count / TrainerUtility.CrossValidationTimes;
            var startIndex = fold *  count;

            if (fold == TrainerUtility.CrossValidationTimes - 1)
                count = this.features.Count - startIndex;

            var endIndex = startIndex + count;

            for(int i = 0; i < this.features.Count; i++)
            {
                if(i >= startIndex && i< endIndex)
                {
                    continue;
                }

                trainData.Add(this.features[i]);
            }

            return trainData;
        }
    }
}
