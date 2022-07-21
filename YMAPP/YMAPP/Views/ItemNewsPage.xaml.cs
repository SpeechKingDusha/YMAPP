using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YMAPP.ViewModels;

namespace YMAPP.Views
{
    public partial class ItemNewsPage : ContentPage
    {
        public ItemNewsViewModel ViewModel { get; private set; }
        public ItemNewsPage (ItemNewsViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            this.BindingContext = ViewModel;
        }
    }
}