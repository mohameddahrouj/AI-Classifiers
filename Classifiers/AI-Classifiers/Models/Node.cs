using System;

namespace Classifiers.Model
{
    public class Node
    {
        public int Id { get; private set; }
        public String Name { get; private set; }

        public double probabilityGivenParentZero;
        public double probabilityGivenParentOne;

        public Node left;
        public Node right;

        public Node(int id)
        {
            Id = id;
            this.Name = $"F: {this.Id}";
        }

        public Node(int id, String name)
        {
            Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public double GetProbability(double value, double parentValue)
        {
            if(parentValue == 0)
            {
                return value == 0 ? probabilityGivenParentZero : 1 - probabilityGivenParentZero;
            }           
            else
            {
                return value == 0 ? probabilityGivenParentOne : 1 - probabilityGivenParentOne;
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
