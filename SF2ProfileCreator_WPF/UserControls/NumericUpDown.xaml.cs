using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string fl_value = "0";

        private int decimalplaces;

        public string Value
        {
            get { return fl_value; }
            set
            {
                fl_value = value;
            }
        }

        public int DecimalPlaces
        {
            get { return  decimalplaces; }
            set
            {
                decimalplaces = value;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            double val;
            if (!double.TryParse(Value, out val))
                val = 0.0;
            val += 1.0 / Pow(10, DecimalPlaces);
            val = double.Round(val, DecimalPlaces);
            Value = val.ToString();
            txtNumValue.Text = Value;
        }

        private void SubButton_Click(object sender, RoutedEventArgs e)
        {
            double val = double.Parse(Value);
            val -= 1.0 / Pow(10, DecimalPlaces);
            val = double.Round(val, DecimalPlaces);
            Value = val.ToString();
            txtNumValue.Text = Value;
        }

        private static int Pow(int num, int exp)
        {
            if (exp == 0) return 1;
            for (int i = 1; i < exp; i++)
            {
                num *= num;
            }
            return num;
        }
    }
}
