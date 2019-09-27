using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public class FloatingLabelEntry : FloatingLabelBase<Entry>
    {
        protected override BindableProperty ValueBindingProperty => Entry.TextProperty;
    }
}
