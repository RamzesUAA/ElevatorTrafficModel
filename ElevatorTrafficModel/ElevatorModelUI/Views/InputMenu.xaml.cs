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
      //  public ObservableCollection<string> ElevatorTypes { get; set; }

        public InputMenu()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            
        }

        private void InitializeSecondWindowComponent()
        {
            dgData.ItemsSource = Elevators;
        }


        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            Elevators = new ObservableCollection<Elevator>();
            if (!TextBoxesValidator())
            {
                MessageBox.Show("All fields should be filled");
                return;
            }
            elevatorSet.Visibility = Visibility.Visible;

            for(int i = 0; i < Convert.ToInt32(textBox_ElevatorsCount.Text); ++i)
            {
                Elevators.Add(new Elevator() { ID = $"Elevator_" + (i+1), ElevatorSpeed = 1, UpDown = "UP" });
            }
            InitializeSecondWindowComponent();
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {

            elevatorSet.Visibility = Visibility.Hidden;
        }

        private void btn_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow workingSpace = new MainWindow(Elevators.ToList(), Convert.ToInt32(textBox_FloorsCount.Text));
            workingSpace.Show();
            this.Close();
        }

        private void Floors_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool IsDigit;
            Regex regex = new Regex("[^5-9]+");
            IsDigit = regex.IsMatch(e.Text);
            if (IsDigit == true)
            {
                e.Handled = true;
                return;
            }
        }

        private void Elevator_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool IsDigit;
            Regex regex = new Regex("[^1-4]+");
            IsDigit = regex.IsMatch(e.Text);
            if (IsDigit == true)
            {
                e.Handled = true;
                return;
            }
        }



        private void textBox_ElevatorsCount_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HelperElevatorCount.Visibility = Visibility.Visible;

        }

        private void textBox_ElevatorsCount_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HelperElevatorCount.Visibility = Visibility.Hidden;

        }

        private void textBox_FloorsCount_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HelperFloorCount.Visibility = Visibility.Visible;
        }

        private void textBox_FloorsCount_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HelperFloorCount.Visibility = Visibility.Hidden;
        }
    }
}
