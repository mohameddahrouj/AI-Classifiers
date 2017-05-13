using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifiers.Utilities
{
    public class FileUtility
    {
        public static List<double[]> ConvertCSVToList(String filePath)
        {
            var result = new List<double[]>();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] lines = reader.ReadLine().Split(',');
                        double[] numbers = Array.ConvertAll(lines, double.Parse);
                        result.Add(numbers);
                    }
                }
            }
            catch (FileNotFoundException exception)
            {
                throw exception;
            }

            return result;
        }
    }
}
