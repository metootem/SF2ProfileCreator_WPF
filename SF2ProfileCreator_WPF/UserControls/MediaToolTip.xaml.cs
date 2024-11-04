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

namespace SF2ProfileCreator_WPF.UserControls
{
    /// <summary>
    /// Interaction logic for MediaToolTip.xaml
    /// </summary>
    public partial class MediaToolTip : UserControl
    {
        public MediaToolTip()
        {
            InitializeComponent();
            DataContext = this;
        }

        private Uri mediaSource;
        private string header = string.Empty;

        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        public required Uri MediaSource
        {
            get { return mediaSource; }
            set
            {
                mediaSource = value;
            }
        }

        private void PreviewMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            PreviewMedia.Position = TimeSpan.Zero;
        }
    }
}
