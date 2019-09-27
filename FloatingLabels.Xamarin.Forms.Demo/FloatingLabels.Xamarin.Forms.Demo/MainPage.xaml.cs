using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FloatingLabels.Xamarin.Forms.Demo
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public Command SearchCommand { get; } = new Command(_OnSearch);

        private string _pickerValue;

        public string PickerValue
        {
            get => _pickerValue;
            set
            {
                _pickerValue = value;
                OnPropertyChanged();
            }
        }


        public MainPage()
        {
            InitializeComponent();
            PickerValue = "Red";
        }

        private static void _OnSearch(object obj)
        {
            Application.Current.MainPage.DisplayAlert("Info", "Start search now!", "OK");
        }
    }
}
