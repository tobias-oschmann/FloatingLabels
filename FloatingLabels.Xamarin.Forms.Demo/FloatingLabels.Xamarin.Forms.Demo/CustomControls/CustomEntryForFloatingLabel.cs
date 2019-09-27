using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms.Demo.CustomControls
{
    public class CustomEntryForFloatingLabel : Entry, IFloatingLabelContent
    {
        public object Value { get => Text; set => Text = value.ToString(); }

        public CustomEntryForFloatingLabel()
        {
            TextChanged += _OnTextChanged;
        }

        private void _OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e.OldTextValue, e.NewTextValue);
        }

        public event FloatingLabelValueChangedCallback ValueChanged;

        public bool DisplayLabelInside(object value) => string.IsNullOrWhiteSpace(value as string);
    }
}
