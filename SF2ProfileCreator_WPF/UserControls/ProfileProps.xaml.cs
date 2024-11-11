using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace SF2ProfileCreator_WPF.UserControls
{
    /// <summary>
    /// Interaction logic for ProfileProps.xaml
    /// </summary>
    public partial class ProfileProps : UserControl
    {
        bool IsDragging;

        public ProfileProps()
        {
            InitializeComponent();
            DataContext = this;
            
        }

        private void btnRawProfile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDragging = true;
        }

        private void btnRawProfile_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsDragging = false;
        }

        private void btnRawProfile_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDragging)
                return;
            var mouseX = Mouse.GetPosition((IInputElement)sender).X * -1;
            if (gridRawProfile.Width + mouseX < gridRawProfile.MinWidth || gridRawProfile.Width + mouseX > gridRawProfile.MaxWidth)
                return;
            gridRawProfile.Width += mouseX;
        }

        List<string> profileRawLine = [];
        List<string> sectionLines = [];

        private string filePath = string.Empty;

        private bool isTypeStatue = false;
        private bool isTypeChaser = false;

        private Visibility tabStatueVis = Visibility.Collapsed;
        private Visibility tabChaserVis = Visibility.Collapsed;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FilePath"));
                if (filePath != "New Profile")
                {
                    UpdateRawProfileViewer();
                    LoadProfileProps();
                }
                else
                {
                    profileRawLine.Add("\"\"");
                    profileRawLine.Add("{");
                    profileRawLine.Add("}");
                    for (int i = 0; i < profileRawLine.Count; i++)
                    {
                        txtProfileRaw.Text += profileRawLine[i] + '\n';
                    }
                    sectionLines = profileRawLine;
                }
            }
        }

        public bool IsTypeStatue
        {
            get { return isTypeStatue; }
            set
            {
                isTypeStatue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsTypeStatue"));
            }
        }

        public bool IsTypeChaser
        {
            get { return isTypeChaser; }
            set
            {
                isTypeChaser = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsTypeChaser"));
            }
        }

        public Visibility TabStatueVis
        {
            get { return tabStatueVis; }
            set
            {
                tabStatueVis = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TabStatueVis"));
            }
        }

        public Visibility TabChaserVis
        {
            get { return tabChaserVis; }
            set
            {
                tabChaserVis = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TabChaserVis"));
            }
        }

        private void UpdateRawProfileViewer()
        {
            profileRawLine = [.. File.ReadAllLines(FilePath)];
            for (int i = 0; i < profileRawLine.Count; i++)
            {
                txtProfileRaw.Text += profileRawLine[i] + '\n';
            }
            sectionLines = profileRawLine;
        }

        private void LoadProfileProps()
        {
            foreach(string line in sectionLines)
            {
                if (line[0] != '\\' && line.Contains('"'))
                {
                    foreach(char c in line)
                    {
                        if (c != '"')
                            txtProfileName.Text += c;
                    }
                    break;
                }
            }
            txtProfileRaw.UpdateLayout();
            KvGoToSection(txtProfileName.Text);

            //Main
            txtBossName.Text = KvGetValue("name", string.Empty);
            cmbxProfileType.SelectedIndex = Int32.Parse(KvGetValue("type", "1")) - 1;
            string[] buffer = KvGetValueArray("eye_pos", 3, ["0", "0", "0"]);
            nmEyePos1.Value = buffer[0];
            nmEyePos2.Value = buffer[1];
            nmEyePos3.Value = buffer[2];
            buffer = KvGetValueArray("eye_ang_offset", 3, ["0", "0", "0"]);
            nmEyeAng1.Value = buffer[0];
            nmEyeAng2.Value = buffer[1];
            nmEyeAng3.Value = buffer[2];
            buffer = KvGetValueArray("mins", 3, ["0", "0", "0"]);
            nmMins1.Value = buffer[0];
            nmMins2.Value = buffer[1];
            nmMins3.Value = buffer[2];
            buffer = KvGetValueArray("maxs", 3, ["0", "0", "0"]);
            nmMaxs1.Value = buffer[0];
            nmMaxs2.Value = buffer[1];
            nmMaxs3.Value = buffer[2];
            nmKillRadius.Value = KvGetValue("kill_radius", "0");
            nmKillCoolDown.Value = KvGetValue("kill_cooldown", "0.0");
            chkRaidHitbox.IsChecked = KvGetBool("use_raid_hitbox", false);

            //Selection
            chkRandSel.IsChecked = KvGetBool("enable_random_selection", false);
            chkRandBoxinSel.IsChecked = KvGetBool("enable_random_selection_boxing", false);
            chkRandRevSel.IsChecked = KvGetBool("enable_random_selection_renevant", false);
            chkRandRevAdmSel.IsChecked = KvGetBool("enable_random_selection_renevant_admin", false);
            chkAdminOnlySel.IsChecked = KvGetBool("admin_only", false);

            //Model
            txtModelPath.Text = KvGetValue("model", string.Empty);
            nmModelScale.Value = KvGetValue("model_scale", "1.0");
            int temp = Int32.Parse(KvGetValue("effect_renderfx", "0"));
            cmbxRenderFX.SelectedIndex = Int32.Parse(KvGetValue("effect_renderfx", "0"));
            cmbxRenderMode.SelectedIndex = Int32.Parse(KvGetValue("effect_rendermode", "0"));
            buffer = KvGetValueArray("effect_rendercolor", 4, ["255", "255", "255", "255"]);
            nmEffectRenderColor1.Value = buffer[0];
            nmEffectRenderColor2.Value = buffer[1];
            nmEffectRenderColor3.Value = buffer[2];
            nmEffectRenderColor4.Value = buffer[3];
        }

        private static string KvGetValueFromLine(string text)
        {
            int bracketCount = 0;
            char[] line = text.ToCharArray();
            string output = string.Empty;
            foreach (char c in line)
            {
                if (bracketCount > 2)
                {
                    if (c == '\"')
                    {
                        return output;
                    }
                    output += c;
                }
                if (c == '\"')
                {
                    bracketCount++;
                }
            }
            return output;
        }

        private static string[] KvGetArrayValue(string text, int size)
        {
            int idx = 0;
            string[] val = new string[size];
            char[] line = KvGetValueFromLine(text).ToCharArray();
            string temp = string.Empty;
            foreach (char c in line)
            {
                if (c == ' ')
                {
                    val[idx] = temp;
                    idx++;
                    temp = string.Empty;
                }
                else temp += c;
            }
            val[idx] = temp;
            return val;
        }

        private string[] KvGetValueArray(string key, int size, string[] notfound)
        {
            int brackets = 0;
            foreach (string line in sectionLines)
            {
                if (line.Contains("{"))
                    brackets++;
                if (line.Contains("}") && brackets > 0)
                    brackets--;
                if (line.Contains("\"" + key + "\"") && brackets < 1)
                {
                    int idx = 0;
                    string[] val = new string[size];
                    char[] linechar = KvGetValueFromLine(line).ToCharArray();
                    string temp = string.Empty;
                    foreach (char c in linechar)
                    {
                        if (c == ' ')
                        {
                            val[idx] = temp;
                            idx++;
                            temp = string.Empty;
                        }
                        else temp += c;
                    }
                    val[idx] = temp;
                    return val;
                }
            }
            return notfound;
        }

        private string KvGetValue(string key, string notfound)
        {
            int brackets = 0;
            foreach (string line in sectionLines)
            {
                if (line.Contains("{"))
                    brackets++;
                if (line.Contains("}") && brackets > 0)
                    brackets--;
                if (line.Contains("\"" + key + "\"") && brackets < 1)
                    return KvGetValueFromLine(line);
            }
            return notfound;
        }

        private bool KvGetBool(string key, bool notfound)
        {
            int brackets = 0;
            foreach (string line in sectionLines)
            {
                if (line.Contains("{"))
                    brackets++;
                if (line.Contains("}") && brackets > 0)
                    brackets--;
                if (line.Contains("\"" + key + "\"") && brackets < 1)
                {
                    if (KvGetValueFromLine(line) == "1")
                        return true;
                    else
                        return false;
                }
            }
            return notfound;
        }

        private void KvGoToSection(string section)
        {
            List<string> newLines = [];
            int brackets = 0;
            bool InSection = false;
            foreach(string line in sectionLines)
            {
                if (InSection)
                    newLines.Add(line);
                if (line.Contains('{'))
                    brackets++;
                if (line.Contains('}') && brackets > 0)
                {
                    brackets--;
                    if (brackets < 1 && InSection)
                    {
                        Trace.WriteLine(newLines.Count);
                        newLines.RemoveAt(newLines.Count - 1);
                        newLines.RemoveAt(0);
                        sectionLines = newLines;
                        break;
                    }
                }
                if (line.Contains("\"" + section + "\"") && !InSection && brackets < 1)
                {
                    InSection = true;
                }
                
            }
        }

        private void cmbxProfileType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbxProfileType.SelectedIndex == 0) // Statue
            {
                IsTypeStatue = true;
                IsTypeChaser = false;
                tabStatue.Visibility = Visibility.Visible;
                tabChaser.Visibility = Visibility.Collapsed;
                Trace.WriteLine("Statue");
            }
            else if (cmbxProfileType.SelectedIndex == 1) // Chaser
            {
                IsTypeStatue = false;
                IsTypeChaser = true;
                tabStatue.Visibility = Visibility.Collapsed;
                tabChaser.Visibility = Visibility.Visible;
                Trace.WriteLine("Chaser");
            }
        }
    }
}
