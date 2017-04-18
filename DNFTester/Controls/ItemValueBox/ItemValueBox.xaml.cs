using System.Windows;
using DNFTester.Entities.AbstractMachine;

namespace DNFTester.Controls.ItemValueBox
{
    /// <summary>
    ///     Interaction logic for MatrixValueBox_.xaml
    /// </summary>
    public partial class ItemValueBox
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(ItemValue), typeof(ItemValueBox), new PropertyMetadata(new ItemValue(0, 0, 5, 5)));

        public static readonly DependencyProperty ShowStateProperty = DependencyProperty.Register("ShowState",
            typeof(bool), typeof(ItemValueBox), new PropertyMetadata(true));

        public static readonly DependencyProperty ShowOutputProperty = DependencyProperty.Register("ShowOutput",
            typeof(bool), typeof(ItemValueBox), new PropertyMetadata(true));

        public ItemValueBox()
        {
            InitializeComponent();
        }

        public ItemValue Value
        {
            get { return (ItemValue)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public bool ShowState
        {
            get { return (bool)GetValue(ShowStateProperty); }
            set { SetValue(ShowStateProperty, value); }
        }

        public bool ShowOutput
        {
            get { return (bool)GetValue(ShowOutputProperty); }
            set { SetValue(ShowOutputProperty, value); }
        }
    }
}