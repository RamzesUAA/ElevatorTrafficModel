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
using System.Collections.ObjectModel;
using ElevatorModelBL.Models;
using ElevatorModelBL.Enums;

namespace ElevatorModelUI
{

    public class StatusList:List<string>
    {
        public StatusList()
        {
            this.Add(ElevatorType.Hydraulic.ToString());
            this.Add(ElevatorType.MachineRoom.ToString());
            this.Add(ElevatorType.Traction.ToString());

        }
    }

    /// <summary>
    /// Interaction logic for InputMenu.xaml
    /// </summary>
    /// 
    public partial class InputMenu : Window
    {
        public ObservableCollection<Elevator> Elevators { get; set; }
        public ObservableCollection<string> ElevatorTypes { get; set; }

        Building building = new Building();

        public InputMenu()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            Elevators = new ObservableCollection<Elevator>();
        }

        private void InitializeSecondWindowComponent()
        {
            dgData.ItemsSource = Elevators;
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
            if(int.Parse(digit) <= 17 && int.Parse(digit) != 0)
            {
                e.Handled = false;
                return;
            }
            e.Handled = true;
        }

        private bool TextBoxesValidator()
        {
            if(textBox_ElevatorsCount.Text == "")
            {
                return false;
            }
            if(textBox_FloorsCount.Text=="")
            {
                return false;
            }
            return true;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!TextBoxesValidator())
            {
                MessageBox.Show("All fields should be filled");
                return;
            }
            elevatorSet.Visibility = Visibility.Visible;
            building.CountOfFloors = Convert.ToInt32(textBox_FloorsCount.Text);
            for(int i = 0; i < Convert.ToInt32(textBox_ElevatorsCount.Text); ++i)
            {
                Elevators.Add(new Elevator() { ID = i + 1 });
            }
            InitializeSecondWindowComponent();
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            elevatorSet.Visibility = Visibility.Hidden;
        }

        private void btn_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow workingSpace = new MainWindow(Elevators.ToList(), building.CountOfFloors);
            workingSpace.Show();
            this.Close();
        }
    }
}
