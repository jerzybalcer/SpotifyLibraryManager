using SpotifyAPI.Web;
using SpotifyLibraryManager.Helpers;
using SpotifyLibraryManager.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace SpotifyLibraryManager.ViewModels
{
    public class ContentViewModel : INotifyPropertyChanged
    {
        public static ObservableCollection<SavedAlbum> VisibleAlbums { get; set; } = Albums.AvailableAlbums;
        public ObservableCollection<SimpleTrack> SelectedTracks { get; set; } = new ObservableCollection<SimpleTrack>();

        private string _selectedTitle;
        private string _selectedArtist;
        private string _selectedReleaseDate;
        private string _selectedAddedAt;
        private string _selectedTotalTracks;
        private string _selectedDuration;
        private string _selectedUri;
        public string SelectedTitle
        {
            get { return _selectedTitle; }
            set { _selectedTitle = value; OnPropertyChanged(); }
        }
        public string SelectedArtist
        {
            get { return _selectedArtist; }
            set { _selectedArtist = value; OnPropertyChanged(); }
        }
        public string SelectedReleaseDate
        {
            get {
                if (_selectedReleaseDate == null) return "";
                else
                {
                    return TimeToStringConverter.ToLongDateString(_selectedReleaseDate, "Released");
                }
            }
            set { _selectedReleaseDate = value; OnPropertyChanged(); }
        }
        public string SelectedAddedAt
        {
            get {
                if (_selectedAddedAt == null) return "";
                else
                {
                    return TimeToStringConverter.ToLongDateString(_selectedAddedAt, "Added");
                }
            }
            set { _selectedAddedAt = value; OnPropertyChanged(); }
        }
        public string SelectedTotalTracks
        {
            get {
                if (_selectedTotalTracks == null) return "";
                else return _selectedTotalTracks + " tracks"; }
            set { _selectedTotalTracks = value; OnPropertyChanged(); }
        }
        public string SelectedDuration
        {
            get
            {
                if (_selectedDuration == null) return "";
                else return ", " + _selectedDuration;
            }
            set { _selectedDuration = value; OnPropertyChanged(); }
        }

        public RelayCommand SelectAlbumCommand { get; private set; }
        private void SelectAlbum(object obj)
        {
            var selectedAlbum = obj as SavedAlbum;
            SelectedTitle = selectedAlbum.Album.Name;
            SelectedArtist = selectedAlbum.Album.Artists[0].Name;
            SelectedReleaseDate = selectedAlbum.Album.ReleaseDate;
            SelectedAddedAt = selectedAlbum.AddedAt.ToString();
            SelectedTotalTracks = selectedAlbum.Album.TotalTracks.ToString();
            _selectedUri = selectedAlbum.Album.Uri;

            int duration = 0;
            SelectedTracks.Clear();
            foreach (var track in selectedAlbum.Album.Tracks.Items)
            {
                duration += track.DurationMs;
                SelectedTracks.Add(track);
            }
            SelectedDuration = TimeToStringConverter.FromMiliseconds(duration);
        }
        public RelayCommand OpenWithSpotifyCommand { get; private set; }
        private void OpenWithSpotify(object obj)
        {
            try
            {
                Process.Start("spotify", "--uri=" + _selectedUri);
            }
            catch {
                var browser = new Process();
                browser.StartInfo.UseShellExecute = true;
                browser.StartInfo.FileName = "https://www.spotify.com/us/download/other/";
                browser.Start(); 
            }
        }

        public ContentViewModel()
        {
            SelectAlbumCommand = new RelayCommand(SelectAlbum, null);
            OpenWithSpotifyCommand = new RelayCommand(OpenWithSpotify, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
