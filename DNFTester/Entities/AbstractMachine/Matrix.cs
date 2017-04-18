using System.Collections.ObjectModel;

namespace DNFTester.Entities.AbstractMachine
{
    public class Matrix : ObservableCollection<Vector>
    {
        public Matrix(int maxState, int inputCount, int maxOutput)
        {
            for (var i = 0; i < maxState; i++)
            {
                Add(new Vector(inputCount, maxState, maxOutput, i+1));
            }
        }

        public Matrix() : this(0, 0, 0) { }
    }
}