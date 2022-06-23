using System.ComponentModel;
using Xamarin.Forms;
using YMAPP.ViewModels;

namespace YMAPP.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}