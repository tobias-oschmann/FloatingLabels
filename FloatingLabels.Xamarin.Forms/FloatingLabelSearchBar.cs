using System.Windows.Input;
using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public class FloatingLabelSearchBar : FloatingLabelBase<SearchBar>
    {
        public static readonly BindableProperty SearchCommandProperty =
            BindableProperty.Create(nameof(SearchCommand), typeof(ICommand), typeof(FloatingLabelEditor), null, BindingMode.OneWay);

        public ICommand SearchCommand
        {
            get => (ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }

        protected override BindableProperty ValueBindingProperty => SearchBar.TextProperty;

        protected override int PlaceholderMarginLeft => Device.RuntimePlatform == Device.Android ? 50 : base.PlaceholderMarginLeft;

        public FloatingLabelSearchBar()
        {
            ctrlContent.SetBinding(SearchBar.SearchCommandProperty, new Binding() { Path = nameof(SearchCommand), Source = this });
        }
    }
}
