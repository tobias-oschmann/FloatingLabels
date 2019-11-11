using System;
using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public class FloatingLabelCheckBox : FloatingLabelBase<CheckBox>
    {
        protected override int MarginTop
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android)
                    return -22;
                return base.MarginTop;
            }
        }


        public static readonly BindableProperty CheckedTextProperty =
           BindableProperty.Create(nameof(CheckedText), typeof(string), typeof(FloatingLabelCheckBox), string.Empty, BindingMode.OneWay, propertyChanged: (s, n, o) => (s as FloatingLabelCheckBox)._RefreshText());

        public string CheckedText
        {
            get => (string)GetValue(CheckedTextProperty);
            set => SetValue(CheckedTextProperty, value);
        }

        public static readonly BindableProperty UncheckedTextProperty =
            BindableProperty.Create(nameof(UncheckedText), typeof(string), typeof(FloatingLabelCheckBox), string.Empty, BindingMode.OneWay, propertyChanged: (s, n, o) => (s as FloatingLabelCheckBox)._RefreshText());

        public string UncheckedText
        {
            get => (string)GetValue(UncheckedTextProperty);
            set => SetValue(UncheckedTextProperty, value);
        }

        protected override BindableProperty ValueBindingProperty => CheckBox.IsCheckedProperty;
        protected override bool DisplayLabelInside(object value) => false;

        private readonly Label ctrlCheckBoxText;

        public FloatingLabelCheckBox()
        {
            ctrlContent.CheckedChanged += _OnCheckedChanged;

            Grid.ColumnDefinitions.Clear();
            Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(26, GridUnitType.Absolute) });
            Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            ctrlLabel.SetValue(Grid.ColumnProperty, 0);
            ctrlLabel.SetValue(Grid.ColumnSpanProperty, 2);
            ctrlContent.SetValue(Grid.ColumnProperty, 0);
            ctrlContent.SetValue(Grid.ColumnSpanProperty, 2);
            ctrlContent.HorizontalOptions = LayoutOptions.Fill;

            ctrlCheckBoxText = new Label();
            ctrlCheckBoxText.SetValue(Grid.ColumnProperty, 1);
            ctrlCheckBoxText.Margin = new Thickness(0, Math.Abs(MarginTop) + 6, 0, 0);
            ctrlCheckBoxText.InputTransparent = true;
            Grid.Children.Add(ctrlCheckBoxText);

            _OnCheckedChanged(this, new CheckedChangedEventArgs(ctrlContent.IsChecked));
        }

        protected override void OnTextColorChanged(Color oldValue, Color newValue)
        {
            ctrlCheckBoxText.TextColor = newValue;
        }

        private void _OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            _RefreshText();
        }

        private void _RefreshText()
        {
            ctrlCheckBoxText.Text = ctrlContent.IsChecked ? CheckedText : UncheckedText;
        }
    }
}
