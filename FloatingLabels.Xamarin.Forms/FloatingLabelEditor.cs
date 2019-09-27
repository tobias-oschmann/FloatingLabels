using System;
using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public class FloatingLabelEditor : FloatingLabelBase<FloatingLabelEditor.BetterAutoSizingEditor>
    {
        public class BetterAutoSizingEditor : Editor
        {
            public static readonly BindableProperty MinimumRowsProperty =
                   BindableProperty.Create(nameof(MinimumRows), typeof(int), typeof(BetterAutoSizingEditor), 0, BindingMode.OneWay);

            public int MinimumRows { get; set; }

            protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
            {
                var fontCharHeightFactor = 1.6f;

                var sizeRequest = base.OnMeasure(widthConstraint, heightConstraint);
                if (MinimumHeightRequest == -1 && MinimumRows == 0)
                    return sizeRequest;
                if (MinimumRows > 0)
                    return new SizeRequest(new Size(sizeRequest.Request.Width, Math.Max((FontSize * fontCharHeightFactor) * MinimumRows, sizeRequest.Request.Height)));
                return new SizeRequest(new Size(sizeRequest.Request.Width, Math.Max(MinimumHeightRequest, sizeRequest.Request.Height)));
            }
        }

        public static readonly BindableProperty AutoSizeProperty =
            BindableProperty.Create(nameof(AutoSize), typeof(EditorAutoSizeOption), typeof(FloatingLabelEditor), EditorAutoSizeOption.Disabled, BindingMode.OneWay);

        public EditorAutoSizeOption AutoSize
        {
            get => (EditorAutoSizeOption)GetValue(AutoSizeProperty);
            set => SetValue(AutoSizeProperty, value);
        }

        public static readonly BindableProperty MinimumRowsProperty =
            BindableProperty.Create(nameof(MinimumRows), typeof(int), typeof(FloatingLabelEditor), 0, BindingMode.OneWay, propertyChanged: _OnMinimumRowsPropertyChanged);

        private static void _OnMinimumRowsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is FloatingLabelEditor editor))
                return;
            editor.ctrlContent.MinimumRows = (int)newValue;
        }

        public int MinimumRows { get; set; }

        public FloatingLabelEditor()
        {
            ctrlContent.SetBinding(Editor.AutoSizeProperty, new Binding() { Path = nameof(AutoSize), Source = this });
            ctrlContent.SetBinding(BetterAutoSizingEditor.MinimumRowsProperty, new Binding() { Path = nameof(MinimumRows), Source = this });
        }

        protected override BindableProperty ValueBindingProperty => Editor.TextProperty;
    }
}
