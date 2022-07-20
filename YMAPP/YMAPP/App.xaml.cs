using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YMAPP.Models;
using YMAPP.Services;
using YMAPP.Views;

namespace YMAPP
{
    public partial class App : Application
    {
        public App()

        {
            //ParserNews.Initialize();
            InitializeComponent();
            MainPage = new NavigationPage(new FriendsListPage());
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
