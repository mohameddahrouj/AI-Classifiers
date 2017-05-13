using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifiers.Model
{
    public class ConfusionMatrix
    {
        private int[,] matrix;

        //n x n
        public ConfusionMatrix(int numberOfClasses)
        {
            this.matrix = new int[numberOfClasses, numberOfClasses];
        }

        public void increaseElement(int row, int column)
        {
            this.matrix[row, column]++;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    builder.Append(matrix[i, j].ToString() + generateOffset(matrix[i, j].ToString()));
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        public String generateOffset(String value)
        {
            if (value.Length == 3)
            {
                return "   ";
            }
            if (value.Length == 2)
            {
                return "     ";
            }
            if (value.Length == 1)
            {
                return "       ";
            }
            return " ";
        }

        public double CalculateAccuaracy()
        {
            var totalCount = 0;
            var numberOfCorrectClassifications = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (i == j)
                        numberOfCorrectClassifications += matrix[i, j];
                    totalCount += matrix[i, j];
                }
            }

            return (double) numberOfCorrectClassifications/ totalCount;
        }
    }
}
