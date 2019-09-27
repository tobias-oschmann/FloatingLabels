using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public class FloatingLabelTimePicker : FloatingLabelBase<TimePicker>
    {
        protected override BindableProperty ValueBindingProperty => DatePicker.DateProperty;

        public FloatingLabelTimePicker()
        {
            ctrlContent.HorizontalOptions = LayoutOptions.Start;
        }

        protected override bool DisplayLabelInside(object value) => false;
    }
}
