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
using System.Runtime.Serialization;
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
        int elevatorSpeed = 1;
        Generator generator = new Generator();
        QueryController QueryController;
        ElevatorController ElevatorController = new ElevatorController();
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
            //  gameTimer.Tick += gameTimerEvent;
            
        }
        public MainWindow(List<Elevator> elevators, int countOfFloors):this()
        {
            Elevators = elevators;
            FloorsCount = countOfFloors;
            MakeFloors();
            MakeElevators();
            QueryController = new QueryController(Elevators, Floors);
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

                Rectangle item = build.Children.OfType<Rectangle>().LastOrDefault();
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
                Canvas.SetBottom(elevator, 9);
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

        void MakeRequests()
        {
            foreach(var elevator in Elevators)
            {
                foreach (var people in QueryController.Queries)
                {
                    foreach (var person in people.PeopleInQueue[elevator])
                    {
                        elevator.QueueOfRequests.Add(person.CurrentFloor);
                    }
                }
            }
        }



        private void btn_GeneratePassangers_Click(object sender, RoutedEventArgs e)
        {
            List<Person> people = generator.GetPassangers(Floors);

            foreach (var person1 in people)
            {
                QueryController.Add(person1, Elevators);
            }
            MakeRequests();

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
                            Tag = "passenger" + elevator.Key.ID + item.NumberOfFloor,
                            ToolTip = new ToolTip{ Content = personInQueue.Name },
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
            foreach(var person in people)
            {
                People.Add(person);
            }
        }

        void checkInside(List<Person> people)
        {
            string str = "";
            foreach (var item in people)
            {
                str += item.Name += " ";
            }
            MessageBox.Show(str);
        }

        // TODO: витягнути з інтернету багато різних імен, сериалізувати їх в JSON формат і потім провести десериалізацію і рандомно присвоювати кожному об'єкту певне ім'я
        // зробити простий запис даних про стоврених пасажирів ( ім'я, вага, намір їхати на певний поверх, поточний поверх)
        // інформацію про ліфти ( тип, вантажопідйомність)


        private void gameElevatorEvent(object sender, EventArgs e)
        {
            foreach(var item in Elevators)
            {
                var elevator = build.Children.OfType<Rectangle>().Where(p => p.Name == item.ID).FirstOrDefault();
                Canvas.SetBottom(elevator, Canvas.GetBottom(elevator) + elevatorSpeed);

                if (Canvas.GetBottom(elevator) < 9 || Canvas.GetBottom(elevator)  > 40* (Floors.Count()-1) + 9)
                {
                    elevatorSpeed = -elevatorSpeed;
                }
                List<Rectangle> person = new List<Rectangle>();
                foreach (var floor in build.Children.OfType<Rectangle>())
                {
                    if ((string)floor.Tag == "floorItem")
                    {
                        floor.Stroke = Brushes.Black;   

                         Rect elevatorHitBox = new Rect(Canvas.GetLeft(elevator), Canvas.GetBottom(elevator), elevator.Width, elevator.Height);
                         Rect floorHitBox = new Rect(Canvas.GetLeft(floor), Canvas.GetBottom(floor), floor.Width, floor.Height);
                        
                        if (elevatorHitBox.IntersectsWith(floorHitBox))
                        {
                            // TODO: перевірити, чи хтось хоче виходити на поточному поверсі, якщо ні, то не зупинятись, або, якщо є вільно ще 70 кг
                            // то тоді зупинитись і перевірити першу людину в черзі, чи її вага є менша за 70 кг і чи вона хоче їхати в тому самому напрямку, куди і рухається 

                            var currentFloor = Floors.Where(p => p.ID == (string)floor.Name).First();
                            var CurrentQuery = QueryController.GetQuery(currentFloor);
                            List<Person> currentQueryToTheElevator = CurrentQuery.PeopleInQueue[item];
                            
                            if (currentQueryToTheElevator==null)
                            {
                                continue;
                            }


                            for(int i = 0; i  < currentQueryToTheElevator.Count; ++i)
                            {
                           
                                if (ElevatorController.ElevatorFilling(item, currentQueryToTheElevator.First()) == true)
                                {
                                    // MessageBox.Show(item.ToString());
                                    person = build.Children.OfType<Rectangle>().Where(p => (string)p.Name == currentQueryToTheElevator.First().Name).ToList();
                                    currentQueryToTheElevator.Remove(currentQueryToTheElevator.First());
                                }
                                //checkInside(currentQueryToTheElevator);
                            }
                            var peopleToExit = item.PeopleInsideElevator.Where(p => p.FloorIntention == currentFloor).ToList();
                            for(int i = 0; i < peopleToExit.Count; ++i)
                            {
                                
                               
                                //MessageBox.Show(peopleToExit.First().ToString());
                                peopleToExit.Remove(peopleToExit.First());
                            }

                            // TODO: зробити розсинхрон ліфів + додати перевірку, щоб вони доїжджали до максимального поверха, який є викликаний, а не до максимального пверху в будинку

                            // MessageBox.Show($"{elevator.Name} on the {floor.Name}");
                            // elevatorSpeed = 0;
                            // Canvas.SetTop(player, Canvas.GetTop(floor) - player.Height);
                        }
                        
                    }
                }
                for (int i = 0; i < person.Count; i++)
                {
                  
                    build.Children.Remove(person.First());
                }
            }
        }


        

        private void btn_RunModel_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Tick += gameElevatorEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
        }

        private void gameTimerEvent(object sender, EventArgs e)
        {
            /*
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
            */
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
