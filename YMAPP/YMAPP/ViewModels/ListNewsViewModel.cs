using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using YMAPP.Services;
using YMAPP.Views;

namespace YMAPP.ViewModels
{
    public class ListNewsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ItemNewsViewModel> ListNews { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CreateNewsCommand { protected set; get; }
        public ICommand DeleteNewsCommand { protected set; get; }
        public ICommand SaveNewsCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        ItemNewsViewModel selectedNews;

        public INavigation Navigation { get; set; }

        public ListNewsViewModel()
        {
            ParserNews.Initialize();
            ListNews = new ObservableCollection<ItemNewsViewModel>();

            foreach (var item in ParserNews.ListAllNews)
            {
                ItemNewsViewModel itemNews = new ItemNewsViewModel();
                itemNews.Name = item.Name;
                itemNews.Date = item.Date;
                itemNews.Author = item.Author;
                itemNews.MinText = item.MinText;
                itemNews.Image = item.Image;
                ListNews.Add(itemNews);
            }


            CreateNewsCommand = new Command(CreateNews);
            DeleteNewsCommand = new Command(DeleteNews);
            SaveNewsCommand = new Command(SaveNews);
            BackCommand = new Command(Back);
        }

        public ItemNewsViewModel SelectedNews
        {
            get { return selectedNews; }
            set
            {
                if (selectedNews != value)
                {
                    ItemNewsViewModel itemNews = value;
                    selectedNews = null;
                    OnPropertyChanged("SelectedNews");
                    Navigation.PushAsync(new ItemNewsPage(itemNews));
                }
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void CreateNews()
        {
            Navigation.PushAsync(new ItemNewsPage(new ItemNewsViewModel() { ListViewModel = this }));
        }
        private void Back()
        {
            Navigation.PopAsync();
        }
        private void SaveNews(object newsObject)
        {
            ItemNewsViewModel itemNews = newsObject as ItemNewsViewModel;
            if (itemNews != null && itemNews.IsValid && !ListNews.Contains(itemNews))
            {
                ListNews.Add(itemNews);
            }
            Back();
        }
        private void DeleteNews(object newsObject)
        {
            ItemNewsViewModel itemNews = newsObject as ItemNewsViewModel;
            if (itemNews != null)
            {
                ListNews.Remove(itemNews);
            }
            Back();
        }
    }
}
