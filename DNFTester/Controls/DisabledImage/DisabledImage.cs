using System.Windows.Controls;

namespace DNFTester.Controls.DisabledImage
{
    public class DisabledImage : Image
    {
        protected override void OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name.Equals("IsEnabled"))
            {
                Opacity = !IsEnabled ? 0.5 : 1;
            }
            base.OnPropertyChanged(e);
        }
    }
}
