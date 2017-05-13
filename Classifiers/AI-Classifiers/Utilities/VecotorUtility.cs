using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifiers.Utilities
{
    public static class VecotorUtility
    {
        public static void ConvertListToBinary(List<double[]> vectors)
        {
            int columnLength = vectors[0].Length;
            var averages = CalculateAverageOfEachColumn(GetColumns(vectors));

            for (int i = 1; i < columnLength; i++)
            {
                double threshold = averages[i - 1];
                threshold -= averages[i - 1] * 0.1124443434;

                for (int j = 0; j < vectors.Count; j++)
                {
                    if (vectors[j][i] > threshold)
                        vectors[j][i] = 1;
                    else
                        vectors[j][i] = 0;
                }
            }
        }

        public static List<double> CalculateAverageOfEachColumn(List<List<double>> vectors)
        {
            var averages = new List<double>();

            foreach (var value in vectors)
            {
                averages.Add(value.Average());
            }

            return averages;
        }

        public static List<List<double>> GetColumns(List<double[]> vectors)
        {
            var columns = new List<List<double>>();
            var columnLength = vectors[0].Length;

            for (int i = 1; i < columnLength; i++)
            {
                var list = new List<double>();

                for (int j = 0; j < vectors.Count; j++)
                {
                    list.Add(vectors[j][i]);
                }

                columns.Add(list);
            }

            return columns;
        }

        public static int CountNumberOfTimesColumnEquals(List<double[]> vectors, int column, double value)
        {
            return vectors.Where(vector => vector[column] == value).Count();
        }

        public static int CountNumberOfTimesColumnsEquals(List<double[]> vectors, int columnOne, int columnTwo, double valueOne, double valueTwo)
        {
            return vectors.Where(vector => vector[columnOne] == valueOne && vector[columnTwo] == valueTwo).Count();
        }
    }
}
