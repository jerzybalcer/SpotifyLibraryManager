using SpotifyLibraryManager.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SpotifyLibraryManager
{
    /// <summary>
    /// Interaction logic for ToolBar.xaml
    /// </summary>
    public partial class ToolBar : UserControl
    {
        public ToolBar()
        {
            InitializeComponent();
            Albums.AlbumsLoadedChanged += () =>
            {
                ResetSortMenuChecks();
            };
        }

        // Toggle between ascending and descending order indicator
        private void SortDirectionChanger_Click(object sender, RoutedEventArgs e)
        {
            if (SortDirection.Text == "⋀") SortDirection.Text = "⋁";
            else SortDirection.Text = "⋀";
        }

        // Clear "Search" text on textbox focus
        private void SearchText_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SearchText.Text = "";
        }

        // More aesthetics
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchBtn.Focus();
        }

        private void MenuLogout_Click(object sender, RoutedEventArgs e)
        {
            var win = Application.Current.MainWindow as MainWindow;
            win.ContentFrame.Navigate(new Uri("/Views/Pages/LoginPage.xaml", UriKind.Relative));
        }

        private void ResetSortMenuChecks()
        {
            foreach (MenuItem item in SortTopItem.Items)
                item.IsChecked = false;

            SortByAddedAt.IsChecked = true;
        }

        private void Sort_Checked(object sender, RoutedEventArgs e)
        {
            var checkedItem = sender as MenuItem;

            foreach(MenuItem item in SortTopItem.Items)
            {
                if (item != checkedItem)
                    item.IsChecked = false;
            }
        }
    }
}
