using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyLibraryManager.Models
{
    public delegate void AlbumsLoadedChangedHandler();
    public static class Albums
    {
        private static bool _sortAscending = false;
        private static int _total = 51;
        private static string _sortBy = "Added At";

        public static string SortBy
        {
            get { return _sortBy; }
            set { _sortBy = value; }
        }

        public static event AlbumsLoadedChangedHandler AlbumsLoadedChanged;
        public static void OnAlbumsLoadedChanged()
        {
            AlbumsLoadedChanged?.Invoke();
        }
        private static List<SavedAlbum> AllAlbums { get; set; } = new List<SavedAlbum>();
        public static ObservableCollection<SavedAlbum> AvailableAlbums { get; set; } = new ObservableCollection<SavedAlbum>();

        public static async Task DownloadAllAlbums()
        {
            AvailableAlbums.Clear();
            OnAlbumsLoadedChanged();

            while (AvailableAlbums.Count < _total)
            {
                try
                {
                    await Paginate();
                }
                catch
                {
                    await Task.Delay(4000);
                }
            }
            AllAlbums = AvailableAlbums.ToList();
            OnAlbumsLoadedChanged();
        }

        private static async Task Paginate()
        {
            var page = await Spotify.Client.Library.GetAlbums(new LibraryAlbumsRequest { Limit = 50, Offset = 0 });
            _total = (int)page.Total;

            while (page.Next != null)
            {
                page = await Spotify.Client.Library.GetAlbums(new LibraryAlbumsRequest { Limit = 50, Offset = AvailableAlbums.Count });
                foreach (var item in page.Items) AvailableAlbums.Add(item);
                
                await Task.Delay(1000);
            }
        }

        public static void ChangeSortDirection()
        {
            if (_sortAscending) _sortAscending = false;
            else _sortAscending = true;
            Sort(SortBy);
        }

        public static void Sort(string sortBy)
        {
            sortBy = sortBy.Replace(" ", "");
            SortBy = sortBy;

            Func<SavedAlbum, object> keySelector;

            if (sortBy == "Artist")
            {
                keySelector = (x) => x.Album.Artists[0].Name;
            }
            else if (sortBy == "AddedAt")
            {
                keySelector = (x) => x.AddedAt;
            }
            else
            {
                keySelector = (x) => x.Album.GetType().GetProperty(sortBy).GetValue(x.Album, null);
            }

            List<SavedAlbum> sorted = new();

            if (_sortAscending)
                sorted = AvailableAlbums.OrderBy(keySelector).ToList();
            else
                sorted = AvailableAlbums.OrderByDescending(keySelector).ToList();

            AvailableAlbums.Clear();

            foreach (var item in sorted) AvailableAlbums.Add(item);
        }

        public static void Search(string searchingFor)
        {
            AvailableAlbums.Clear();

            foreach (var item in AllAlbums)
            {
                if (item.Album.Name.ToLower().Contains(searchingFor.ToLower()) ||
                    item.Album.Artists.Any(artist => artist.Name.ToLower().Contains(searchingFor.ToLower())))
                    AvailableAlbums.Add(item);
            }
            Sort(SortBy);
        }
        public static void ClearOnLogout()
        {
            AvailableAlbums.Clear();
            AllAlbums.Clear();
            _total = 51;
            _sortBy = "Added At";
        }
    }
}
