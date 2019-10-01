using System;
using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public delegate void FloatingLabelValueChangedCallback(object sender, object oldValue, object newValue);

    public interface IFloatingLabelContent
    {
        event FloatingLabelValueChangedCallback ValueChanged;
        event EventHandler<FocusEventArgs> Focused;
        event EventHandler<FocusEventArgs> Unfocused;

        bool DisplayLabelInside(object value);

        object Value { get; set; }
    }

    public interface IFloatingLabelPlaceholderMarginLeft
    {
        int PlaceholderMarginLeft { get; }
    }

    public class FloatingLabelContentView : FloatingLabelBase<ContentView>
    {
        public static readonly new BindableProperty ContentProperty =
           BindableProperty.Create(nameof(Content), typeof(View), typeof(FloatingLabelContentView), null, BindingMode.OneWay, propertyChanged: _OnContentChanged);

        private static void _OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is FloatingLabelContentView cv))
                return;
            if (oldValue is IFloatingLabelContent o)
            {
                o.ValueChanged -= cv._OnValueChanged;
                o.Focused -= cv._OnFocusChanged;
                o.Unfocused -= cv._OnFocusChanged;
            }

            if (newValue is IFloatingLabelContent n)
            {
                n.ValueChanged += cv._OnValueChanged;
                n.Focused += cv._OnFocusChanged;
                n.Unfocused += cv._OnFocusChanged;
                cv.Value = n.Value;
            }
            cv.RefreshLabelPosition(true);
        }

        public new View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        protected override BindableProperty ValueBindingProperty => LabelProperty;
        protected override bool DisplayLabelInside(object value) => (!Content?.IsFocused ?? false) && (_ContentAsInterface?.DisplayLabelInside(value) ?? false);

        protected override int PlaceholderMarginLeft => _ContentAsMarginLeft?.PlaceholderMarginLeft ?? base.PlaceholderMarginLeft;

        public FloatingLabelContentView()
        {
            ctrlContent.SetBinding(ContentView.ContentProperty, new Binding() { Path = nameof(Content), Source = this });
        }

        private IFloatingLabelContent _ContentAsInterface => (Content as IFloatingLabelContent);
        private IFloatingLabelPlaceholderMarginLeft _ContentAsMarginLeft => (Content as IFloatingLabelPlaceholderMarginLeft);

        private void _OnValueChanged(object sender, object oldValue, object newValue)
        {
            Value = newValue;
        }

        private void _OnFocusChanged(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
                _OnFocused(sender, e);
            else
                _OnUnfocused(sender, e);
        }
    }
}
