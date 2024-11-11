using Microsoft.Win32;
using SF2ProfileCreator_WPF.UserControls;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Path = System.IO.Path;

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

            LoadConfig();
        }

        private void LoadConfig()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\config.cfg"))
            {
                string[] config = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\config.cfg");
                if (config.Length < 1)
                {
                    Trace.WriteLine("profilePathSetting= Not Found.");
                    return;
                }
                if (config[0].Contains("profilePathSetting="))
                {
                    bool val = false;
                    string output = string.Empty;
                    foreach (char c in config[0])
                    {
                        if (val)
                            output += c;
                        if (c == '=')
                            val = true;
                    }
                    txtProfilesPath.Text = output;
                    Trace.WriteLine("profilePathSetting=" + txtProfilesPath.Text);
                }
            }
            else
                Trace.WriteLine("Config Not Found.");
        }

        private void menuDrag_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
        private void txtProfilesPath_TextChanged(object sender, TextChangedEventArgs e)
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
            dialog.DefaultDirectory = Directory.GetCurrentDirectory();
            dialog.ShowDialog();
            txtProfilesPath.Text = dialog.FolderName;
            UpdatePacksView();
            
        }
        private void profileButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btnFile)
                AddProfile(btnFile.Tag.ToString() ?? string.Empty);
        }

        private void btnNewProfile_Click(object sender, RoutedEventArgs e)
        {
            CreateProfile();
        }

        private void btnSelectProfile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                DefaultDirectory = Directory.GetCurrentDirectory(),
                Title = "Select .cfg file.",
                Filter = "Config file (*.cfg)|*.cfg"
            };
            dialog.ShowDialog();
            if (dialog.FileName == "")
                return;
            AddProfile(dialog.FileName);
        }

        private void UpdatePacksView()
        {
            
            if (tabProfilesPackViewer != null)
            {
                tabProfilesPackViewer.Items.Clear();
                string path = txtProfilesPath.Text;
                if (Directory.Exists(path))
                {
                    if (Path.GetFileNameWithoutExtension(path) == "packs")
                    {
                        string[] dirs = Directory.GetDirectories(txtProfilesPath.Text);
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            IEnumerable<string> files = Directory.EnumerateFiles(dirs[i]).Where(s => s.EndsWith(".cfg"));
                            if (files.Any())
                            {
                                TabItem tabItem = new()
                                {
                                    Header = Path.GetFileNameWithoutExtension(dirs[i]),
                                    Foreground = System.Windows.Media.Brushes.White
                                };
                                tabProfilesPackViewer.Items.Add(tabItem);

                                ListView listView = new()
                                {
                                    Width = 280,
                                    MaxWidth = 300,
                                    VerticalAlignment = VerticalAlignment.Top
                                };


                                tabItem.Content = listView;

                                foreach (string file in files)
                                {
                                    if (Path.GetExtension(file) == ".cfg")
                                    {
                                        Button btnFile = new()
                                        {
                                            Tag = file,
                                            Content = Path.GetFileName(file),
                                            Foreground = System.Windows.Media.Brushes.White
                                        };
                                        btnFile.Click += profileButton_Click;

                                        listView.Items.Add(btnFile);
                                    }
                                }
                            }
                        }
                        File.WriteAllText(Directory.GetCurrentDirectory() + "\\config.cfg", "profilePathSetting=" + path);
                        Trace.WriteLine("profilePathSetting= Written To " + path);
                    }
                    else txtProfilesPath.Text = "Select Valid Packs Path.";
                }
                
            }
            
        }

        private void CreateProfile()
        {
            TabItem tabItem = new()
            {
                Tag = "New Profile",
                Header = new TabItemEx(this, "New Profile")
            };
            tabProfiles.Items.Add(tabItem);

            ProfileProps profileProps = new()
            {
                FilePath = "New Profile"
            };
            tabItem.Content = profileProps;

            if (tabProfiles.Items.Count == 1)
            {
                tabItem.IsSelected = true;
            }
        }

        private void AddProfile(string path)
        {
            if (path == string.Empty)
                return;

            bool found = false;
            for (int i = 0; i < tabProfiles.Items.Count; i++)
            {
                TabItem tabItem = (TabItem)tabProfiles.Items.GetItemAt(i);
                if (tabItem.Tag.ToString() == Path.GetFileNameWithoutExtension(path))
                {
                    found = true;
                }
            }

            if (!found)
            {
                TabItem tabItem = new()
                {
                    Tag = Path.GetFileNameWithoutExtension(path),
                    Header = new TabItemEx(this, Path.GetFileNameWithoutExtension(path))
                };
                tabProfiles.Items.Add(tabItem);

                ProfileProps profileProps = new()
                {
                    FilePath = path
                };
                tabItem.Content = profileProps;

                if (tabProfiles.Items.Count == 1)
                {
                    tabItem.IsSelected = true;
                }
            }
        }

        public void CloseProfileTab(string profile)
        {
            for (int i = 0; i < tabProfiles.Items.Count; i++)
            {
                TabItem tabItem = (TabItem)tabProfiles.Items.GetItemAt(i);
                if (profile == tabItem.Tag.ToString())
                {
                    tabProfiles.Items.RemoveAt(i);
                    break;
                }
            }

        }
    }
}