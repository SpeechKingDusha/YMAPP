using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace YMAPP.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "Улица Московская";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://www.ym-penza.ru/"));
        }

        public ICommand OpenWebCommand { get; }
    }
}