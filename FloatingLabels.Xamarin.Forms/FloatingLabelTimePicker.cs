using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public class FloatingLabelTimePicker : FloatingLabelBase<TimePicker>
    {
        public static readonly BindableProperty FormatProperty =
                   BindableProperty.Create(nameof(Format), typeof(string), typeof(FloatingLabelTimePicker), defaultValue: null, defaultBindingMode: BindingMode.OneWay);

        public string Format
        {
            get => (string)GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }
        protected override BindableProperty ValueBindingProperty => DatePicker.DateProperty;

        public FloatingLabelTimePicker()
        {
            ctrlContent.SetBinding(TimePicker.FormatProperty, new Binding() { Path = nameof(Format), Source = this, });
        }

        protected override bool DisplayLabelInside(object value) => false;
    }
}
