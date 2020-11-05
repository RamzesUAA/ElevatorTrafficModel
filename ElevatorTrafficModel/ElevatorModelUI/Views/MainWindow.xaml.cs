using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ElevatorModelBL.Controllers;
using ElevatorModelBL.Models;
using ElevatorModelBL.Enums;

namespace ElevatorModelUI
{
    /// <summary>
    /// Interaction logic for WorkingSpace.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageBrush HydraulicElevator = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/HydraulicElevator.png"))
        };
        ImageBrush MachineRoomElevator = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/MachineRoomElevator.png"))
        };
        ImageBrush TractionElevator = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/TractionElevator.png"))
        };
        ImageBrush floorSprite = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Floor.png"))
        };
        ImageBrush BoySprite = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Boy.png"))
        };
        ImageBrush GirlSprite = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Girl.png"))
        };

        bool goLeft, goRight, goUp, goDown;
        int playerSpeed = 4;
        int speed = 6;

        List<Elevator> Elevators;
        List<Person> People;
        List<Floor> Floors;
        int FloorsCount;

        DispatcherTimer gameTimer = new DispatcherTimer();
        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            People = new List<Person>();
            Floors = new List<Floor>();

            myCanvas.Focus();
            gameTimer.Tick += gameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
            box.Fill = BoySprite;
        }
        public MainWindow(List<Elevator> elevators, int countOfFloors):this()
        {
            Elevators = elevators;
            FloorsCount = countOfFloors;
            MakeFloors();
            MakeElevators();
        }

        private void MakeFloors()
        {
            int counter = 0;
            for (int i = 0; i < FloorsCount; ++ i)
            {
                counter++;
                Rectangle floor = new Rectangle()
                {
                    Name = $"floor" + counter,
                    Height = 10,
                    Width = 800,
                    Fill = floorSprite,
                    Tag = "floorItem"
                };

                Rectangle item = build.Children.OfType<Rectangle>().Last();
                Canvas.SetLeft(floor, 0);
                if (counter != 1)
                {
                    Canvas.SetBottom(floor, Canvas.GetBottom(item) + 40);
                }
                else
                {
                    Canvas.SetBottom(floor, 0);
                }

                build.Children.Add(floor);
                Floors.Add(new Floor() { ID = floor.Name });
            }
        }

      
        private void MakeElevators()
        {
            int countOfElevators = 0;
            foreach(var Elevator in Elevators)
            {
                
                Rectangle elevator = new Rectangle();
                countOfElevators++;
                switch (Elevator.TypeOfElevator)
                {
                    case ElevatorType.Hydraulic:
                        elevator.Name = Elevator.ID;
                        elevator.Height = 30;
                        elevator.Width = 20;
                        elevator.Fill = HydraulicElevator;
                        elevator.Tag = "elevatorItem";
                        break;
                    case ElevatorType.MachineRoom:
                        elevator.Name = Elevator.ID;
                        elevator.Height = 30;
                        elevator.Width = 20;
                        elevator.Fill = MachineRoomElevator;
                        elevator.Tag = "elevatorItem";
                        break;
                    case ElevatorType.Traction:
                        elevator.Name = Elevator.ID;
                        elevator.Height = 30;
                        elevator.Width = 20;
                        elevator.Fill = TractionElevator;
                        elevator.Tag = "elevatorItem";
                        break;
                }
                
                var item = build.Children.OfType<Rectangle>().Last();
                Canvas.SetBottom(elevator, 0);
                if (countOfElevators != 1)
                {
                    Canvas.SetLeft(elevator, Canvas.GetLeft(item) + 154);
                }
                else
                {
                    Canvas.SetLeft(elevator, 150);
                }
                build.Children.Add(elevator);
            }
        }

 
        private void btn_addFloor_Click(object sender, RoutedEventArgs e)
        {
           
        }

        Generator generator = new Generator();
        QueryController QueryController = new QueryController();
        private void btn_GeneratePassangers_Click(object sender, RoutedEventArgs e)
        {
          
            //generator.GetPassangers(People, Floors);
            People.Add(new Person()
            {
                Name = "Roman",
                CurrentFloor = Floors[2],
                Weigh = 32,
                FloorIntention = Floors[2]
            });
            People.Add(new Person()
            {
                Name = "Olga",
                CurrentFloor = Floors[2],
                Weigh = 32,
                FloorIntention = Floors[2]
            });
            People.Add(new Person()
            {
                Name = "Julia",
                CurrentFloor = Floors[0],
                Weigh = 32,
                FloorIntention = Floors[2]
            });

            People.Add(new Person()
            {
                Name = "Ron",
                CurrentFloor = Floors[0],
                Weigh = 32,
                FloorIntention = Floors[2]
            });

            People.Add(new Person()
            {
                Name = "Oksa",
                CurrentFloor = Floors[0],
                Weigh = 32,
                FloorIntention = Floors[2]
            });
            People.Add(new Person()
            {
                Name = "Jeko",
                CurrentFloor = Floors[0],
                Weigh = 32,
                FloorIntention = Floors[2]
            });
            People.Add(new Person()
            {
                Name = "David",
                CurrentFloor = Floors[0],
                Weigh = 32,
                FloorIntention = Floors[2]
            });
            People.Add(new Person()
            {
                Name = "Sofa",
                CurrentFloor = Floors[0],
                Weigh = 32,
                FloorIntention = Floors[2]
            });
            People.Add(new Person()
            {
                Name = "Oleg",
                CurrentFloor = Floors[0],
                Weigh = 32,
                FloorIntention = Floors[2]
            });

            foreach (var item in People)
            {
                QueryController.Add(item, Elevators);
            }

            int countOfPersons = 0;
            foreach (var item in QueryController.Queries)
            {
                foreach(var elevator in item.PeopleInQueue)
                {
                    int countInQueue = 0;
                    foreach (var personInQueue in elevator.Value)
                    {
                        countOfPersons++;
                        Rectangle person = new Rectangle()
                        {
                            Name = personInQueue.Name,
                            Height = 30,
                            Width = 20,
                            Fill = countOfPersons % 2 == 0 ? GirlSprite : BoySprite,
                            Tag = "passenger" + elevator.Key.ID + item.NumberOfFloor
                        };

  
                        var currentPosition = build.Children.OfType<Rectangle>().Where(p => p.Name == personInQueue.CurrentFloor.ToString()).FirstOrDefault();
                        var currentElevatorQueue = build.Children.OfType<Rectangle>().Where(p => p.Name == elevator.Key.ID).FirstOrDefault();
                      
                        Canvas.SetLeft(person, Canvas.GetLeft(currentElevatorQueue)- 25- countInQueue * 20);
                        Canvas.SetBottom(person, Canvas.GetBottom(currentPosition) + 7);
                        build.Children.Add(person);
                        countInQueue++;

                    }
                }
            }
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
