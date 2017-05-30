using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Printing;
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
            initializeMinimizedMatrix();
            setDefaultApplicationState();

            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(CloseWindow, ExecuteCloseWindowCommand));
        }

        #region [Properties]

        private BackgroundWorkerOnGrid _worker;

        private readonly string _defaultSound = Application.StartupPath + "\\attention.wav";
        private string _soundPath;

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

        private Matrix _minMiliMatrix = new Matrix();
        private Matrix _minMureMatrix = new Matrix();

        public static readonly DependencyProperty VectorProperty = DependencyProperty.Register("Vector", typeof(Vector), typeof(MainWindow), new PropertyMetadata(new Vector(0, 0, 0, 0)));

        public static readonly DependencyProperty IsMinimazingProperty = DependencyProperty.Register("IsMinimazing", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));
        public static readonly DependencyProperty IsGeneratingProperty = DependencyProperty.Register("IsGenerating", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));
        public static readonly DependencyProperty IsMiliProperty = DependencyProperty.Register("IsMili", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));
        public static readonly DependencyProperty IsMureProperty = DependencyProperty.Register("IsMure", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

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

        public bool IsMili
        {
            get { return (bool)GetValue(IsMiliProperty); }
            set { SetValue(IsMiliProperty, value); }
        }
        public bool IsMure
        {
            get { return (bool)GetValue(IsMureProperty); }
            set { SetValue(IsMureProperty, value); }
        }
        public bool IsMinimazing
        {
            get { return (bool)GetValue(IsMinimazingProperty); }
            set { SetValue(IsMinimazingProperty, value); }
        }
        public bool IsGenerating
        {
            get { return (bool)GetValue(IsGeneratingProperty); }
            set { SetValue(IsGeneratingProperty, value); }
        }
        #endregion

        #region [Methods]

        private void setDefaultApplicationState()
        {
            gdStart.Visibility = Visibility.Visible;
            gdWorkSpace.Visibility = Visibility.Collapsed;
        }

        private void setTabVisibility()
        {
            this.tiMinMili.IsSelected = IsMinimazing && IsMili;
            this.tiMinMura.IsSelected = IsMinimazing && IsMure;
            this.tiGenMili.IsSelected = IsGenerating && IsMili;
            this.tiGenMura.IsSelected = IsGenerating && IsMure;
        }

        private Matrix getMinMiliMatrix()
        {
            var res = new Matrix();

            res.Add(new Vector(new List<ItemValue> {
                new ItemValue(2, 0, 4, 2),
                new ItemValue(4, 1, 4, 2),
                new ItemValue(0, 0, 4, 2),
                new ItemValue(3, 0, 4, 2) }, 1));
            res.Add(new Vector(new List<ItemValue> {
                new ItemValue(4, 1, 4, 2),
                new ItemValue(3, 1, 4, 2),
                new ItemValue(0, 0, 4, 2),
                new ItemValue(0, 0, 4, 2) }, 2));
            res.Add(new Vector(new List<ItemValue> {
                new ItemValue(3, 2, 4, 2),
                new ItemValue(4, 1, 4, 2),
                new ItemValue(4, 0, 4, 2),
                new ItemValue(1, 0, 4, 2) }, 3));
            res.Add(new Vector(new List<ItemValue> {
                new ItemValue(3, 0, 4, 2),
                new ItemValue(4, 2, 4, 2),
                new ItemValue(3, 0, 4, 2),
                new ItemValue(2, 0, 4, 2) }, 4));
            return res;
        }

        private Matrix getMinMureMatrix()
        {
            var res = new Matrix();


            res.Add(new Vector(new List<ItemValue> {
                new ItemValue(4, 1, 4, 2),
                new ItemValue(2, 1, 4, 2),
                new ItemValue(1, 1, 4, 2)}, 1));
            res.Add(new Vector(new List<ItemValue> {
                new ItemValue(4, 1, 4, 2),
                new ItemValue(3, 1, 4, 2),
                new ItemValue(1, 1, 4, 2) }, 2));
            res.Add(new Vector(new List<ItemValue> {
                new ItemValue(1, 2, 4, 2),
                new ItemValue(2, 2, 4, 2),
                new ItemValue(1, 2, 4, 2) }, 3));
            res.Add(new Vector(new List<ItemValue> {
                new ItemValue(2, 2, 4, 2),
                new ItemValue(2, 2, 4, 2),
                new ItemValue(2, 2, 4, 2) }, 4));
            return res;
        }

        private void initializeMinimizedMatrix()
        {
            _minMiliMatrix = getMinMiliMatrix();
            _minMureMatrix = getMinMureMatrix();
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

        private Matrix minimaze(Matrix machineIn)
        {
            if (IsMure)
            {
                for (int i = 0; i < machineIn.Count; i++)
                {
                    for (int j = 0; j < machineIn[i].Count; j++)
                    {
                        machineIn[i][j].Output = Vector[i].Output;
                    }
                }
            }

            var result = new List<List<List<GroupItem>>>();
            for (var i = 0; i < StateCount; i++)
            {
                var subResult = new List<List<GroupItem>>();
                for (var j = 0; j < StateCount; j++)
                {
                    if (i != j && i < j && machineIn[i].IsCover(machineIn[j]))
                    {
                        var items = machineIn[i].GetIntersection(machineIn[j]);
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
                    var vector = machineIn[bi[j] - 1];
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

            var resMachine = new Matrix();
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
                resMachine.Add(new Vector(listVector, i + 1));
            }
            return resMachine;
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
            setTabVisibility();

            if (IsMinimazing)
            {
                MachineOut = minimaze(Machine);
            }
            else
            {
                initializeMinimizedMatrix();

                var needStateCount = StateCount;
                var resultMatrix = new Matrix();
                if (IsMili)
                {
                    resultMatrix = getMinMiliMatrix();

                    StateCount = 4;
                    InputsCount = 4;
                    OutputsCount = 2;

                    do
                    {
                        var random = new Random();
                        var tempMatrix = resultMatrix;
                        var randomColIndex = random.Next(0, StateCount);
                        var randomRowIndexState = random.Next(0, InputsCount);
                        var newState = random.Next(0, StateCount + 1);
                        var newOut = random.Next(0, OutputsCount + 1);

                        var stateCol = new Vector(tempMatrix[randomColIndex], StateCount + 1);
                        stateCol[randomRowIndexState] = new ItemValue(newState, newOut, StateCount + 1, OutputsCount);
                        if (tempMatrix.Contains(stateCol)) { continue; }
                        tempMatrix.Add(stateCol);
                        ++StateCount;
                        var temp = minimaze(tempMatrix);
                        var notEqual = 0;
                        if (temp.Count == _minMiliMatrix.Count)
                        {
                            for (var i = 0; i < temp.Count; i++)
                            {
                                for (var j = 0; j < InputsCount; j++)
                                {
                                    if (temp[i][j].State != _minMiliMatrix[i][j].State || temp[i][j].Output != _minMiliMatrix[i][j].Output)
                                    {
                                        ++notEqual;
                                    }
                                }
                            }
                            if (notEqual != 0)
                            {
                                --StateCount;
                                resultMatrix.RemoveAt(resultMatrix.Count - 1);
                            }
                        }
                        else
                        {
                            --StateCount;
                            resultMatrix.RemoveAt(resultMatrix.Count - 1);
                        }


                    } while (resultMatrix.Count < needStateCount);

                    MachineOut = _minMiliMatrix;
                    Machine = resultMatrix;
                }
                else
                {
                    resultMatrix = getMinMureMatrix();
                    StateCount = 4;
                    InputsCount = 3;
                    OutputsCount = 2;
                    Vector = new Vector(new List<ItemValue> {
                        new ItemValue(0, 1, 4, 2),
                        new ItemValue(0, 1, 4, 2),
                        new ItemValue(0, 2, 4, 2),
                        new ItemValue(0, 2, 4, 2)}, 1);
                    do
                    {
                        var random = new Random();
                        var tempMatrix = resultMatrix;
                        var randomColIndex = random.Next(0, StateCount);
                        var randomRowIndexState = random.Next(0, InputsCount);
                        var newState = random.Next(0, StateCount + 1);
                        var newOut = random.Next(0, OutputsCount + 1);

                        var stateCol = new Vector(tempMatrix[randomColIndex], StateCount + 1);
                        for (var i = 0; i < stateCol.Count; i++)
                        {
                            stateCol[i] = new ItemValue((int)stateCol[i].State, newOut, StateCount + 1, OutputsCount);
                        }
                        stateCol[randomRowIndexState] = new ItemValue(newState, newOut, StateCount + 1, OutputsCount);
                        if (tempMatrix.Contains(stateCol)) { continue; }
                        this.Vector.Add(new ItemValue(0, newOut, 4, 2));
                        tempMatrix.Add(stateCol);
                        ++StateCount;
                        var temp = minimaze(tempMatrix);
                        var notEqual = 0;
                        if (temp.Count == _minMureMatrix.Count)
                        {
                            for (var i = 0; i < temp.Count; i++)
                            {
                                for (var j = 0; j < InputsCount; j++)
                                {
                                    if (temp[i][j].State != _minMureMatrix[i][j].State || temp[i][j].Output != _minMureMatrix[i][j].Output)
                                    {
                                        ++notEqual;
                                    }
                                }
                            }
                            if (notEqual != 0)
                            {
                                --StateCount;
                                resultMatrix.RemoveAt(resultMatrix.Count - 1);
                                this.Vector.RemoveAt(this.Vector.Count - 1);
                            }
                        }
                        else
                        {
                            --StateCount;
                            resultMatrix.RemoveAt(resultMatrix.Count - 1);
                            this.Vector.RemoveAt(this.Vector.Count - 1);
                        }
                    } while (resultMatrix.Count < needStateCount);

                    MachineOut = _minMureMatrix;
                    Machine = resultMatrix;
                }
            }
        }

        private void BtnInitialize_OnClick(object sender, RoutedEventArgs e)
        {
            setTabVisibility();
            Machine = new Matrix(StateCount, InputsCount, OutputsCount);
            Vector = new Vector(StateCount, StateCount, OutputsCount, 0);

            MachineOut = new Matrix(0, 0, 0);
            BSet = string.Empty;
        }

        #endregion

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Выполнил: студент 5-го курса \nСоколовский Александр Иванович \nНаучный руководитель: доцент \nСупрун Валерий Павлович", "О программе", MessageBoxButton.OK);
        }

        private void BtStart_OnClick(object sender, RoutedEventArgs e)
        {
            gdStart.Visibility = Visibility.Collapsed;
            gdWorkSpace.Visibility = Visibility.Visible;
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
                printDialog.PrintVisual(tcPrint, "Распечатать");
            }
        }
    }
}