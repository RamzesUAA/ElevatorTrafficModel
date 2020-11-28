using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
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


        private void Btn_Close_Click(object sender, RoutedEventArgs e)
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

            for(var i = 0; i < Convert.ToInt32(textBox_ElevatorsCount.Text); ++i)
            {
                Elevators.Add(new Elevator() { Id = $"Elevator_" + (i+1), ElevatorSpeed = 1, UpDown = "UP" });
            }
            InitializeSecondWindowComponent();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {

            elevatorSet.Visibility = Visibility.Hidden;
        }

        private void Btn_Set_Click(object sender, RoutedEventArgs e)
        {
            foreach(var elevator in Elevators)
            {
                if(elevator.TypeOfElevator == 0)
                {
                    MessageBox.Show("You should choose the type for each of the elevators.");
                    return;
                }
            }

            MainWindow workingSpace = new MainWindow(Elevators.ToList(), Convert.ToInt32(textBox_FloorsCount.Text));
            workingSpace.Show();
            this.Close();
        }

        private void Floors_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^5-9]+");
            var isDigit = regex.IsMatch(e.Text);
            if (isDigit != true) return;
            e.Handled = true;
        }

        private void Elevator_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^1-4]+");
            var isDigit = regex.IsMatch(e.Text);
            if (isDigit != true) return;
            e.Handled = true;
        }



        private void TextBox_ElevatorsCount_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HelperElevatorCount.Visibility = Visibility.Visible;

        }

        private void TextBox_ElevatorsCount_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HelperElevatorCount.Visibility = Visibility.Hidden;

        }

        private void TextBox_FloorsCount_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HelperFloorCount.Visibility = Visibility.Visible;
        }

        private void TextBox_FloorsCount_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HelperFloorCount.Visibility = Visibility.Hidden;
        }
    }
}
