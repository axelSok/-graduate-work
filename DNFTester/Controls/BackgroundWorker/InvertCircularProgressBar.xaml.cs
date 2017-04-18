#region

using System.Windows;

#endregion

namespace PariSpace.LineDesigner
{
    /// <summary>
    /// Логика взаимодействия для CircularProgressBar.xaml
    /// </summary>
    public partial class InvertCircularProgressBar
    {
        public InvertCircularProgressBar()
        {
            InitializeComponent();
        }

        private void UserControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsEnabled) Opacity = 1; else Opacity = 0.5;
        }
    }
}
