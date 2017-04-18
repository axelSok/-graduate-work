using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DNFTester.Entities;

namespace DNFTester.Controls.PolinomBlock
{
    /// <summary>
    /// Interaction logic for PolinomBlockControl.xaml
    /// </summary>
    public partial class PolinomBlockControl : UserControl
    {
        public PolinomBlockControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DNFMatrixProperty = DependencyProperty.Register("DNFMatrix", typeof(BoolMatrix), typeof(PolinomBlockControl), new PropertyMetadata(new BoolMatrix(), PropertyChangedCallback));
        public static DependencyProperty NameFunctionProperty = DependencyProperty.Register("NameFunction", typeof(string), typeof(PolinomBlockControl), new PropertyMetadata(string.Empty));
        public static DependencyProperty OperationProperty = DependencyProperty.Register("Operation", typeof(string), typeof(PolinomBlockControl), new PropertyMetadata(string.Empty));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PolinomBlockControl)d).CreateGPolinom((BoolMatrix)e.NewValue);
        }

        private void CreateGPolinom(BoolMatrix newValue)
        {
            spPolinom.Children.Clear();
            spPolinom.Children.Add(new TextBlock()
            {
                Text = NameFunction + "="
            });
            for (int i = 0; i < newValue.Count; i++)
            {
                if (newValue[i].Count(item => item.Value == "0") == newValue[i].Count)
                    spPolinom.Children.Add(new TextBlock() {Text = "1"});
                else
                {
                    for (int j = 0; j < newValue[i].Count; j++)
                    {
                        if (newValue[i][j].Value == "-" || newValue[i][j].Value == "0") continue;
                        
                        spPolinom.Children.Add(new IndexedTextBlock.IndexedTextBlock
                        {
                            Text = "X",
                            Index = (j + 1).ToString(),
                            IndexSize = 12,
                            Enverse = Visibility.Collapsed,
                            VerticalContentAlignment = VerticalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        });
                    }
                }
                
                if (i != newValue.Count - 1)
                spPolinom.Children.Add(new TextBlock()
                {
                    Text = Operation,
                    VerticalAlignment = VerticalAlignment.Top
                });
            }
        }

        public BoolMatrix DNFMatrix
        {
            get { return (BoolMatrix)GetValue(DNFMatrixProperty); }
            set { SetValue(DNFMatrixProperty, value); }
        }

        public string NameFunction
        {
            get { return (string)GetValue(NameFunctionProperty); }
            set { SetValue(NameFunctionProperty, value); }
        }

        public string Operation
        {
            get { return (string)GetValue(OperationProperty); }
            set { SetValue(OperationProperty, value); }
        }
    }
}
