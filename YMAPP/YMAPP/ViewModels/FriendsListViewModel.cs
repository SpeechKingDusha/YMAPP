using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using YMAPP.Services;
using YMAPP.Views;

namespace YMAPP.ViewModels
{
    public class FriendsListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FriendViewModel> Friends { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateFriendCommand { protected set; get; }
        public ICommand DeleteFriendCommand { protected set; get; }
        public ICommand SaveFriendCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }
        FriendViewModel selectedFriend;

        public INavigation Navigation { get; set; }

        public FriendsListViewModel()
        {
            ParserNews.Initialize();
            Friends = new ObservableCollection<FriendViewModel>();

            foreach (var item in ParserNews.ListAllNews)
            {
                FriendViewModel tempFriend = new FriendViewModel();
                tempFriend.Name = item.Name;
                tempFriend.Email = item.Author;
                tempFriend.Phone = item.MinText;
                Friends.Add(tempFriend);
            }


            CreateFriendCommand = new Command(CreateFriend);
            DeleteFriendCommand = new Command(DeleteFriend);
            SaveFriendCommand = new Command(SaveFriend);
            BackCommand = new Command(Back);
        }

        public FriendViewModel SelectedFriend
        {
            get { return selectedFriend; }
            set
            {
                if (selectedFriend != value)
                {
                    FriendViewModel tempFriend = value;
                    selectedFriend = null;
                    OnPropertyChanged("SelectedFriend");
                    Navigation.PushAsync(new FriendPage(tempFriend));
                }
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void CreateFriend()
        {
            Navigation.PushAsync(new FriendPage(new FriendViewModel() { ListViewModel = this }));
        }
        private void Back()
        {
            Navigation.PopAsync();
        }
        private void SaveFriend(object friendObject)
        {
            FriendViewModel friend = friendObject as FriendViewModel;
            if (friend != null && friend.IsValid && !Friends.Contains(friend))
            {
                Friends.Add(friend);
            }
            Back();
        }
        private void DeleteFriend(object friendObject)
        {
            FriendViewModel friend = friendObject as FriendViewModel;
            if (friend != null)
            {
                Friends.Remove(friend);
            }
            Back();
        }
    }
}
