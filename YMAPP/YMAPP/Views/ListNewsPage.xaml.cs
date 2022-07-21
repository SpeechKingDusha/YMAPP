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
    public partial class ListNewsPage : ContentPage
    {
        public ListNewsPage()
        {
            InitializeComponent();
            BindingContext = new ListNewsViewModel() { Navigation = this.Navigation };
        }
    }
}