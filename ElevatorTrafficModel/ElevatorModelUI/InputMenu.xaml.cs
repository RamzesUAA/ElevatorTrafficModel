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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace ElevatorModelUI
{
    /// <summary>
    /// Interaction logic for InputMenu.xaml
    /// </summary>
    public partial class InputMenu : Window
    {
        public InputMenu()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool IsDigit;
           
            Regex regex = new Regex("[^0-9]+");
            IsDigit = regex.IsMatch(e.Text);
            if(IsDigit == true)
            {
                e.Handled = true;
                return;
            }
            string digit ="";
            if (((TextBox)sender).Text.Length != 0)
            {
                digit = ((TextBox)sender).Text;
            }

            digit += e.Text;
            if(int.Parse(digit) >= 5 && int.Parse(digit) <= 17)
            {
                e.Handled = false;
                return;
            }
            e.Handled = true;
        }
    }
}
