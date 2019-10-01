﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms
{
    public abstract class FloatingLabelBase<TContentView> : ContentView where TContentView : View, new()
    {
        protected int PlaceholderFontSize
        {
            get
            {
                if (Device.RuntimePlatform == Device.UWP)
                    return TitleFontSize;
                return 16;
            }
        }
        protected int TitleFontSize
        {
            get
            {
                return 14;
            }
        }

        protected int MarginTop
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android)
                    return -16;
                return -28;
            }
        }

        protected virtual int PlaceholderMarginLeft => 10;

        protected readonly Label ctrlLabel;
        protected readonly TContentView ctrlContent;
        protected Grid Grid => Content as Grid;

        #region Bindable Properties

        public static readonly BindableProperty LabelProperty =
           BindableProperty.Create(nameof(Label), typeof(string), typeof(FloatingLabelBase<TContentView>), string.Empty, BindingMode.OneWay, null);

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(object), typeof(FloatingLabelBase<TContentView>), string.Empty, BindingMode.TwoWay, null, _OnValuePropertyBindingChanged);

        private static async void _OnValuePropertyBindingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as FloatingLabelBase<TContentView>;
            if (!control.ctrlContent.IsFocused)
            {
                if (!control.DisplayLabelInside(newValue))
                    await control._TransitionToAbove(false);
                else
                    await control._TransitionToInside(false);
            }
        }

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly BindableProperty LabelColorProperty =
            BindableProperty.Create(nameof(LabelColor), typeof(Color), typeof(FloatingLabelBase<TContentView>), defaultValue: Color.Default, defaultBindingMode: BindingMode.OneWay);

        public Color LabelColor
        {
            get => (Color)GetValue(LabelColorProperty);
            set => SetValue(LabelColorProperty, value);
        }

        private static readonly BindableProperty HorizontalContentOptionsProperty =
            BindableProperty.Create(nameof(HorizontalContentOptions), typeof(LayoutOptions), typeof(FloatingLabelBase<TContentView>), defaultValue: LayoutOptions.Fill, defaultBindingMode: BindingMode.OneWay);

        public LayoutOptions HorizontalContentOptions
        {
            get => (LayoutOptions)GetValue(HorizontalContentOptionsProperty);
            set => SetValue(HorizontalContentOptionsProperty, value);
        }

        private static readonly BindableProperty VerticalContentOptionsProperty =
                BindableProperty.Create(nameof(VerticalContentOptions), typeof(LayoutOptions), typeof(FloatingLabelBase<TContentView>), defaultValue: LayoutOptions.Fill, defaultBindingMode: BindingMode.OneWay);

        public LayoutOptions VerticalContentOptions
        {
            get => (LayoutOptions)GetValue(VerticalContentOptionsProperty);
            set => SetValue(VerticalContentOptionsProperty, value);
        }



        #endregion Bindable Properties

        public FloatingLabelBase()
        {
            var grid = new Grid {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill
            };
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Content = grid;

            ctrlContent = new TContentView() {
                Margin = new Thickness(0, Math.Abs(MarginTop), 0, 0),
            };
            ctrlLabel = new Label() {
                Margin = new Thickness(0, Math.Abs(MarginTop) + 2, 0, 0),
                InputTransparent = true,
            };
            grid.Children.Add(ctrlContent);
            grid.Children.Add(ctrlLabel);

            ctrlContent.SetValue(Grid.RowProperty, 0);
            ctrlContent.SetValue(Grid.ColumnProperty, 0);

            ctrlLabel.SetValue(Grid.RowProperty, 0);
            ctrlLabel.SetValue(Grid.ColumnProperty, 0);

            ctrlContent.SetBinding(ValueBindingProperty, new Binding() { Path = nameof(Value), Source = this, Mode = BindingMode.TwoWay, });
            ctrlContent.SetBinding(MinimumHeightRequestProperty, new Binding() { Path = nameof(MinimumHeightRequest), Source = this });
            ctrlContent.SetBinding(HorizontalOptionsProperty, new Binding() { Path = nameof(HorizontalContentOptions), Source = this, });
            ctrlContent.SetBinding(VerticalOptionsProperty, new Binding() { Path = nameof(VerticalContentOptions), Source = this, });
            ctrlLabel.SetBinding(global::Xamarin.Forms.Label.TextProperty, new Binding() { Path = nameof(Label), Source = this });
            ctrlLabel.SetBinding(global::Xamarin.Forms.Label.TextColorProperty, new Binding() { Path = nameof(LabelColor), Source = this });

            ctrlContent.Focused += _OnFocused;
            ctrlContent.Unfocused += _OnUnfocused;

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += _OnLabelTapped;
            ctrlLabel.GestureRecognizers.Add(tapGesture);

            RefreshLabelPosition(false);
        }

        ~FloatingLabelBase()
        {
            ctrlContent.Focused -= _OnFocused;
            ctrlContent.Unfocused -= _OnUnfocused;
        }

        protected abstract BindableProperty ValueBindingProperty { get; }

        public new void Focus()
        {
            if (IsEnabled)
                ctrlContent.Focus();
        }
        protected virtual bool DisplayLabelInside(object value)
        {
            if (value == null)
                return true;
            if (value is string && string.IsNullOrWhiteSpace((string)value))
                return true;
            return false;
        }

        protected async void _OnFocused(object sender, FocusEventArgs e)
        {
            //if (DisplayLabelInside(Value))
                await _TransitionToAbove(true);
        }

        protected async void _OnUnfocused(object sender, FocusEventArgs e)
        {
            if (DisplayLabelInside(Value))
                await _TransitionToInside(true);
        }

        private void _OnLabelTapped(object sender, EventArgs e)
        {
            Focus();
        }

        public void RefreshLabelPosition(bool animated)
        {
            if (DisplayLabelInside(Value))
                _TransitionToInside(false).Wait();
            else
                _TransitionToAbove(false).Wait();
        }

        private async Task _TransitionToAbove(bool animated)
        {
            if (animated)
            {
                var t1 = ctrlLabel.TranslateTo(0, MarginTop, 100);
                var t2 = _SizeTo(TitleFontSize);
                await Task.WhenAll(t1, t2);
            }
            else
            {
                ctrlLabel.TranslationX = 0;
                ctrlLabel.TranslationY = MarginTop;
                ctrlLabel.FontSize = TitleFontSize;
            }
            ctrlLabel.Opacity = 1;
            ctrlLabel.InputTransparent = false;
        }

        private async Task _TransitionToInside(bool animated)
        {
            //animated = true;
            if (animated)
            {
                var t1 = ctrlLabel.TranslateTo(PlaceholderMarginLeft, 0, 100);
                var t2 = _SizeTo(PlaceholderFontSize);
                await Task.WhenAll(t1, t2);
            }
            else
            {
                ctrlLabel.TranslationX = PlaceholderMarginLeft;
                ctrlLabel.TranslationY = 0;
                ctrlLabel.FontSize = PlaceholderFontSize;
            }
            ctrlLabel.Opacity = 0.5;
            ctrlLabel.InputTransparent = true;
        }

        private Task _SizeTo(int fontSize)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            // setup information for animation
            void callback(double input) { ctrlLabel.FontSize = input; }
            double startingHeight = ctrlLabel.FontSize;
            double endingHeight = fontSize;
            uint rate = 5;
            uint length = 100;
            var easing = Easing.Linear;

            // now start animation with all the setup information
            ctrlLabel.Animate("invis", callback, startingHeight, endingHeight, rate, length, easing, (v, c) => taskCompletionSource.SetResult(c));

            return taskCompletionSource.Task;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(IsEnabled))
            {
                ctrlContent.IsEnabled = IsEnabled;
            }
        }
    }
}
