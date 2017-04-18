#region

using System;
using System.Windows;

#endregion

namespace PariSpace.LineDesigner
{
    /// <summary>
    /// Interaction logic for LoadongTextBlock.xaml
    /// </summary>
    public partial class LoadingTextBlock
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(LoadingTextBlock), new PropertyMetadata(default(String)));

        public LoadingTextBlock()
        {
            InitializeComponent();
        }
        public LoadingTextBlock(string text)
        {
            InitializeComponent();
            Text = text;
        }

        static LoadingTextBlock()
        {
            
        }

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
