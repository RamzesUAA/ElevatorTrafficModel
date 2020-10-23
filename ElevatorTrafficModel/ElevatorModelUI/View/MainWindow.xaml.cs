using ElevatorModelUI;
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

namespace ElevatorModelView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void btn_CallWorkingSpace_Click(object sender, RoutedEventArgs e)
        {
            WorkingSpace space = new WorkingSpace();
            space.Show();
            this.Close();
        }

        private void btn_CallWorkingSpace_Click2(object sender, RoutedEventArgs e)
        {
            DetectionPlatform platform = new DetectionPlatform();
            platform.Show();
            this.Close();
        }

        private void btn_CallWorkingSpace_Click3(object sender, RoutedEventArgs e)
        {
            CollisionDetection collisionDetection = new CollisionDetection();
            collisionDetection.Show();
            this.Close();
        }

        private void btn_CallWorkingSpace_Click4(object sender, RoutedEventArgs e)
        {
            InputMenu menu = new InputMenu();
            menu.Show();
            this.Close();
        }
    }
}
