using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public class FloatingLabelEntry : FloatingLabelInputView<Entry>
    {
        protected override BindableProperty ValueBindingProperty => Entry.TextProperty;

        public FloatingLabelEntry()
        {
        }
    }
}
