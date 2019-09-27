using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public class FloatingLabelDatePicker : FloatingLabelBase<DatePicker>
    {
        protected override BindableProperty ValueBindingProperty => DatePicker.DateProperty;

        public FloatingLabelDatePicker()
        {
            ctrlContent.HorizontalOptions = LayoutOptions.Start;
        }

        protected override bool DisplayLabelInside(object value) => false;
    }
}
