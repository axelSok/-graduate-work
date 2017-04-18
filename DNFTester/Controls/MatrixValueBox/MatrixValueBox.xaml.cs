using System;
using System.Windows;
using System.Windows.Input;
using DNFTester.Entities;

namespace DNFTester.Controls.MatrixValueBox
{
    /// <summary>
    /// Interaction logic for MatrixValueBox_.xaml
    /// </summary>
    public partial class MatrixValueBox
    {
        
        public MatrixValueBox()
        {
            InitializeComponent();
            this.PreviewMouseLeftButtonDown += TextBoxOnPreviewMouseLeftButtonDown;
            this.PreviewMouseRightButtonDown += TextBoxOnPreviewMouseRightButtonDown;
        }

        public MatrixValue Value
        {
            get { return (MatrixValue) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(MatrixValue), typeof(MatrixValueBox), new PropertyMetadata(default(MatrixValue)));

        private void TextBoxOnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Value.Next();
        }

        private void TextBoxOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Value.Prev();
        }
    }
}
