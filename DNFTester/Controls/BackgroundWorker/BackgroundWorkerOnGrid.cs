#region

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PariSpace.LineDesigner;

#endregion

namespace DNFTester.Controls.BackgroundWorkerOnGrid
{

    public class BackgroundWorkerOnGrid : BackgroundWorker
    {
        public Grid HostGrid { get; private set; }

        public bool AddMargins { get; set; }

        public double DimmerOpacity { get; set; }

        public bool DimBackground { get; set; }

        public Brush DimmerBrush { get; set; }

        public Duration DimTransitionDuration { get; set; }

        public object CurrentArgument { get; set; }

        public string LoadingString { get; set; }

        /// <summary>
        /// Создание объекта BackgroundWorker, с отображением процесса загрузки через индикатор (прелоадер).
        /// </summary>
        /// <param name="hostGrid"></param>
        public BackgroundWorkerOnGrid(Grid hostGrid)
            : base()
        {
            HostGrid = hostGrid;
            AddMargins = true;
            DimmerOpacity = 0.75;
            DimBackground = true;
            DimmerBrush = Brushes.White;
            DimTransitionDuration = new Duration(TimeSpan.FromSeconds(0.1));
        }

        /// <summary>
        /// Создание объекта BackgroundWorker, с отображением процесса загрузки через текстовое поле.
        /// </summary>
        /// <param name="hostGrid">Контрол, на котором будет располагаться текст загрузки.</param>
        /// <param name="loadingString">Текст загрузки.</param>
        public BackgroundWorkerOnGrid(Grid hostGrid, string loadingString)
            : base()
        {
            LoadingString = loadingString;
            HostGrid = hostGrid;
            AddMargins = true;
            DimmerOpacity = 0.75;
            DimBackground = true;
            DimmerBrush = Brushes.White;
            DimTransitionDuration = new Duration(TimeSpan.FromSeconds(0.1));
        }

        public new void RunWorkerAsync(object argument)
        {
            CurrentArgument = argument;
            if (this.IsBusy) return;
            ShowPreloader();
            ((BackgroundWorker)this).RunWorkerAsync(argument);
        }

        public new void RunWorkerAsync()
        {
            RunWorkerAsync(null);
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            HidePreloader();
            base.OnRunWorkerCompleted(e);
        }

        public void HidePreloader()
        {
            var grid = (Grid)LogicalTreeHelper.FindLogicalNode(HostGrid, "BusyIndicator");

            //Debug.Assert(grid != null);

            if (grid == null) return;
            grid.Name = string.Empty;

            var fadeOutAnimation = new DoubleAnimation(0.0, DimTransitionDuration);
            fadeOutAnimation.Completed += (sender, args) => OnFadeOutAnimationCompleted(HostGrid, grid);
            grid.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
        }

        private void ShowPreloader()
        {
            var grid = new Grid
            {
                Name = "BusyIndicator",
                Opacity = 0.0
            };

            if (DimBackground)
            {
                grid.Cursor = Cursors.Wait;
                grid.ForceCursor = true;
                //InputManager.Current.PreProcessInput += OnPreProcessInput;
            }
            grid.SetBinding(FrameworkElement.WidthProperty, new Binding("ActualWidth")
            {
                Source = HostGrid
            });
            grid.SetBinding(FrameworkElement.HeightProperty, new Binding("ActualHeight")
            {
                Source = HostGrid
            });
            for (int i = 1; i <= 3; ++i)
            {
                if (i != 2)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    });
                    grid.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    });
                }
                else
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = new GridLength(1, GridUnitType.Star),
                        MaxWidth = 180.0
                    });
                    grid.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Star),
                        MaxHeight = 120.0
                    });
                }
            }

            var viewbox = new Viewbox
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.Both,
                Child = GetChildControl()
                
            };
            //TODO
            viewbox.MouseRightButtonDown += ChildOnMouseRightButtonDown;
            //
            grid.SetValue(Panel.ZIndexProperty, 1000);
            grid.SetValue(Grid.RowSpanProperty, Math.Max(1, HostGrid.RowDefinitions.Count));
            grid.SetValue(Grid.ColumnSpanProperty, Math.Max(1, HostGrid.ColumnDefinitions.Count));
            if (AddMargins)
            {
                viewbox.SetValue(Grid.RowProperty, 1);
                viewbox.SetValue(Grid.ColumnProperty, 1);
            }
            else
            {
                viewbox.SetValue(Grid.RowSpanProperty, 3);
                viewbox.SetValue(Grid.ColumnSpanProperty, 3);
            }
            viewbox.SetValue(Panel.ZIndexProperty, 1);

            var dimmer = new Rectangle
            {
                Name = "Dimmer",
                Opacity = DimmerOpacity,
                Fill = DimmerBrush,
                Visibility = (DimBackground ? Visibility.Visible : Visibility.Collapsed)
            };
            dimmer.SetValue(Grid.RowSpanProperty, 3);
            dimmer.SetValue(Grid.ColumnSpanProperty, 3);
            dimmer.SetValue(Panel.ZIndexProperty, 0);
            grid.Children.Add(dimmer);

            grid.Children.Add(viewbox);

            grid.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(1.0, DimTransitionDuration));

            HostGrid.Children.Add(grid);
            HostGrid.UpdateLayout();
        }

        private UserControl GetChildControl()
        {
            return string.IsNullOrEmpty(LoadingString)
                       ? (UserControl)new CircularProgressBar()
                       : new LoadingTextBlock(LoadingString);
        }

        /// <summary>
        /// Ивент остановки выполнения асинхронного процесса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void ChildOnMouseRightButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            HidePreloader();
            StopWork();
        }
        public void StopWork()
        {
            this.HidePreloader();
            this.WorkerSupportsCancellation = true;
            this.CancelAsync();
            this.Dispose();
            this.Events.Dispose();
            GC.Collect();
        }

        private static void OnPreProcessInput(object sender, PreProcessInputEventArgs e)
        {
            /*if (e.StagingItem.Input.Device != null)
            {
                e.Cancel();
            }*/
        }

        private void OnFadeOutAnimationCompleted(Panel hostGrid, UIElement busyIndicator)
        {

            HostGrid.Children.Remove(busyIndicator);

            if (DimBackground)
            {
                //InputManager.Current.PreProcessInput -= OnPreProcessInput;
            }
        }
    }
}
