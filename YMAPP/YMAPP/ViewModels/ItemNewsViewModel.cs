using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using YMAPP.Models;

namespace YMAPP.ViewModels
{
    public class ItemNewsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ListNewsViewModel lvm;

        public ItemNews ItemNews { get; private set; }

        public ItemNewsViewModel()
        {
            ItemNews = new ItemNews();
        }

        public ListNewsViewModel ListViewModel
        {
            get { return lvm; }
            set
            {
                if (lvm != value)
                {
                    lvm = value;
                    OnPropertyChanged("ListViewModel");
                }
            }
        }
        public string Name
        {
            get { return ItemNews.Name; }
            set
            {
                if (ItemNews.Name != value)
                {
                    ItemNews.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public string Date
        {
            get { return ItemNews.Date; }
            set
            {
                if (ItemNews.Date != value)
                {
                    ItemNews.Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }
        public string Author
        {
            get { return ItemNews.Author; }
            set
            {
                if (ItemNews.Author != value)
                {
                    ItemNews.Author = value;
                    OnPropertyChanged("Author");
                }
            }
        }
        public string MinText
        {
            get { return ItemNews.MinText; }
            set
            {
                if (ItemNews.MinText != value)
                {
                    ItemNews.MinText = value;
                    OnPropertyChanged("MinText");
                }
            }
        }

        public string Image
        {
            get { return ItemNews.Image; }
            set
            {
                if (ItemNews.Image != value)
                {
                    ItemNews.Image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        public string FullTextHtml
        {
            get { return ItemNews.FullTextHtml; }
            set
            {
                if (ItemNews.FullTextHtml != value)
                {
                    ItemNews.FullTextHtml = value;
                    OnPropertyChanged("FullTextHtml");
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return ((!string.IsNullOrEmpty(Name.Trim())) ||
                    (!string.IsNullOrEmpty(Author.Trim())) ||
                    (!string.IsNullOrEmpty(MinText.Trim())));
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
