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
    /// Interaction logic for TabItemEx.xaml
    /// </summary>
    public partial class TabItemEx : UserControl
    {
        MainWindow _main;
        public TabItemEx(MainWindow main, string header)
        {
            InitializeComponent();
            _main = main;
            txtbckName.Text = header;
        }

        private void TabItemEx_Close(object sender, EventArgs e)
        {
            _main.CloseProfileTab(txtbckName.Text);
        }
    }
}
