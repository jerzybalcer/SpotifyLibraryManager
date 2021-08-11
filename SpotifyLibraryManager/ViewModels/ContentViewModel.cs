using SpotifyAPI.Web;
using SpotifyLibraryManager.Helpers;
using SpotifyLibraryManager.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                    CultureInfo lang = new("en-US");

                    if (DateTime.TryParse(_selectedReleaseDate, out DateTime parsedReleaseDate))
                    {
                        return "Released at " + parsedReleaseDate.ToString(lang.DateTimeFormat.LongDatePattern, lang);
                    }
                    else { return "Released in " + _selectedReleaseDate; }
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
                    CultureInfo lang = new("en-US");

                    return "Added at " + DateTime.Parse(_selectedAddedAt).Date.ToString(lang.DateTimeFormat.LongDatePattern, lang);
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

        public RelayCommand SelectAlbumCommand { get; private set; }
        private void SelectAlbum(object obj)
        {
            var selectedAlbum = obj as SavedAlbum;
            SelectedTitle = selectedAlbum.Album.Name;
            SelectedArtist = selectedAlbum.Album.Artists[0].Name;
            SelectedReleaseDate = selectedAlbum.Album.ReleaseDate;
            SelectedAddedAt = selectedAlbum.AddedAt.ToString();
            SelectedTotalTracks = selectedAlbum.Album.TotalTracks.ToString();

            SelectedTracks.Clear();
            foreach (var track in selectedAlbum.Album.Tracks.Items)
                SelectedTracks.Add(track);
        }

        public ContentViewModel()
        {
            SelectAlbumCommand = new RelayCommand(SelectAlbum, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
