using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Input;
using DNFTester.Controls.BackgroundWorkerOnGrid;
using DNFTester.Entities;
using DNFTester.Entities.AbstractMachine;
using Application = System.Windows.Forms.Application;
using Vector = DNFTester.Entities.AbstractMachine.Vector;
using System.Collections.ObjectModel;

namespace DNFTester
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(CloseWindow, ExecuteCloseWindowCommand));
            //SetDefaultApplicationState();
        }

        #region [Properties]

        private BackgroundWorkerOnGrid _worker;

        private readonly string _defaultSound = Application.StartupPath + "\\attention.wav";
        private string _soundPath;

        private bool _isMurMachine = false;

        public string SoundPath
        {
            get { return !string.IsNullOrEmpty(_soundPath) ? _soundPath : _defaultSound; }
            set
            {
                if (_soundPath != value && !string.IsNullOrEmpty(value))
                {
                    _soundPath = value;
                }
            }
        }



        public static readonly DependencyProperty VectorProperty = DependencyProperty.Register("Vector", typeof(Vector), typeof(MainWindow), new PropertyMetadata(new Vector(0, 0, 0, 0)));

        public static readonly DependencyProperty IsMachineInitProperty = DependencyProperty.Register("IsMachineInit", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public static readonly DependencyProperty BSetProperty = DependencyProperty.Register("BSet", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty MachineProperty = DependencyProperty.Register("Machine", typeof(Matrix), typeof(MainWindow), new PropertyMetadata(new Matrix(0, 0, 0)));

        public static readonly DependencyProperty MachineOutProperty = DependencyProperty.Register("MachineOut", typeof(Matrix), typeof(MainWindow), new PropertyMetadata(new Matrix(0, 0, 0)));

        public static readonly DependencyProperty StateCountProperty = DependencyProperty.Register("StateCount", typeof(int), typeof(MainWindow), new PropertyMetadata(6));

        public static readonly DependencyProperty OutputsCountProperty = DependencyProperty.Register("OutputsCount", typeof(int), typeof(MainWindow), new PropertyMetadata(2));

        public static readonly DependencyProperty InputsCountProperty = DependencyProperty.Register("InputsCount", typeof(int), typeof(MainWindow), new PropertyMetadata(4));

        public Vector Vector
        {
            get { return (Vector)GetValue(VectorProperty); }
            set { SetValue(VectorProperty, value); }
        }

        public string BSet
        {
            get { return (string)GetValue(BSetProperty); }
            set { SetValue(BSetProperty, value); }
        }

        public Matrix Machine
        {
            get { return (Matrix)GetValue(MachineProperty); }
            set { SetValue(MachineProperty, value); }
        }

        public Matrix MachineOut
        {
            get { return (Matrix)GetValue(MachineOutProperty); }
            set { SetValue(MachineOutProperty, value); }
        }

        public int StateCount
        {
            get { return (int)GetValue(StateCountProperty); }
            set { SetValue(StateCountProperty, value); }
        }

        public int OutputsCount
        {
            get { return (int)GetValue(OutputsCountProperty); }
            set { SetValue(OutputsCountProperty, value); }
        }

        public int InputsCount
        {
            get { return (int)GetValue(InputsCountProperty); }
            set { SetValue(InputsCountProperty, value); }
        }

        public bool IsMachineInit
        {
            get { return (bool)GetValue(IsMachineInitProperty); }
            set { SetValue(IsMachineInitProperty, value); }
        }





        #endregion

        #region [Methods]



        /// <summary>
        ///     Проверка правильности задания функции
        /// </summary>
        /// <returns></returns>
        private bool ValidateFunction()
        {
            if (false) //!(DNFFunction0.Count != countIn0 || DNFFunction1.Count != countIn1))
            {
                //SetValidationMessage("Функция не задана.");
                //ValidationState = false;
                //return ValidationState;
            }

            // и корректность задания матриц (не пересекаются ли они)
            /*DNFFunctionIntersection = DNFFunction0.Intersection(DNFFunction1);
            if (DNFFunctionIntersection.Count > 0)
            {
                SetValidationMessage("Пересечение ЧБФ-матриц не пусто. Проверьте корректность задания функции.");
                ValidationState = false;
                return ValidationState;
            }*/
            return true;
        }

        /// <summary>
        ///     Выдать звуковое сообщение
        /// </summary>
        /// <param name="path"></param>
        private static void MakeAlert(string path)
        {
            try
            {
                var player = new SoundPlayer(path);
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private BoolMatrix CreateTable(int width, int heigh)
        {
            var result = new BoolMatrix();
            for (var i = 0; i < heigh; i++)
            {
                result.Add(new BoolVector(width));
            }
            return new BoolMatrix(result);
        }

        #endregion

        #region [Commands]

        /// <summary>
        ///     Закрыть окно. В качестве параметра передается окно, которое необходимо закрыть.
        /// </summary>
        public static RoutedUICommand CloseWindow { get; } = new RoutedUICommand("CloseWindow", "closewindow",
            typeof(MainWindow));

        /// <summary>
        ///     Закрыть окно.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">В качестве параметра обязано передаваться окно для закрытия.</param>
        public static void ExecuteCloseWindowCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var window = e.Parameter as Window;
            window?.Close();
        }

        #endregion Commands

        #region [Handlers]

        private void btnMinimaze_OnClick(object sender, RoutedEventArgs e)
        {
            if (_isMurMachine)
            {
                for (int i = 0; i < Machine.Count; i++)
                {
                    for (int j = 0; j < Machine[i].Count; j++)
                    {
                        Machine[i][j].Output = Vector[i].Output;
                    }
                }
            }

            var result = new List<List<List<GroupItem>>>();
            for (var i = 0; i < StateCount; i++)
            {
                var subResult = new List<List<GroupItem>>();
                for (var j = 0; j < StateCount; j++)
                {
                    if (i != j && i < j && Machine[i].IsCover(Machine[j]))
                    {
                        var items = Machine[i].GetIntersection(Machine[j]);
                        subResult.Add(new List<GroupItem>(items));
                    }
                    else
                    {
                        subResult.Add(new List<GroupItem>());
                    }
                }
                result.Add(subResult);
            }

            bool hasChanged;
            do
            {
                var resultCopy = result.Select(t => new List<List<GroupItem>>(t.Select(x => x.ToList()))).ToList();
                hasChanged = false;
                for (var i = 0; i < result.Count; i++)
                {
                    for (var j = 0; j < result[i].Count; j++)
                    {
                        for (var k = 0; k < result[i][j].Count; k++)
                        {
                            var groupItem = result[i][j][k];
                            if (groupItem.First != 0 && groupItem.Second != 0 &&
                                result[groupItem.First - 1][groupItem.Second - 1].Count == 0)
                            {
                                resultCopy[i][j] = new List<GroupItem>();
                                hasChanged = true;
                            }
                        }
                    }
                }
                result = resultCopy;
            } while (hasChanged);

            var compatibleState = new List<GroupItem>();
            for (var i = 0; i < result.Count; i++)
            {
                for (var j = 0; j < result[i].Count; j++)
                {
                    for (var k = 0; k < result[i][j].Count; k++)
                    {
                        if (compatibleState.Count(item => item.First == i + 1 && item.Second == j + 1) == 0)
                        {
                            compatibleState.Add(new GroupItem(i + 1, j + 1));
                        }
                    }
                }
            }

            ///////////////////////
            var maxGroup = new List<List<int>>();
            var tempMaxGroup = new List<List<int>>();
            var stateArray = new List<int>();

            for (var i = 1; i <= StateCount; i++)
            {
                stateArray.Add(i);
            }
            maxGroup.Add(stateArray);
            tempMaxGroup.Add(stateArray);

            //сначала бежим дпо всем состояниям-столбцам
            List<int> removable;
            for (var i = 1; i <= StateCount; i++)
            {
                var one = new List<int>();
                var two = new List<int>();
                removable = new List<int>();
                //проходим по всем множествам в максимальной группировке
                for (var j = 0; j < maxGroup.Count; j++)
                {
                    var currentMaxGroup = maxGroup[j];

                    if (currentMaxGroup.All(item => item != i)) continue;

                    // получаем возможные пары для ТЕКУЩЕГО стэйта
                    var statePair = new List<GroupItem>();
                    for (var index = 0; index < currentMaxGroup.Count; index++)
                    {
                        var t = currentMaxGroup[index];
                        if (i < t) statePair.Add(new GroupItem(i, t));
                    }
                    var notСompatible =
                        statePair.Where(
                            item =>
                                !compatibleState.Any(
                                    compItem => compItem.First == item.First && compItem.Second == item.Second));
                    if (notСompatible.Any())
                    {
                        removable.Add(j);
                        var exclude = notСompatible.Select(item => item.Second);
                        one = currentMaxGroup.Select(item => item).Where(item => !exclude.Contains(item)).ToList();
                        two = currentMaxGroup.Select(item => item).ToList();
                        two.Remove(i);
                        tempMaxGroup.Add(one);
                        tempMaxGroup.Add(two);
                    }
                }
                for (var j = removable.Count - 1; j >= 0; j--)
                {
                    tempMaxGroup.RemoveAt(removable[j]);
                }

                // получили максимальную группировку (ещё не приведенную!!!)
                maxGroup = tempMaxGroup.Select(t => new List<int>(t.ToList())).ToList();
            }

            removable = new List<int>();
            for (var j = 0; j < maxGroup.Count; j++)
            {
                for (var k = 0; k < maxGroup.Count; k++)
                {
                    if (j == k) continue;

                    var isIncluded = maxGroup[j].Where(item => !maxGroup[k].Any(compItem => compItem == item));
                    if (!isIncluded.Any() && !removable.Contains(j))
                    {
                        removable.Add(j);
                    }
                }
            }

            removable.Sort();
            for (var j = removable.Count - 1; j >= 0; j--)
            {
                maxGroup.RemoveAt(removable[j]);
            }

            var first = maxGroup.First(item => item.Contains(1));
            var other = maxGroup.Where(item => !item.Equals(first));
            var b = new List<List<int>> { first };
            b.AddRange(other);

            var states = new List<List<List<int>>>(b.Count);
            var outs = new List<List<List<int>>>(b.Count);
            for (var i = 0; i < b.Count; i++)
            {
                var statesList = new List<List<int>>(InputsCount);
                var outsList = new List<List<int>>(InputsCount);
                for (var j = 0; j < InputsCount; j++)
                {
                    statesList.Add(new List<int>());
                    outsList.Add(new List<int>());
                }
                states.Add(statesList);
                outs.Add(outsList);
            }
            for (var i = 0; i < b.Count; i++)
            {
                var bi = b[i];
                for (var j = 0; j < bi.Count; j++)
                {
                    var vector = Machine[bi[j] - 1];
                    for (var k = 0; k < vector.Count; k++)
                    {
                        var tempState = (int)vector[k].State;
                        var tempOut = (int)vector[k].Output;
                        if (!states[i][k].Contains(tempState) && tempState != 0)
                        {
                            states[i][k].Add(tempState);
                        }
                        if (!outs[i][k].Contains(tempOut))
                        {
                            outs[i][k].Add(tempOut);
                        }
                    }
                }
            }

            MachineOut = new Matrix();
            for (var i = 0; i < b.Count; i++)
            {
                var listVector = new List<ItemValue>();
                for (var j = 0; j < InputsCount; j++)
                {
                    var bState = 0;
                    for (var k = 0; k < b.Count; k++)
                    {
                        if (states[i][j].Count > 0 && states[i][j].All(item => b[k].Contains(item)))
                        {
                            bState = k + 1;
                            continue;
                        }
                    }
                    var bOut = outs[i][j].Max(item => item);
                    listVector.Add(new ItemValue(bState, bOut, b.Count, OutputsCount));
                }
                MachineOut.Add(new Vector(listVector, i + 1));
            }
            BSet = "";
            for (var i = 0; i < b.Count; i++)
            {
                BSet += "b" + (i + 1) + "={";
                for (var j = 0; j < b[i].Count; j++)
                {
                    BSet += "a" + b[i][j];
                    if (j != b[i].Count - 1) BSet += ", ";
                }
                BSet += "}";
                if (i != b.Count - 1) BSet += ", ";
            }
        }

        private void BtnCreateTable_OnClick(object sender, RoutedEventArgs e)
        {
            Machine = new Matrix(StateCount, InputsCount, OutputsCount);
            Machine = new Matrix
            {
                new Vector(new ObservableCollection<ItemValue>
                {
                    new ItemValue(2, 0, StateCount, OutputsCount),
                    new ItemValue(3, 1, StateCount, OutputsCount),
                    new ItemValue(0, 0, StateCount, OutputsCount),
                    new ItemValue(4, 0, StateCount, OutputsCount)
                }, 1),
                new Vector(new ObservableCollection<ItemValue>
                {
                    new ItemValue(3, 1, StateCount, OutputsCount),
                    new ItemValue(5, 1, StateCount, OutputsCount),
                    new ItemValue(0, 0, StateCount, OutputsCount),
                    new ItemValue(0, 0, StateCount, OutputsCount)
                }, 2),
                new Vector(new ObservableCollection<ItemValue>
                {
                    new ItemValue(4, 0, StateCount, OutputsCount),
                    new ItemValue(6, 0, StateCount, OutputsCount),
                    new ItemValue(3, 0, StateCount, OutputsCount),
                    new ItemValue(0, 0, StateCount, OutputsCount)
                }, 3),
                new Vector(new ObservableCollection<ItemValue>
                {
                    new ItemValue(5, 2, StateCount, OutputsCount),
                    new ItemValue(3, 1, StateCount, OutputsCount),
                    new ItemValue(0, 0, StateCount, OutputsCount),
                    new ItemValue(1, 0, StateCount, OutputsCount)
                }, 4),
                new Vector(new ObservableCollection<ItemValue>
                {
                    new ItemValue(0, 0, StateCount, OutputsCount),
                    new ItemValue(6, 1, StateCount, OutputsCount),
                    new ItemValue(0, 0, StateCount, OutputsCount),
                    new ItemValue(0, 0, StateCount, OutputsCount)
                }, 5),
                new Vector(new ObservableCollection<ItemValue>
                {
                    new ItemValue(0, 0, StateCount, OutputsCount),
                    new ItemValue(0, 2, StateCount, OutputsCount),
                    new ItemValue(4, 0, StateCount, OutputsCount),
                    new ItemValue(2, 0, StateCount, OutputsCount)
                }, 6)
            };
            Vector = new Vector(OutputsCount, StateCount, OutputsCount, 0);
            IsMachineInit = true;
            //SetDefaultApplicationState();
        }

        #endregion

        private void BtStartMili_OnClick(object sender, RoutedEventArgs e)
        {
            //gdStart.Visibility = Visibility.Collapsed;
            gdWorkSpace.Visibility = Visibility.Visible;
            gbMure.Visibility = Visibility.Collapsed;
            gbMili.Visibility = Visibility.Visible;
        }

        private void BtStartMure_OnClick(object sender, RoutedEventArgs e)
        {
            _isMurMachine = true;
            //gdStart.Visibility = Visibility.Collapsed;
            gdWorkSpace.Visibility = Visibility.Visible;
            gbMili.Visibility = Visibility.Collapsed;
            gbMure.Visibility = Visibility.Visible;
        }
    }
}