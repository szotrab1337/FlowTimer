using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace FlowTimer.Wpf.Controls
{
    public class DateTimePicker : Control
    {
        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register(nameof(SelectedDateTime), typeof(DateTime?), typeof(DateTimePicker),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedDateTimeChanged));

        public static readonly DependencyProperty ShowTimeProperty =
            DependencyProperty.Register(nameof(ShowTime), typeof(bool), typeof(DateTimePicker),
                new PropertyMetadata(true));

        private DatePicker? _datePicker;
        private TextBox? _hourTextBox;
        private bool _isUpdatingText;
        private TextBox? _minuteTextBox;
        private TextBox? _secondTextBox;

        static DateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePicker),
                new FrameworkPropertyMetadata(typeof(DateTimePicker)));
        }

        public DateTime? SelectedDateTime
        {
            get => (DateTime?)GetValue(SelectedDateTimeProperty);
            set => SetValue(SelectedDateTimeProperty, value);
        }

        public bool ShowTime
        {
            get => (bool)GetValue(ShowTimeProperty);
            set => SetValue(ShowTimeProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _datePicker = GetTemplateChild("PART_DatePicker") as DatePicker;
            _hourTextBox = GetTemplateChild("PART_HourTextBox") as TextBox;
            _minuteTextBox = GetTemplateChild("PART_MinuteTextBox") as TextBox;
            _secondTextBox = GetTemplateChild("PART_SecondTextBox") as TextBox;

            if (_datePicker != null)
            {
                _datePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
            }

            if (_hourTextBox != null)
            {
                _hourTextBox.TextChanged += TimeTextBox_TextChanged;
                _hourTextBox.PreviewTextInput += NumericTextBox_PreviewTextInput;
                _hourTextBox.GotFocus += TimeTextBox_GotFocus;
                _hourTextBox.LostFocus += TimeTextBox_LostFocus;
                _hourTextBox.PreviewMouseDown += TimeTextBox_PreviewMouseDown;
                _hourTextBox.PreviewKeyDown += TimeTextBox_PreviewKeyDown;
                _hourTextBox.PreviewMouseWheel += TimeTextBox_PreviewMouseWheel;
            }

            if (_minuteTextBox != null)
            {
                _minuteTextBox.TextChanged += TimeTextBox_TextChanged;
                _minuteTextBox.PreviewTextInput += NumericTextBox_PreviewTextInput;
                _minuteTextBox.GotFocus += TimeTextBox_GotFocus;
                _minuteTextBox.LostFocus += TimeTextBox_LostFocus;
                _minuteTextBox.PreviewMouseDown += TimeTextBox_PreviewMouseDown;
                _minuteTextBox.PreviewKeyDown += TimeTextBox_PreviewKeyDown;
                _minuteTextBox.PreviewMouseWheel += TimeTextBox_PreviewMouseWheel;
            }

            if (_secondTextBox != null)
            {
                _secondTextBox.TextChanged += TimeTextBox_TextChanged;
                _secondTextBox.PreviewTextInput += NumericTextBox_PreviewTextInput;
                _secondTextBox.GotFocus += TimeTextBox_GotFocus;
                _secondTextBox.LostFocus += TimeTextBox_LostFocus;
                _secondTextBox.PreviewMouseDown += TimeTextBox_PreviewMouseDown;
                _secondTextBox.PreviewKeyDown += TimeTextBox_PreviewKeyDown;
                _secondTextBox.PreviewMouseWheel += TimeTextBox_PreviewMouseWheel;
            }

            UpdateControls();
        }

        private void DatePicker_SelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (!_isUpdatingText)
            {
                UpdateDateTime();
            }
        }

        private void DecrementValue(TextBox textBox)
        {
            if (!int.TryParse(textBox.Text, out var value))
            {
                return;
            }

            var maxValue = textBox == _hourTextBox ? 23 : 59;
            value = value == 0 ? maxValue : value - 1;

            _isUpdatingText = true;
            textBox.Text = value.ToString("D2");
            _isUpdatingText = false;
            UpdateDateTime();
        }

        private void IncrementValue(TextBox textBox)
        {
            if (!int.TryParse(textBox.Text, out var value))
            {
                return;
            }

            var maxValue = textBox == _hourTextBox ? 23 : 59;
            value = (value + 1) % (maxValue + 1);
            _isUpdatingText = true;
            textBox.Text = value.ToString("D2");
            _isUpdatingText = false;
            UpdateDateTime();
        }

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
                return;
            }

            if (sender is not TextBox textBox)
            {
                return;
            }

            string newText;
            int newCaretPosition;

            // If something is selected, remove it
            if (textBox.SelectionLength > 0)
            {
                var selStart = textBox.SelectionStart;
                newText = textBox.Text.Remove(selStart, textBox.SelectionLength);
                newText = newText.Insert(selStart, e.Text);
                newCaretPosition = selStart + 1;
            }
            else
            {
                // If there are already 2 digits, replace the first digit and move
                if (textBox.Text.Length >= 2)
                {
                    newText = textBox.Text.Substring(1) + e.Text;
                    newCaretPosition = 2;
                }
                else
                {
                    // Add a digit to the end
                    newText = textBox.Text + e.Text;
                    newCaretPosition = newText.Length;
                }
            }

            // Validate value
            if (int.TryParse(newText, out var value))
            {
                var maxValue = textBox == _hourTextBox ? 23 : 59;

                if (value > maxValue)
                {
                    e.Handled = true;
                    return;
                }
            }

            _isUpdatingText = true;
            textBox.Text = newText.PadLeft(2, '0');
            textBox.SelectionStart = newCaretPosition;
            _isUpdatingText = false;

            UpdateDateTime();
            e.Handled = true;
        }

        private static void OnSelectedDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DateTimePicker picker)
            {
                picker.UpdateControls();
            }
        }

        private void TimeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Highlight all text when selected
                textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()),
                    DispatcherPriority.Input);
            }
        }

        private void TimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox { Text.Length: 1 } textBox)
            {
                return;
            }

            _isUpdatingText = true;
            textBox.Text = "0" + textBox.Text;
            _isUpdatingText = false;
            UpdateDateTime();
        }

        private void TimeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is not TextBox textBox)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Up:
                    IncrementValue(textBox);
                    e.Handled = true;
                    break;
                case Key.Down:
                    DecrementValue(textBox);
                    e.Handled = true;
                    break;
            }
        }

        private void TimeTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not TextBox { IsKeyboardFocusWithin: false } textBox)
            {
                return;
            }

            e.Handled = true;
            textBox.Focus();
        }

        private void TimeTextBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is not TextBox textBox)
            {
                return;
            }

            if (e.Delta > 0)
            {
                IncrementValue(textBox);
            }
            else if (e.Delta < 0)
            {
                DecrementValue(textBox);
            }

            e.Handled = true;
        }

        private void TimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText)
            {
                return;
            }

            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    _isUpdatingText = true;
                    textBox.Text = "00";
                    textBox.SelectionStart = 0;
                    textBox.SelectionLength = 2;
                    _isUpdatingText = false;
                }
            }

            UpdateDateTime();
        }

        private void UpdateControls()
        {
            if (_datePicker == null)
            {
                return;
            }

            _isUpdatingText = true;

            _datePicker.SelectedDate = SelectedDateTime?.Date;

            if (ShowTime && SelectedDateTime.HasValue)
            {
                _hourTextBox?.Text = SelectedDateTime.Value.Hour.ToString("D2");
                _minuteTextBox?.Text = SelectedDateTime.Value.Minute.ToString("D2");
                _secondTextBox?.Text = SelectedDateTime.Value.Second.ToString("D2");
            }
            else if (ShowTime)
            {
                _hourTextBox?.Text = "00";
                _minuteTextBox?.Text = "00";
                _secondTextBox?.Text = "00";
            }

            _isUpdatingText = false;
        }

        private void UpdateDateTime()
        {
            if (_datePicker?.SelectedDate == null)
            {
                return;
            }

            int hour = 0, minute = 0, second = 0;

            if (ShowTime)
            {
                int.TryParse(_hourTextBox?.Text, out hour);
                int.TryParse(_minuteTextBox?.Text, out minute);
                int.TryParse(_secondTextBox?.Text, out second);

                hour = Math.Min(Math.Max(hour, 0), 23);
                minute = Math.Min(Math.Max(minute, 0), 59);
                second = Math.Min(Math.Max(second, 0), 59);
            }

            try
            {
                var newDateTime = new DateTime(
                    _datePicker.SelectedDate.Value.Year,
                    _datePicker.SelectedDate.Value.Month,
                    _datePicker.SelectedDate.Value.Day,
                    hour, minute, second);

                if (SelectedDateTime != newDateTime)
                {
                    SelectedDateTime = newDateTime;
                }
            }
            catch
            {
                // Invalid datetime
            }
        }
    }
}