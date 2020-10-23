using ElevatorModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for WorkingSpace.xaml
    /// </summary>
    public partial class WorkingSpace : Window
    {
        ImageBrush playerSprite = new ImageBrush();

        bool goLeft, goRight, goUp, goDown;
        int playerSpeed = 8;
        int speed = 12;

        DispatcherTimer gameTimer = new DispatcherTimer();
        public WorkingSpace()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            myCanvas.Focus();

            gameTimer.Tick += gameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
            playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/elevator.png"));
            player.Fill = playerSprite;

        }

        private void gameTimerEvent(object sender, EventArgs e)
        {
            if (goLeft && Canvas.GetLeft(player) > 5)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
                // if go left is true and player is inside the boundary from the left
                // then we can set left of the player to move towards left of the screen
            }
            if (goRight && Canvas.GetLeft(player) + (player.Width + 20) < this.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
                // if go right is true and player is inside the boundary from the right
                // then we can set left of the player to move towards right of the screen
            } 
            if (goUp && Canvas.GetTop(player) > 0)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) - playerSpeed);
                // if go up is true and player is within the boundary from the top 
                // then we can use the set top to move the rec1 towards top of the screen
            }
            if (goDown && Canvas.GetTop(player) + (player.Height + 20) < this.Height)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) + playerSpeed);
                // if go down is true and player is within the boundary from the bottom of the screen
                // then we can set top of rec1 to move down
            }

            Canvas.SetLeft(box, Canvas.GetLeft(box) + speed);
            if(Canvas.GetLeft(box) < 5 || Canvas.GetLeft(box) + (box.Width * 2) > this.Width)
            {
                speed = -speed;
            }
        }

        private void KeiIsDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left)
            {
                goLeft = true;
            }
            if (e.Key == Key.Right)
            {
                goRight = true;
            }
            if(e.Key == Key.Up)
            {
                goUp = true;
            }
            if(e.Key == Key.Down)
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = false;
            }
            if (e.Key == Key.Right)
            {
                goRight = false;
            }
            if (e.Key == Key.Up)
            {
                goUp = false;
            }
            if (e.Key == Key.Down)
            {
                goDown = false;
            }
        }
    }
}
