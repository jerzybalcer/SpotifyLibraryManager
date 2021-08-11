using SpotifyLibraryManager.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace SpotifyLibraryManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Startup();
        }

        private async void Startup()
        {
            var isLoggedIn = await Spotify.Instance.CheckClient();
            if (isLoggedIn)
            {
                ContentFrame.Navigate(new Uri("/Views/Pages/ContentPage.xaml", UriKind.Relative));
                Albums.DownloadAllAlbums();
            }
            else
            {
                ContentFrame.Navigate(new Uri("/Views/Pages/LoginPage.xaml", UriKind.Relative));
            }
        }

        /* Window Controls */
        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void FullScreenBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleMaximized();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                ToggleMaximized();
            }

            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private static void ToggleMaximized()
        {
            if (App.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                App.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else if (App.Current.MainWindow.WindowState == WindowState.Normal)
            {
                App.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }
    }
}
