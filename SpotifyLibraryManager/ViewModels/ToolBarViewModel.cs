using SpotifyAPI.Web;
using SpotifyLibraryManager.Helpers;
using SpotifyLibraryManager.Models;
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
        private void Logout(object obj)
        {
            Spotify.Instance.Logout();
        }
        private bool CanManipulateAlbums(object obj)
        {
            if (!Albums.AreAlbumsLoaded) return false;
            else return true;
        }
        private bool CanLogout(object obj)
        {
            if (Spotify.Client == null || !Albums.AreAlbumsLoaded) return false;
            else return true;
        }
        public ToolBarViewModel()
        {
            Spotify.Instance.UserChanged += () =>
            {
                User = Spotify.Instance.User;
                ToggleMenusAvailability();
            };

            SortCommand = new RelayCommand(SortAlbums, CanManipulateAlbums);
            ChangeSortDirectionCommand = new RelayCommand(ChangeSortDirection, CanManipulateAlbums);
            SearchCommand = new RelayCommand(SearchAlbums, CanManipulateAlbums);
            LogoutCommand = new RelayCommand(Logout, CanLogout);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
