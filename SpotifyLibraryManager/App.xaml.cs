using SpotifyLibraryManager.Models;
using System;
using System.Windows;

namespace SpotifyLibraryManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Spotify.Instance = new Spotify();
        }
    }
}
