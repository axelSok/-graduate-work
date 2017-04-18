using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using Control = System.Windows.Controls.Control;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace DNFTester.Controls.NumericUpDown
{
    public class NumericUpDown : Control
    {
        #region Members

        /// <summary>
        /// Flags if the Text and Value properties are in the process of being sync'd
        /// </summary>
        private bool _isSyncingTextAndValueProperties;
        private bool _isInitialized;

        /// <summary>
        /// Name constant for Text template part.
        /// </summary>
        internal const string ElementTextName = "PART_TextBox";

        /// <summary>
        /// Name constant for Spinner template part.
        /// </summary>
        internal const string ElementSpinnerName = "PART_Spinner";

        private const string _validChars = ".,1234567890+-";

        #endregion //Members

        #region Properties


        public bool IsDefault { get; set; }

        public double? PreviousValue { get; internal set; }

        private TextBox _textBox;
        internal TextBox PART_TextBox
        {
            get { return _textBox; }
            set
            {
                if (_textBox != value)
                {
                    if (_textBox != null)
                    {
                        _textBox.PreviewKeyDown -= TextBoxOnPreviewKeyDown;
                        _textBox.PreviewTextInput -= TextBox_TextInput;
                        _textBox.PreviewLostKeyboardFocus -= TextBox_LostFocus;
                        _textBox.KeyUp -= TextBox_KeyUp;
                    }
                    _textBox = value;
                    if (_textBox != null)
                    {
                        _textBox.PreviewKeyDown += TextBoxOnPreviewKeyDown;
                        _textBox.PreviewTextInput += TextBox_TextInput;
                        _textBox.PreviewLostKeyboardFocus += TextBox_LostFocus;
                        _textBox.KeyUp += TextBox_KeyUp;
                    }
                }
            }
        }

        public Spinner _spinner;
        internal Spinner PART_Spinner
        {
            get { return _spinner; }
            private set
            {
                _spinner = value;
                _spinner.Spin += OnSpinnerSpin;
            }
        }

        #region CanBeNull

        public bool CanBeNull
        {
            get { return (bool)GetValue(CanBeNullProperty); }
            set { SetValue(CanBeNullProperty, value); }
        }

        public static readonly DependencyProperty CanBeNullProperty =
            DependencyProperty.Register("CanBeNull", typeof(bool), typeof(NumericUpDown),
                new PropertyMetadata(true, OnCanBeNullPropertyChanged));

        private static void OnCanBeNullPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nud = (d as NumericUpDown);
            if (nud != null)
            {
                nud.Value = nud.OnCoerceValue(nud.Value);
            }
        }
        #endregion CanBeNull

        #region AllowSpin

        public static readonly DependencyProperty AllowSpinProperty = DependencyProperty.Register("AllowSpin", typeof(bool), typeof(NumericUpDown), new UIPropertyMetadata(true));
        public bool AllowSpin
        {
            get { return (bool)GetValue(AllowSpinProperty); }
            set { SetValue(AllowSpinProperty, value); }
        }

        #endregion AllowSpin

        #region ShowButtonSpinner

        public static readonly DependencyProperty ShowButtonSpinnerProperty = DependencyProperty.Register("ShowButtonSpinner", typeof(bool), typeof(NumericUpDown), new UIPropertyMetadata(true));
        public bool ShowButtonSpinner
        {
            get { return (bool)GetValue(ShowButtonSpinnerProperty); }
            set { SetValue(ShowButtonSpinnerProperty, value); }
        }

        #endregion ShowButtonSpinner

        #region IsEditable

        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(true));
        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        #endregion IsEditable

        #region Text

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(NumericUpDown), new FrameworkPropertyMetadata(default(String), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextPropertyChanged));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nud = (NumericUpDown)d;
            nud.OnTextChanged((string)e.OldValue, (string)e.NewValue);
            if (nud._isInitialized)
                nud.SyncTextAndValueProperties((string)e.NewValue);
        }

        protected void OnTextChanged(string previousValue, string currentValue)
        {

        }

        #endregion Text

        #region Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double?), typeof(NumericUpDown),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValuePropertyChanged, OnCoerceValuePropertyCallback));
        public virtual double? Value
        {
            get { return (double?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, OnCoerceValue(value)); }
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nud = (NumericUpDown)d;

            if (e.OldValue == e.NewValue) return;

            nud.PreviousValue = (double?)e.OldValue;
            nud.OnValueChanged((double?)e.OldValue, (double?)e.NewValue);

            if (nud._isInitialized)
                nud.SyncTextAndValueProperties((double?)e.NewValue);
        }

        protected virtual void OnValueChanged(double? oldValue, double? newValue)
        {
            SetValidSpinDirection();
            var args = new RoutedPropertyChangedEventArgs<double?>(oldValue, newValue) { RoutedEvent = ValueChangedEvent };
            RaiseEvent(args);
        }

        private static object OnCoerceValuePropertyCallback(DependencyObject d, object baseValue)
        {
            var num = d as NumericUpDown;
            return num != null ? num.OnCoerceValue((double?)baseValue) : baseValue;
        }

        protected double? OnCoerceValue(double? value)
        {
            if (value == null)
            {
                if (CanBeNull)
                {
                    return null;
                }
                value = 0;
            }

            if (value < Minimum)
            {
                return Minimum;
            }
            return value > Maximum ? Maximum : value;
        }

        #endregion //Value

        #region Minimum

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(NumericUpDown), new PropertyMetadata(Double.MinValue, OnMinimumPropertyChanged));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        private static void OnMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nud = (NumericUpDown)d;
            if ((double)e.NewValue > nud.Maximum) nud.Maximum = (double)e.NewValue;
            else
            {
                nud.Value = nud.Value;
                nud.SetValidSpinDirection();
            }
        }

        #endregion Minimum

        #region Maximum

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(NumericUpDown),
            new PropertyMetadata(Double.MaxValue, OnMaximumPropertyChanged, CoerceMaximumProperty));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private static void OnMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nud = (NumericUpDown)d;
            nud.Value = nud.Value;
            nud.SetValidSpinDirection();
        }

        private static object CoerceMaximumProperty(DependencyObject d, object baseValue)
        {
            var nud = (NumericUpDown)d;
            return (double)baseValue < nud.Minimum ? nud.Minimum : baseValue;
        }

        #endregion Maximum

        #region Increment

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(double), typeof(NumericUpDown), new PropertyMetadata(1.0));
        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        #endregion

        #region FormatString

        public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register("FormatString", typeof(string), typeof(NumericUpDown), new PropertyMetadata(string.Empty, OnFormatStringPropertyPropertyChanged));
        public string FormatString
        {
            get { return (string)GetValue(FormatStringProperty); }
            set { SetValue(FormatStringProperty, value); }
        }

        private static void OnFormatStringPropertyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nud = (NumericUpDown)d;
            nud.OnFormatStringChanged(e.OldValue.ToString(), e.NewValue.ToString());
        }

        protected virtual void OnFormatStringChanged(string oldValue, string newValue)
        {
            SyncTextAndValueProperties(Value);
        }

        #endregion FormatString

        #region SelectAllOnGotFocus

        public static readonly DependencyProperty SelectAllOnGotFocusProperty = DependencyProperty.Register("SelectAllOnGotFocus", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false));
        public bool SelectAllOnGotFocus
        {
            get { return (bool)GetValue(SelectAllOnGotFocusProperty); }
            set { SetValue(SelectAllOnGotFocusProperty, value); }
        }

        #endregion SelectAllOnGotFocus

        #region IsHighlighted

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false));

        #endregion IsHighlighted

        #endregion

        #region Events

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double?>), typeof(NumericUpDown));
        public event RoutedPropertyChangedEventHandler<double?> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        #endregion Events

        #region Constructors

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        #endregion Constructors

        #region Methods

        protected void SyncTextAndValueProperties(object newValue)
        {
            if (_isSyncingTextAndValueProperties)
                return;

            _isSyncingTextAndValueProperties = true;

            if (newValue is string)
            {
                var newText = newValue.ToString();
                newText = newText.Replace(".", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                newText = newText.Replace(",", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                var value = OnCoerceValue(ConvertTextToValue(newText));
                Value = value;
                Text = ConvertValueToText(value);
            }
            else
            {
                Text = ConvertValueToText((double?)newValue);
            }

            _isSyncingTextAndValueProperties = false;
        }

        protected void SyncTextAndValuePropertiesinCurrentControll(object newValue)
        {
            if (_isSyncingTextAndValueProperties)
                return;

            _isSyncingTextAndValueProperties = true;

            if (newValue is string)
            {
                try
                {
                    var newText = newValue.ToString().Replace(".", ",");
                    //if (Text != newText)
                    {
                        double? value = null;

                        if (!String.IsNullOrEmpty(newValue.ToString()))
                        {
                            double tempValue = 0;
                            if (Double.TryParse(newValue.ToString().Replace(".", ","), out tempValue))
                                value = Convert.ToDouble(newValue.ToString().Replace(".", ","));
                        }
                        Value = value;
                        Text = newText;
                    }
                }
                catch (Exception)
                {
                    SyncTextAndValueProperties(Text);
                }
            }
            else
            {
                Text = newValue.ToString();
            }

            _isSyncingTextAndValueProperties = false;
        }

        public void OnSpin(SpinEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (e.Direction == SpinDirection.Increase)
                DoIncrement();
            else
                DoDecrement();
        }

        /// <summary>
        /// Performs an increment if conditions allow it.
        /// </summary>
        private void DoDecrement()
        {
            if (PART_Spinner == null || (PART_Spinner.ValidSpinDirection & ValidSpinDirections.Decrease) == ValidSpinDirections.Decrease)
            {
                OnDecrement();
            }
        }

        /// <summary>
        /// Performs a decrement if conditions allow it.
        /// </summary>
        private void DoIncrement()
        {
            if (PART_Spinner == null || (PART_Spinner.ValidSpinDirection & ValidSpinDirections.Increase) == ValidSpinDirections.Increase)
            {
                OnIncrement();
            }
        }

        protected double? ConvertTextToValue(string text)
        {
            double result;
            if (text.Trim() == string.Empty)
            {
                return null;
            }
            if (!double.TryParse(text, out result))
            {
                PART_TextBox.Text = Text = ConvertValueToText(Value);
                return Value;
            }
            return result;
        }

        protected string ConvertValueToText(object value)
        {
            double? d;
            if (value is double?)
            {
                d = (double?)value;
            }
            else
            {
                d = Value;
            }

            if (d == null)
                return "";
            if (Double.IsNaN(d ?? 0))
                return "NaN";
            if (Double.IsPositiveInfinity(d ?? 0) || Double.MaxValue == d)
                return "Infinity";
            if (Double.IsNegativeInfinity(d ?? 0) || Double.MinValue == d)
                return "Negative-Infinity";
            return (d ?? 0).ToString(FormatString);
        }

        protected void OnIncrement()
        {
            var value = Value;
            if (value != null)
            {
                Value = OnCoerceValue((value ?? 0) + Increment);
            }
        }

        protected void OnDecrement()
        {
            double? value = Value;
            if (value != null)
            {
                Value = OnCoerceValue((value ?? 0) - Increment);
            }
        }

        private bool IsValidCharInString(string str)
        {
            foreach (var c in str)
            {
                if (!_validChars.Contains(c)) return false;
            }
            return true;
        }

        #endregion Methods

        #region Base Class Overrides

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (!_isInitialized)
            {
                _isInitialized = true;
                SyncTextAndValueProperties(Value);
            }
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_TextBox = GetTemplateChild(ElementTextName) as TextBox;
            PART_Spinner = GetTemplateChild(ElementSpinnerName) as Spinner;

            SetValidSpinDirection();

            //if (SelectAllOnGotFocus)
            {
                //in order to select all the text we must handle both the keybord (tabbing) and mouse (clicking) events
                PART_TextBox.GotKeyboardFocus += OnTextBoxGotKeyBoardFocus;
                PART_TextBox.PreviewMouseLeftButtonDown += OnTextBoxPreviewMouseLeftButtonDown;

                if (IsDefault)
                {
                    this.PART_TextBox.TextChanged += PartTextBoxOnTextChanged;
                    this.DataContextChanged += OnDataContextChanged;
                }
            }
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.DataContextChanged -= OnDataContextChanged;
            //if (Application.Current.MainWindow.IsActive)
            {
                PART_TextBox.Focus();
                PART_TextBox.SelectAll();
                //
                //PART_TextBox.RaiseEvent(new KeyboardFocusChangedEventArgs(Keyboard.PrimaryDevice, 0, null, PART_TextBox) { RoutedEvent = Keyboard.GotKeyboardFocusEvent, Source = PART_TextBox });
            }
        }

        private void PartTextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.PART_TextBox.TextChanged -= PartTextBoxOnTextChanged;
            //if (Application.Current.MainWindow.IsActive)
            {
                PART_TextBox.Focus();
                PART_TextBox.SelectAll();
                //
                //PART_TextBox.RaiseEvent(new KeyboardFocusChangedEventArgs(Keyboard.PrimaryDevice, 0, null, PART_TextBox) { RoutedEvent = Keyboard.GotKeyboardFocusEvent, Source = PART_TextBox });
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    {
                        if (AllowSpin)
                            DoIncrement();
                        e.Handled = true;
                        break;
                    }
                case Key.Down:
                    {
                        if (AllowSpin)
                            DoDecrement();
                        e.Handled = true;
                        break;
                    }
                /* case Key.Enter:
                     {
                         if (IsEditable)
                             SyncTextAndValueProperties(NumericUpDown.TextProperty, PART_TextBox.Text);
                         break;
                     }*/
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            /*if (!e.Handled && AllowSpin)
            {
                if (e.Delta < 0)
                {
                    DoDecrement();
                }
                else if (0 < e.Delta)
                {
                    DoIncrement();
                }

                e.Handled = true;
            }*/
        }

        protected override void OnAccessKey(AccessKeyEventArgs e)
        {
            if (PART_TextBox != null)
                PART_TextBox.Focus();

            base.OnAccessKey(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (PART_TextBox != null)
                PART_TextBox.Focus();
        }

        #endregion Base Class Overrides

        #region Event Handlers

        private void OnTextBoxGotKeyBoardFocus(object sender, RoutedEventArgs e)
        {
            PART_TextBox.SelectAll();
        }

        void OnTextBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!PART_TextBox.IsKeyboardFocused)
            {
                e.Handled = true;
                PART_TextBox.Focus();
            }
        }

        public void OnSpinnerSpin(object sender, SpinEventArgs e)
        {
            if (AllowSpin)
                OnSpin(e);
        }

        private void TextBox_TextInput(object sender, TextCompositionEventArgs e)
        {

            string str = e.Text;
            if (!string.IsNullOrEmpty(str))
            {
                if (!IsValidCharInString(str) || !IsNotRepeatedSumbol(str))
                {
                    e.Handled = true;
                    return;
                }
            }
            e.Handled = false;
        }

        private void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            PART_TextBox.TextChanged -= PartTextBoxOnTextChanged;
            if (e.Key == Key.Space)
            {
                _isSyncingTextAndValueProperties = false;
                var text = _textBox.Text;
                _textBox.Text = string.Empty;
                Text = String.Empty;
                Value = Double.NaN;
                SyncTextAndValuePropertiesinCurrentControll(String.Empty);
                _isSyncingTextAndValueProperties = false;
                _textBox.Text = text;
                SyncTextAndValuePropertiesinCurrentControll(text);
                e.Handled = true;
            }
        }


        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Tab && e.Key != Key.LeftShift && e.Key != Key.RightShift && e.Key != Key.Down && e.Key != Key.Up && e.Key != Key.Left && e.Key != Key.Right)
            {
                var curretnIndex = (sender as TextBox).CaretIndex;
                SyncTextAndValuePropertiesinCurrentControll(_textBox.Text);
                _textBox.CaretIndex = curretnIndex;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SyncTextAndValueProperties(Text);
            DataContextChanged -= OnDataContextChanged;
            PART_TextBox.TextChanged -= PartTextBoxOnTextChanged;
        }

        private void TextBox_PastingEventHandler(object sender, DataObjectPastingEventArgs e)
        {
            string clipboard = (e.DataObject.GetData(typeof(string)) as string).Trim();
            if (clipboard != "")
            {
                clipboard = clipboard.Replace(".", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                clipboard = clipboard.Replace(",", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                double d;
                if (!double.TryParse(clipboard, out d))
                {
                    e.CancelCommand();
                    e.Handled = true;
                }
            }
        }

        #endregion Event Handlers

        /// <summary>
        /// Проверка на валидность ввода (не позволять пользователю вводить повторяющиеся символы "+", "-", ".", ",")
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool IsNotRepeatedSumbol(string s)
        {
            if (_textBox.Text.Length > 0 && _textBox.SelectedText != _textBox.Text)
            {
                if (_textBox.Text.Contains(s) && (s == "-" || s == "+" || s == ","))
                    return false;
                if (_textBox.Text[0] != s[0] && (s == "-" || s == "+"))
                    return false;
                if (_textBox.Text.Contains(",") && s == ".") return false;
            }
            if (_textBox.Text.Length == 0 && (s == "," || s == ".")) return false;
            return true;
        }

        #region Methods

        /// <summary>
        /// Sets the valid spin direction based on current value, minimum and maximum.
        /// </summary>
        private void SetValidSpinDirection()
        {
            var validDirections = ValidSpinDirections.None;

            if (Value < Maximum)
            {
                validDirections = validDirections | ValidSpinDirections.Increase;
            }

            if (Value > Minimum)
            {
                validDirections = validDirections | ValidSpinDirections.Decrease;
            }

            if (PART_Spinner != null)
            {
                PART_Spinner.ValidSpinDirection = validDirections;
            }
        }



        #endregion Methods
    }
}
