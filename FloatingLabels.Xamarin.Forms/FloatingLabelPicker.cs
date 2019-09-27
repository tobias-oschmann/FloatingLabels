using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public class FloatingLabelPicker : FloatingLabelBase<Picker>
    {
        public static readonly BindableProperty ItemsSourceProperty =
           BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(FloatingLabelPicker), null, BindingMode.OneWay);

        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public IList<string> Items
        {
            get => ctrlContent.Items;
            set
            {
                ctrlContent.Items.Clear();
                if (value == null)
                    return;
                foreach (var val in value)
                    ctrlContent.Items.Add(val);
            }
        }

        protected override BindableProperty ValueBindingProperty => Picker.SelectedItemProperty;

        public FloatingLabelPicker()
        {
            ctrlContent.SetBinding(Picker.ItemsSourceProperty, new Binding() { Path = nameof(ItemsSource), Source = this });
        }
    }
}
