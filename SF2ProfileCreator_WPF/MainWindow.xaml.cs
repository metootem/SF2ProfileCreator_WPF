using Microsoft.Win32;
using System.IO;
using System.Windows;
using WinControls = System.Windows.Controls;

namespace SF2ProfileCreator_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnResize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else WindowState = WindowState.Maximized;
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void txtProfilesPath_TextChanged(object sender, WinControls.TextChangedEventArgs e)
        {
            if (txtProfilesPath.Text.Length < 1)
            {
                txtProfilesPathPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                txtProfilesPathPlaceholder.Visibility = Visibility.Collapsed;
                UpdatePacksView();
            }
        }
        private void btnProfilesPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new();
            dialog.ShowDialog();
            txtProfilesPath.Text = dialog.FolderName;
            UpdatePacksView();
            
        }
        private void profileButton_Click(object sender, EventArgs e)
        {
            /*
            if (sender is WinControls.Button btnFile)
                txtProfile.Text = btnFile.Tag.ToString();
            else txtProfile.Text = sender.GetType().FullName;
            */
        }

        private void UpdatePacksView()
        {
            
            if (tabProfiles != null)
            {
                tabProfiles.Items.Clear();
                string path = txtProfilesPath.Text;
                if (Directory.Exists(path))
                {
                    if (Path.GetFileNameWithoutExtension(path) == "packs")
                    {
                        string[] dirs = Directory.GetDirectories(txtProfilesPath.Text);
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            WinControls.TabItem tabItem = new()
                            {
                                Header = Path.GetFileNameWithoutExtension(dirs[i]),
                                Foreground = System.Windows.Media.Brushes.White
                            };
                            tabProfiles.Items.Add(tabItem);

                            WinControls.ListView listView = new()
                            {
                                Width = 280,
                                MaxWidth = 300,
                                MaxHeight = 300
                            };


                            tabItem.Content = listView;

                            string[] files = Directory.GetFiles(dirs[i]);
                            for (int j = 0; j < files.Length; j++)
                            {
                                WinControls.Button btnFile = new()
                                {
                                    Tag = files[j],
                                    Content = Path.GetFileName(files[j]),
                                    Foreground = System.Windows.Media.Brushes.White
                                };
                                btnFile.Click += profileButton_Click;

                                listView.Items.Add(btnFile);
                            }
                        }
                    }
                    else txtProfilesPath.Text = "Select Valid Packs Path.";
                }
                
            }
            
        }

       
    }
}