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

using System.Windows.Threading;

namespace ElevatorModelUI
{
    /// <summary>
    /// Interaction logic for DetectionPlatform.xaml
    /// </summary>
    public partial class DetectionPlatform : Window
    {


        DispatcherTimer timer = new DispatcherTimer();

        int speed = 10;
        int dropSpeed = 10;
        bool goLeft, goRight;
        public DetectionPlatform()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            myCanvas.Focus();
            timer.Tick += MainTimerEvent;
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Start();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            Canvas.SetTop(player, Canvas.GetTop(player) + dropSpeed); 

            if(goLeft==true  && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - speed);
            }

            if (goRight && Canvas.GetLeft(player) + (player.Width + 15) < this.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + speed);
            }

            if (Canvas.GetTop(player) + (player.Height * 2) > this.Height)
            {
                Canvas.SetTop(player, -80);
            }


            foreach(var x in myCanvas.Children.OfType<Rectangle>())
            {
                if((string)x.Tag == "platform")
                {
                    x.Stroke = Brushes.Black;

                    Rect playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
                    Rect platformHitBox =  new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if(playerHitBox.IntersectsWith(platformHitBox))
                    {
                        dropSpeed = 0;
                        Canvas.SetTop(player, Canvas.GetTop(x) - player.Height);
                        txtInfo.Content = "Landed on - " + x.Name;
                    }
                    else
                    {
                        dropSpeed = 10;
                    }
                }
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = false;
            }
            else if (e.Key == Key.Right)
            {
                goRight = false;
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = true;
            }
            else if (e.Key == Key.Right)
            {
                goRight = true;
            }
        }
    }
}
