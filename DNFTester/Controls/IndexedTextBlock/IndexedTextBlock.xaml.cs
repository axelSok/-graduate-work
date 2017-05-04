using System.Windows;

namespace DNFTester.Controls
{
    /// <summary>
    /// Interaction logic for IndexedTextBox.xaml
    /// </summary>
    public partial class IndexedTextBlock
    {
        public static readonly DependencyProperty EnverseProperty = DependencyProperty.Register("Enverse", typeof(Visibility), typeof(IndexedTextBlock), new PropertyMetadata(default(Visibility)));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(IndexedTextBlock), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(string), typeof(IndexedTextBlock), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty IndexSizeProperty = DependencyProperty.Register("IndexSize", typeof(int), typeof(IndexedTextBlock), new PropertyMetadata(default(int)));

        public IndexedTextBlock()
        {
            Enverse = Visibility.Collapsed;
            InitializeComponent();
        }

        public Visibility Enverse
        {
            get { return (Visibility) GetValue(EnverseProperty); }
            set { SetValue(EnverseProperty, value); }
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Index
        {
            get { return (string) GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public int IndexSize
        {
            get { return (int) GetValue(IndexSizeProperty); }
            set { SetValue(IndexSizeProperty, value); }
        }
    }
}
