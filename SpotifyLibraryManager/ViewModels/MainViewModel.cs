using SpotifyAPI.Web;
using SpotifyLibraryManager.Helpers;
using SpotifyLibraryManager.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpotifyLibraryManager
{
    public class MainViewModel : INotifyPropertyChanged // divide this class into some child classes (ToolBarViewModel, ContentViewModel, DetailsViewModel)
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

        private string selectedTitle;
        private string selectedArtist;
        private string selectedReleaseDate;
        private DateTime selectedAddedAt;
        private int selectedTotalTracks;

        public string SelectedTitle
        {
            get { return selectedTitle; }
            set { selectedTitle = value; OnPropertyChanged(); }
        }
        public string SelectedArtist { get => selectedArtist; set { selectedArtist = value; OnPropertyChanged(); } }
        public string SelectedReleaseDate { get => selectedReleaseDate; set { selectedReleaseDate = value; OnPropertyChanged(); } }
        public DateTime SelectedAddedAt { get => selectedAddedAt; set { selectedAddedAt = value; OnPropertyChanged(); } }
        public int SelectedTotalTracks { get => selectedTotalTracks; set { selectedTotalTracks = value; OnPropertyChanged(); } }

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

        public static ObservableCollection<SavedAlbum> VisibleAlbums { get; set; } = Albums.AvailableAlbums;
        public RelayCommand SortCommand { get; private set; }
        public RelayCommand ChangeSortDirectionCommand { get; private set; }
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }
        public RelayCommand SelectAlbumCommand { get; private set; }
        public MainViewModel()
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
            SelectAlbumCommand = new RelayCommand(SelectAlbum, null);
        }
        private void Logout(object obj)
        {
            Spotify.Instance.Logout();
        }
        private void SelectAlbum(object obj)
        {
            var selectedAlbum = obj as SavedAlbum;
            SelectedTitle = selectedAlbum.Album.Name;
            SelectedArtist = selectedAlbum.Album.Artists[0].Name;
            SelectedReleaseDate = selectedAlbum.Album.ReleaseDate;
            SelectedAddedAt = selectedAlbum.AddedAt;
            SelectedTotalTracks = selectedAlbum.Album.TotalTracks;
        }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
