using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DNFTester.Entities
{
    public class BoolMatrix : ObservableCollection<BoolVector>
    {
        public BoolMatrix() : base() { }

        public BoolMatrix(BoolMatrix matrix)
        {
            foreach (BoolVector bv in matrix)
                this.Add(new BoolVector(bv));
        }

        public BoolMatrix(IEnumerable<BoolVector> matrix)
        {
            foreach (BoolVector bv in matrix)
                this.Add(new BoolVector(bv));
        }

        public bool Contains(BoolVector vector)
        {
            return this.Any(boolVector => boolVector.Equals(vector));
        }

        /// <summary>
        /// Обобщенное склеивание матрицы
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public BoolMatrix GeneralBonding()
        {
            bool repeat;
            var newMatrix = new BoolMatrix(this);
            do
            {
                repeat = false;
                for (var i = 0; i < newMatrix.Count - 1; i++)
                {
                    for (var j = i + 1; j < newMatrix.Count; j++)
                    {
                        var boundVector = newMatrix[i].GenericBonding(newMatrix[j]);
                        if (boundVector != null && !newMatrix.Contains(boundVector))
                        {
                            newMatrix.Add(boundVector);
                            repeat = true;
                        }
                    }
                }
            } while (repeat);
            
            return newMatrix;
        }

        /// <summary>
        /// Склеивание матрицы
        /// </summary>
        /// <returns></returns>
        public BoolMatrix Bonding()
        {
            var newMatrix = new BoolMatrix(this);
            for (var i = 0; i < Count - 1; i++)
            {
                for (var j = i + 1; j < Count; j++)
                {
                    var boundVector = this[i].Gluing(this[j]);
                    if (boundVector != null)
                    {
                        newMatrix.Add(boundVector);
                    }
                }
            }
            return newMatrix.Absorption();
        }

        /// <summary>
        /// Поглащение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public BoolMatrix Absorption()
        {
            var result = new BoolMatrix();
            var deleted = new List<int>();
            for (var i = 0; i < Count; i++)
            {
                for (var j = 0; j < Count; j++)
                {
                    if (deleted.Contains(j) || deleted.Contains(i)) continue;
                    if (i == j) continue;
                    
                    if (this[i].IsAbsorpt(this[j]) && !deleted.Contains(j))
                        deleted.Add(j);
                }
            }
            for (var i = 0; i < Count; i++)
            {
                if(!deleted.Contains(i))
                result.Add(this[i]);
            }
            return result;
        }

        /// <summary>
        /// Гамма-преобразование над матрицей
        /// </summary>
        /// <returns></returns>
        public BoolMatrix GammaConvertion()
        {
            var n = this[0].Count;
            var A = new BoolMatrix(this);
            var A0 = new BoolMatrix();
            var A1 = new BoolMatrix();

            for (var i = 0; i < n; i++)
            {
                A0 = new BoolMatrix(A.Where(item => item[i].Value == "0" || item[i].Value == "-"));
                A1 = new BoolMatrix(A.Where(item => item[i].Value == "1" || item[i].Value == "-"));
                var Ai = new BoolMatrix();

                for (int j = 0; j < A0.Count; j++)
                {
                    A0[j][i] = new MatrixValue("0");

                }
                for (int j = 0; j < A1.Count; j++)
                {
                    A1[j][i] = new MatrixValue("1");
                }
                for (int j = 0; j < A0.Count; j++)
                {
                    var temp = new BoolVector(A0[j]);
                    temp[i] = new MatrixValue("1");
                    Ai.Add(temp);
                }

                A0 = A0.Bonding();
                A1 = A1.Bonding();
                Ai = Ai.Bonding();
                var t0 = A1.Substraction(Ai);
                var t1 = Ai.Substraction(A1);
                var t2 = A0.Union(t0);
                var t3 = t2.Union(t1);
                t3 = t3.Bonding();
                A = new BoolMatrix(t3);

            }
            return new BoolMatrix(A);
        }

        /// <summary>
        /// Объединение матриц
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private BoolMatrix Union(BoolMatrix matrix)
        {
            var result = new BoolMatrix(this);
            foreach (var bv in matrix)
            {
                result.Add(bv);
            }
            if (result.Count > 1)
                result.Absorption();
            return result.Bonding();
        }

        /// <summary>
        /// Пересечение матриц
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public BoolMatrix Intersection(BoolMatrix matrix)
        {
            var result = new BoolMatrix();
            for (int i = 0; i < Count; i++)
            {
                foreach (BoolVector t in matrix.Where(t => !this[i].IsOrtogonal(t)))
                {
                    result.Add(this[i].Union(t));
                }
            }
            return result;
        }

        /// <summary>
        /// Разность булевых матриц
        /// </summary>
        /// <param name="matrix">Отнимаемое</param>
        /// <returns></returns>
        private BoolMatrix Substraction(BoolMatrix matrix)
        {
            var t = new BoolMatrix(this);
            for (int i = 0; i < matrix.Count; i++)
            {
                var newT = new BoolMatrix();
                for (int j = 0; j < t.Count; j++)
                {
                    var temp = t[j].Substraction(matrix[i]);
                    if (temp != null)
                        foreach (var vector in temp)
                        {
                            newT.Add(vector);
                        }
                }
                t = newT;
            }
            return t;
        }

        /// <summary>
        /// Построить полную матрицу векторов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public BoolMatrix GetExtendedMatrix()
        {
            var binaryVectors = new BoolMatrix(this.Where(item => item.Count(value => value.Value == "-") == 0));

            var ternaryVectors1 = new BoolMatrix(this.Where(item => item.Count(value => value.Value == "-") > 0));
            bool repeat;
            do
            {
                repeat = false;
                var ternaryVectors2 = new BoolMatrix();
                foreach (BoolVector boolVector in ternaryVectors1)
                {
                    for (int i = 0; i < boolVector.Count; i++)
                    {
                        if (boolVector[i].Value == "-")
                        {
                            var newVector1 = new BoolVector(boolVector);
                            var newVector2 = new BoolVector(boolVector);
                            newVector1[i] = new MatrixValue("0");
                            newVector2[i] = new MatrixValue("1");

                            if (newVector1.Any(item => item.Value == "-"))
                            {
                                ternaryVectors2.Add(newVector1);
                                repeat = true;
                            }  
                            else if(!binaryVectors.Contains(newVector1))
                                binaryVectors.Add(newVector1);

                            if (newVector2.Any(item => item.Value == "-"))
                            {
                                ternaryVectors2.Add(newVector2);
                                repeat = true;
                            }
                            else if (!binaryVectors.Contains(newVector2)) 
                                binaryVectors.Add(newVector2);
                        }
                    }
                }
                if (repeat)
                    ternaryVectors1 = new BoolMatrix(ternaryVectors2);
            } while (repeat);
            
            return binaryVectors;
        }

        /// <summary>
        /// Построить полную матрицу векторов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public BoolMatrix Inverse()
        {
            var result = new BoolMatrix(this);
            for (int i = 0; i < this.Count; i++)
            {
                for (int j = 0; j < this[i].Count; j++)
                {
                    result[i][j] = new MatrixValue(this[i][j].GetInvers());
                }
            }
            return result;
        }

        public bool IsConst0()
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Count(value => value.Value == "0" || value.Value == "-") == this[i].Count)
                    return false;
            }
            return true;
        }

        public bool IsConst1()
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Count(value => value.Value == "-" || value.Value == "1") == this[i].Count)
                    return true;
            }
            return false;
        }

        public bool IsMonoton()
        {
            var absorption = this.GeneralBonding().Absorption();
            var count = absorption.Count(t => t.Count(value => value.Value == "-" || value.Value == "1") == t.Count);
            return absorption.Count == count;
        }

        public bool HasZeroVector()
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Count(value => value.Value == "0" || value.Value == "-") == this[i].Count)
                    return true;
            }
            return false;
        }

        public bool HasOneVector()
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Count(value => value.Value == "1" || value.Value == "-") == this[i].Count)
                    return true;
            }
            return false;
        }

        public bool Compare(BoolMatrix matrix)
        {
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < matrix.Count; j++)
                {
                    if (!this[i].Compare(matrix[j]))
                        return false;
                }
            }
            return true;
        }
    }
}
