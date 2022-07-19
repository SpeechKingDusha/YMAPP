using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using YMAPP.Models;
using YMAPP.Services;

namespace YMAPP.ViewModels
{
    public class MainPageViewModel: INotifyPropertyChanged
    {
        private float progress;
        public float Progress
        {
            get { return progress; }
            private set
            {
                progress = value;
                OnPropertyChanged("ProgressState");
            }
        }
        public MainPageViewModel()
        {
            ParserNews.Initialize();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
