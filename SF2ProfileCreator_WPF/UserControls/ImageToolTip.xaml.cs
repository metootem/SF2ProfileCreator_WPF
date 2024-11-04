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
    /// Interaction logic for ImageToolTip.xaml
    /// </summary>
    public partial class ImageToolTip : UserControl
    {
        public ImageToolTip()
        {
            InitializeComponent();
            DataContext = this;
        }

        private Uri imageSource;
        private string header = string.Empty;

        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        public required Uri ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
            }
        }
    }
}
