#region

using System.Windows;

#endregion

namespace PariSpace.LineDesigner
{
    /// <summary>
    /// Логика взаимодействия для CircularProgressBar.xaml
    /// </summary>
    public partial class CircularProgressBar
    {
        public CircularProgressBar()
        {
            InitializeComponent();
        }

        static CircularProgressBar()
        {
            //Use a default Animation Framerate of 30, which uses less CPU time
            //than the standard 50 which you get out of the box
            //Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata
            //{
            //    DefaultValue = 20
            //});
        }

        private void UserControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Opacity = IsEnabled ? 1 : 0.5;
        }
    }
}
