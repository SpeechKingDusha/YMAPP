using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YMAPP.Models;

namespace YMAPP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            ItemNews.InitializeAsync();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
