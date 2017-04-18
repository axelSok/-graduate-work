using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DNFTester.Entities.AbstractMachine
{
    public class Vector : ObservableCollection<ItemValue>
    {
        public Vector(int inputCount, int maxState, int maxOutput, int currentState)
        {
            CurrentState = currentState;
            for (var t = 0; t < inputCount; t++)
            {
                Add(new ItemValue(0, 0, maxState, maxOutput));
            }
        }

        public Vector(ObservableCollection<ItemValue> collection, int currentState)
        {
            CurrentState = currentState;
            for (var i = 0; i < collection.Count; i++)
            {
                Add(collection[i]);
            }
        }

        public Vector(List<ItemValue> listVector, int currentState)
        {
            CurrentState = currentState;
            foreach (ItemValue t in listVector)
            {
                Add(t);
            }
        }

        public int CurrentState { get; set; }

        public bool IsCover(Vector vector)
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Output == null || vector[i].Output == null || this[i].Output == 0 || vector[i].Output == 0)
                    continue;
                if (this[i].Output == vector[i].Output) continue;
                return false;
            }
            return true;
        }

        public IList<GroupItem> GetIntersection(Vector vector)
        {
            var result = new List<GroupItem>();
            for (var i = 0; i < Count; i++)
            {
                if ((this[i].State == CurrentState && vector[i].State == vector.CurrentState) ||
                    (this[i].State == vector.CurrentState && vector[i].State == CurrentState) ||
                    (this[i].State == vector[i].State) ||
                    (this[i].State == 0 || vector[i].State == 0)) continue;
                result.Add(new GroupItem(this[i].State, vector[i].State));
            }
            if (result.Count == 0)
            {
                for (int i = 0; i < vector.Count; i++)
                {
                    result.Add(new GroupItem(0, 0));
                }
            }
            return result;
        }
    }
}