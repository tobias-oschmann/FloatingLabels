using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public class FloatingLabelDatePicker : FloatingLabelBase<DatePicker>
    {
        private static readonly BindableProperty FormatProperty =
                BindableProperty.Create(nameof(Format), typeof(string), typeof(FloatingLabelDatePicker), defaultValue: null, defaultBindingMode: BindingMode.OneWay);

        public string Format
        {
            get => (string)GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }


        protected override BindableProperty ValueBindingProperty => DatePicker.DateProperty;

        public FloatingLabelDatePicker()
        {
            ctrlContent.SetBinding(DatePicker.FormatProperty, new Binding() { Path = nameof(Format), Source = this, });
        }

        protected override bool DisplayLabelInside(object value) => false;
    }
}
