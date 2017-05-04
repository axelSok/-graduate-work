using System;
using System.Windows;
using System.Windows.Threading;

namespace DNFTester.Controls
{
    /// <summary>
    /// Interaction logic for StatusBarControl.xaml
    /// </summary>
    public partial class StatusBarControl
    {
        public StatusBarControl()
        {
            InitializeComponent();
            startClock();
            SetDefaultApplicationState();
        }

        #region [Properties]

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(StatusBarControl), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty TimerProperty = DependencyProperty.Register("Timer", typeof(string), typeof(StatusBarControl), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(bool), typeof(StatusBarControl), new PropertyMetadata(true));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public string Timer
        {
            get { return (string)GetValue(TimerProperty); }
            set { SetValue(TimerProperty, value); }
        }

        public bool Status
        {
            get { return (bool)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        #endregion

        #region [Methods]
        private void SetDefaultApplicationState()
        {
            Status = true;
            SetMessage(string.Empty);

        }

        private void startClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickEvent;
            timer.Start();
        }

        private void tickEvent(object sender, EventArgs e)
        {
            Timer = DateTime.Now.ToLongTimeString();
        }

        private void SetMessage(string message)
        {
            Message = message;
        }
        #endregion
    }
}
