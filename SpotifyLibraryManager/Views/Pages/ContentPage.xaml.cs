using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpotifyLibraryManager
{
    /// <summary>
    /// Interaction logic for ContentPage.xaml
    /// </summary>
    public partial class ContentPage : Page
    {
        public ContentPage()
        {
            InitializeComponent();
        }

        private void Title_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var txt = sender as TextBlock;

            if (txt.Text.Length == 0) OpenWithSpotifyBtn.Visibility = Visibility.Collapsed;
            else OpenWithSpotifyBtn.Visibility = Visibility.Visible;
        }
    }
}
