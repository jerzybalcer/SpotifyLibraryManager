using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SpotifyLibraryManager
{
    /// <summary>
    /// Interaction logic for AlbumCoverButton.xaml
    /// </summary>
    public partial class AlbumCoverButton : UserControl
    {
        public AlbumCoverButton()
        {
            InitializeComponent();

        }
        public AlbumCoverButton(string title, string artist, string imageUrl)
        {
            InitializeComponent();
            Title.Text = title;
            Artist.Text = artist;
            Cover.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(imageUrl);
        }

        private void UserControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
