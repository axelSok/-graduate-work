#region

using System.ComponentModel;

#endregion

namespace PariSpace.LineDesigner
{

    public class NotifyBackgroundWorker : BackgroundWorker, INotifyPropertyChanged
    {
        public new bool IsBusy
        {
            get { return ((BackgroundWorker)this).IsBusy; }

            private set
            {                
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsBusy"));
            }
            
        }        

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            try
            {
                IsBusy = ((BackgroundWorker)this).IsBusy;
                base.OnDoWork(e);
            }
            finally
            {
                IsBusy = ((BackgroundWorker)this).IsBusy;
            }
        }

        #region Члены INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }


}
