using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DNFTester.Entities
{
    public class BoolVector : ObservableCollection<MatrixValue>
    {
        public BoolVector() { }

        public BoolVector(BoolVector vector)
        {
            foreach (MatrixValue mv in vector)
                this.Add(mv);
        }

        public BoolVector(int length)
        {
            for (var j = 0; j < length; j++)
                this.Add(new MatrixValue("-"));
        }

        /// <summary>
        /// Обобщенное склеивание
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public BoolVector GenericBonding(BoolVector vector)
        {
            if (!IsAdjacent(vector) || this.Count != vector.Count) return null;

            var newVector = new BoolVector(this);
            for (var i = 0; i < this.Count; i++)
            {
               if(this[i].Value == vector[i].GetInvers())
                   newVector[i] = new MatrixValue("-");
               else if(vector[i].Value != "-")
                   newVector[i] = new MatrixValue(vector[i].Value);
            }
            return newVector;//.Any(item => item.Value != "-")? newVector : null;
        }

        /// <summary>
        /// Определяение смежности векторов
        /// </summary>
        public bool IsAdjacent(BoolVector vect2)
        {
            var intersectionCount = 0;
            for (var i = 0; i < this.Count(); i++)
            {
                if (this[i].Value == "-" || vect2[i].Value == "-") continue;
                if (this[i].GetInvers() == vect2[i].Value) intersectionCount++;
            }
            return intersectionCount == 1;
        }

        /// <summary>
        /// Определяение перпендикулярности векторов
        /// </summary>
        public bool IsOrtogonal(BoolVector vect2)
        {
            for (var i = 0; i < this.Count(); i++)
                if (this[i].Value != "-" && vect2[i].Value != "-" && this[i].GetInvers() == vect2[i].Value) 
                    return true;
            return false;
        }

        /// <summary>
        /// Поглащается ли вектор
        /// </summary>
        public bool IsAbsorpt(BoolVector vector)
        {
            for (var i = 0; i < this.Count(); i++)
            {
                if (this[i].Value == "-" || this[i].Value == vector[i].Value) continue;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Склеивание двух векторов
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public BoolVector Gluing(BoolVector vector)
        {
            if (this.Count != vector.Count) return null;

            var differentPositionsCount = 0;
            var position = 0;
            for (var i = 0; i < this.Count(); i++)
            {
                if (this[i].Value == vector[i].Value) continue;

                if (this[i].GetInvers() == vector[i].Value && differentPositionsCount == 0)
                {
                    differentPositionsCount++;
                    position = i;
                }
                else return null;
            }
            if (position <= -1 || differentPositionsCount >= 2) return null;
            var newVector = new BoolVector(this);
            newVector[position] = new MatrixValue("-");
            return newVector;
        }

        public bool Equals(BoolVector vector)
        {
            for (var i = 0; i < Count; i++)
            {
                if (!this[i].Equals(vector[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Разность векторов
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public ObservableCollection<BoolVector> Substraction(BoolVector vector)
        {
            if (Count != vector.Count || this.Equals(vector)) return null;


            var result = new ObservableCollection<BoolVector>();
            
            if (IsOrtogonal(vector))
            {
                result.Add(new BoolVector(this));
                return result;
            }

            for (var i = 0; i < this.Count; i++)
            {
                if (this[i].Value == "-" && vector[i].Value != "-")
                {
                    var newVector = new BoolVector(this);
                    newVector[i] = new MatrixValue(vector[i].GetInvers());
                    result.Add(newVector);
                }
                    
            }
            return result;
        }

        public BoolVector Union(BoolVector vector)
        {
            var newVector = new BoolVector(this);
            for (int i = 0; i < Count; i++)
            {
                if(this[i].Value == "-" && vector[i].Value != "-")
                    newVector[i] = new MatrixValue(vector[i].Value);
            }
            return newVector;
        }

        public bool Compare(BoolVector vector)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (!this[i].Compare(vector[i]))
                    return false;
            }
            return true;
        }
    }
}
