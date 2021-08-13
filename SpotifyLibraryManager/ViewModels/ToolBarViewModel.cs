using SpotifyAPI.Web;
using SpotifyLibraryManager.Helpers;
using SpotifyLibraryManager.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpotifyLibraryManager.ViewModels
{
    public class ToolBarViewModel : INotifyPropertyChanged
    {
        private PrivateUser _user;
        public PrivateUser User
        {
            get
            {
                if (_user == null)
                    return new PrivateUser { DisplayName = "Not logged in" };
                else return _user;
            }
            set { _user = value; OnPropertyChanged(); }
        }

        private bool _areMenusEnabled = false;
        public bool AreMenusEnabled
        {
            get { return _areMenusEnabled; }
            set { _areMenusEnabled = value; OnPropertyChanged(); }
        }
        private void ToggleMenusAvailability()
        {
            if (AreMenusEnabled) AreMenusEnabled = false;
            else AreMenusEnabled = true;
        }
        public RelayCommand SortCommand { get; private set; }
        public RelayCommand ChangeSortDirectionCommand { get; private set; }
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }

        private void SortAlbums(object obj)
        {
            Albums.Sort(obj as string);
        }
        private void ChangeSortDirection(object obj)
        {
            Albums.ChangeSortDirection();
        }
        private void SearchAlbums(object obj)
        {
            Albums.Search(obj as string);
        }
        private void RefreshAlbums(object obj)
        {
            Albums.DownloadAllAlbums();
        }
        private void Logout(object obj)
        {
            Spotify.Instance.Logout();
        }
        public ToolBarViewModel()
        {
            Spotify.Instance.UserChanged += () =>
            {
                User = Spotify.Instance.User;
                ToggleMenusAvailability();
            };

            Albums.AlbumsLoadedChanged += () => {
                ToggleMenusAvailability();
            };

            SortCommand = new RelayCommand(SortAlbums, null);
            ChangeSortDirectionCommand = new RelayCommand(ChangeSortDirection, null);
            SearchCommand = new RelayCommand(SearchAlbums, null);
            LogoutCommand = new RelayCommand(Logout, null);
            RefreshCommand = new RelayCommand(RefreshAlbums, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
