using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public abstract class FloatingLabelInputView<TContentView> : FloatingLabelBase<TContentView> 
        where TContentView : InputView, new()
    {

        public static readonly BindableProperty IsReadOnlyProperty =
                BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(FloatingLabelInputView<TContentView>), defaultValue: false, defaultBindingMode: BindingMode.OneWay);

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }


        public static readonly BindableProperty IsSpellCheckEnabledProperty =
                BindableProperty.Create(nameof(IsSpellCheckEnabled), typeof(bool), typeof(FloatingLabelInputView<TContentView>), defaultValue: true, defaultBindingMode: BindingMode.OneWay);

        public bool IsSpellCheckEnabled
        {
            get => (bool)GetValue(IsSpellCheckEnabledProperty);
            set => SetValue(IsSpellCheckEnabledProperty, value);
        }

        public static readonly BindableProperty MaxLengthProperty =
                BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(FloatingLabelInputView<TContentView>), defaultValue: int.MaxValue, defaultBindingMode: BindingMode.OneWay);

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }


        public static readonly BindableProperty KeyboardProperty =
                BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(FloatingLabelInputView<TContentView>), defaultValue: Keyboard.Default, defaultBindingMode: BindingMode.OneWay);

        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        public FloatingLabelInputView()
        {
            ctrlContent.SetBinding(InputView.IsReadOnlyProperty, new Binding() { Path = nameof(IsReadOnly), Source = this, });
            ctrlContent.SetBinding(InputView.IsSpellCheckEnabledProperty, new Binding() { Path = nameof(IsSpellCheckEnabled), Source = this, });
            ctrlContent.SetBinding(InputView.MaxLengthProperty, new Binding() { Path = nameof(MaxLength), Source = this, });
            ctrlContent.SetBinding(InputView.KeyboardProperty, new Binding() { Path = nameof(Keyboard), Source = this, });
        }
    }
}
