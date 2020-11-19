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
        ImageBrush PictureSprite = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PictureWall.jpg"))
        };
        ImageBrush WindowSprite = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Window.jpg"))
        };





        bool goLeft, goRight, goUp, goDown;
        int playerSpeed = 4;
        int speed = 6;

        Generator generator = new Generator();
        QueryController QueryController;
        //ElevatorController ElevatorController = new ElevatorController();
        List<Elevator> Elevators;
        List<Person> People;
        List<Floor> Floors;
        int FloorsCount;

        DispatcherTimer gameTimer = new DispatcherTimer();
        DispatcherTimer passengersGeneratorTimer = new DispatcherTimer();

        public event EventHandler ThresholdReached;
        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            People = new List<Person>();
            Floors = new List<Floor>();

            myCanvas.Focus();
            
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
                List<Rectangle> pictures = new List<Rectangle>();
                List<Rectangle> windows = new List<Rectangle>();

                for (int j = 0; j < 6; ++j)
                {
                    pictures.Add(new Rectangle()
                    {
                        Width = 30,
                        Height = 20,
                        Fill = PictureSprite
                    });

                    windows.Add(new Rectangle()
                    {
                        Width = 25,
                        Height = 35,
                        Fill = WindowSprite
                    });
                }


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
                    Canvas.SetBottom(floor, Canvas.GetBottom(item) + 75);

                    Canvas.SetLeft(windows.Last(), 50);
                    Canvas.SetBottom(windows.Last(), Canvas.GetBottom(item) + 20);
                    build.Children.Add(windows.Last());

                    foreach (var picture in pictures)
                    {
                        Canvas.SetLeft(picture, 10);
                        Canvas.SetBottom(picture, Canvas.GetBottom(item) + 30);
                        build.Children.Add(picture);
                    }

                }
                else
                {
                    Canvas.SetBottom(floor, 0);

                    Canvas.SetLeft(windows.Last(), 50);
                    Canvas.SetBottom(windows.Last(), 20);
                    build.Children.Add(windows.Last());

                    foreach (var picture in pictures)
                    {
                        Canvas.SetLeft(picture, 10);
                        Canvas.SetBottom(picture, 30);
                        build.Children.Add(picture);
                    }

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
                        elevator.Height = 50;
                        elevator.Width = 30;
                        elevator.Fill = HydraulicElevator;
                        elevator.Tag = "elevatorItem";
                        break;
                    case ElevatorType.MachineRoom:
                        elevator.Name = Elevator.ID;
                        elevator.Height = 50;
                        elevator.Width = 30;
                        elevator.Fill = MachineRoomElevator;
                        elevator.Tag = "elevatorItem";
                        break;
                    case ElevatorType.Traction:
                        elevator.Name = Elevator.ID;
                        elevator.Height = 50;
                        elevator.Width = 30;
                        elevator.Fill = TractionElevator;
                        elevator.Tag = "elevatorItem";
                        break;
                }
                
                var item = build.Children.OfType<Rectangle>().Last();
                Canvas.SetBottom(elevator, 0);
                if (countOfElevators != 1)
                {
                    Canvas.SetLeft(elevator, Canvas.GetLeft(item) + 152);
                }
                else
                {
                    Canvas.SetLeft(elevator, 146);
                }
                build.Children.Add(elevator);
            }
        }

 
        private void btn_addFloor_Click(object sender, RoutedEventArgs e)
        {
           
        }

        void MakeRequests(List<Person> People)
        {

            foreach (var elevator in Elevators)
            {
                foreach (var person in People)
                {
                    foreach (var people in QueryController.Queries)
                    {
                        foreach (var personInQueue in people.PeopleInQueue[elevator])
                        {
                            if (personInQueue == person)
                            {
                                elevator.QueueOfRequests.Add(person.CurrentFloor);
                            }
                        }
                    }
                }
            }
        }


        // DONE: реалізувати перевірку людей в черзі, до одного ліфта максимум може ставати в чергу 4 людини.
        // DONE: атоматизувати появу нових пасажирів.
        // DONE: зробити перевірку на ввід кількості поверхів, та кількості пасажирів.
        // DONE: додати можливість повернення до початкового меню.
        // DONE: пофіксити баг з поверненням назад в початковому меню.
        // DONE: спарсити імена у файл(10000 імен).

          
        // TODO: реалізувати продовження руху ліфта після зупинки
        // TODO: додати трайкетч на перевірку файлу з некоректними даними,  і перевіряти трайкетчем чи існує файл, якщо файлу не існує, то виводити повідомлення через трай кетч.
        // TODO: додати ще один трайкетч.
        // TODO: доробити інтерфейс, особливо бекграунд.
        // TODO: перечитати методичку і додати все, що не портрібно.
        private void btn_GeneratePassangers_Click(object sender, RoutedEventArgs e)
        {
            List<Person> people = generator.GetPassangers(Floors);

            string str = "";
            foreach (var item in people) 
            {
                str += $"{item.Name}, current floor: {item.CurrentFloor}\n";

            }

            
          
            QueryController.Add(people, Elevators);
    
            MakeRequests(people);


            foreach (var item in QueryController.Queries)
            {
                foreach(var elevator in item.PeopleInQueue)
                {
                    int countInQueue = 0;
                    foreach (var reload in elevator.Value)
                    {
                        build.Children.Remove(build.Children.OfType<Rectangle>().Where(p => p.Name == reload.Name.ToString()).FirstOrDefault());
                    }
                    foreach (var personInQueue in elevator.Value)
                    {
                       

                        Rectangle person = new Rectangle()
                        {
                            Name = personInQueue.Name,
                            Height = 40,
                            Width = 25,
                            Fill = personInQueue.Sex == "Woman" ? GirlSprite : BoySprite,
                            Tag = "passenger" + elevator.Key.ID + item.NumberOfFloor,
                            ToolTip = new ToolTip{ Content = personInQueue.Name + ", floor intension: " + personInQueue.FloorIntention + ", weight: " + personInQueue.Weigh},
                        };

                        
                        var currentPosition = build.Children.OfType<Rectangle>().Where(p => p.Name == personInQueue.CurrentFloor.ToString()).FirstOrDefault();
                        var currentElevatorQueue = build.Children.OfType<Rectangle>().Where(p => p.Name == elevator.Key.ID).FirstOrDefault();
                      
                        Canvas.SetLeft(person, Canvas.GetLeft(currentElevatorQueue)- 30- countInQueue * 25);
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

        private void passengerGeneratorEvent(object sender, EventArgs e)
        {
            List<Person> people = generator.GetPassangers(Floors);

            string str = "";
            foreach (var item in people)
            {
                str += $"{item.Name}, current floor: {item.CurrentFloor}\n";

            }

            QueryController.Add(people, Elevators);

            MakeRequests(people);


            foreach (var item in QueryController.Queries)
            {
                foreach (var elevator in item.PeopleInQueue)
                {
                    int countInQueue = 0;
                    foreach (var reload in elevator.Value)
                    {
                        build.Children.Remove(build.Children.OfType<Rectangle>().Where(p => p.Name == reload.Name.ToString()).FirstOrDefault());
                    }
                    foreach (var personInQueue in elevator.Value)
                    {
                        Rectangle person = new Rectangle()
                        {
                            Name = personInQueue.Name,
                            Height = 40,
                            Width = 25,
                            Fill = personInQueue.Sex == "Woman" ? GirlSprite : BoySprite,
                            Tag = "passenger" + elevator.Key.ID + item.NumberOfFloor,
                            ToolTip = new ToolTip { Content = personInQueue.Name + ", floor intension: " + personInQueue.FloorIntention + ", weight: " + personInQueue.Weigh },
                        };


                        var currentPosition = build.Children.OfType<Rectangle>().Where(p => p.Name == personInQueue.CurrentFloor.ToString()).FirstOrDefault();
                        var currentElevatorQueue = build.Children.OfType<Rectangle>().Where(p => p.Name == elevator.Key.ID).FirstOrDefault();

                        Canvas.SetLeft(person, Canvas.GetLeft(currentElevatorQueue) - 30 - countInQueue * 25);
                        Canvas.SetBottom(person, Canvas.GetBottom(currentPosition) + 7);
                        build.Children.Add(person);
                        countInQueue++;
                    }
                }
            }
            foreach (var person in people)
            {
                People.Add(person);
            }
        }

        private void gameElevatorEvent(object sender, EventArgs e)
        {
            foreach (var item in Elevators)
            {
                var RectangleElevator = build.Children.OfType<Rectangle>().Where(p => p.Name == item.ID).First();
                Canvas.SetBottom(RectangleElevator, Canvas.GetBottom(RectangleElevator) + item.ElevatorSpeed);
                List<Rectangle> person = new List<Rectangle>();
                foreach (var floor in build.Children.OfType<Rectangle>().Where(p => (string)p.Tag == "floorItem"))
                {
                    floor.Stroke = Brushes.Black;
                    Rect elevatorHitBox = new Rect(Canvas.GetLeft(RectangleElevator), Canvas.GetBottom(RectangleElevator), RectangleElevator.Width, RectangleElevator.Height);
                    Rect floorHitBox = new Rect(Canvas.GetLeft(floor), Canvas.GetBottom(floor), floor.Width, floor.Height);

                    if (elevatorHitBox.IntersectsWith(floorHitBox))
                    {
                        var currentFloor = Floors.Where(p => p.ID == (string)floor.Name).First();


                        if(item.MaxTurnedPoint()==null && item.PeopleInsideElevator.Count() == 0 && item.QueueOfRequests.Count()==0)
                        {
                            item.UpDown = "Stopped";
                            item.ElevatorSpeed = 0;
                        }
                        else if(item.MaxTurnedPoint()==null && item.PeopleInsideElevator.Count() != 0)
                        {
                            item.UpDown = item.UpDown;
                            item.ElevatorSpeed = item.ElevatorSpeed;
                        }
                        else if (currentFloor.ID[5] == item.MaxTurnedPoint().ID[5] || currentFloor.ID[5] == Floors.Last().ID[5])
                        {
                            item.UpDown = "DOWN";
                            item.ElevatorSpeed = -1;
                        }
                        else if (currentFloor.ID[5] == item.MinTurnedPoint().ID[5] || currentFloor.ID[5] == Floors.First().ID[5])
                        {
                            item.UpDown = "UP";
                            item.ElevatorSpeed = 1;
                        }


                        var currentQuery = QueryController.GetQuery(currentFloor);
                        List<Person> currentQueryToTheElevator = currentQuery.PeopleInQueue[item];


                        for(int i = 0; i < currentQueryToTheElevator.Count; ++i)
                        {
                            if((item.CurrentWeigh + currentQueryToTheElevator[i].Weigh) < item.MaxWeigh)
                            {
                                if(currentQueryToTheElevator[i].FloorIntention.ID[5] > currentFloor.ID[5] && item.UpDown == "UP")
                                {
                                    person.Add(build.Children.OfType<Rectangle>().Where(p => (string)p.Name == currentQueryToTheElevator[i].Name).First());
                                    item.Filling(currentQueryToTheElevator[i]);
                                    item.QueueOfRequests.Remove(currentFloor);
                                    item.QueueFromInside.Add(currentQueryToTheElevator[i].FloorIntention);
                                    currentQueryToTheElevator.Remove(currentQueryToTheElevator[i]);
                                }
                                else if(currentQueryToTheElevator[i].FloorIntention.ID[5] < currentFloor.ID[5] && item.UpDown == "DOWN")
                                {
                                    person.Add(build.Children.OfType<Rectangle>().Where(p => (string)p.Name == currentQueryToTheElevator[i].Name).First());
                                    item.Filling(currentQueryToTheElevator[i]);
                                    item.QueueOfRequests.Remove(currentFloor);
                                    item.QueueFromInside.Add(currentQueryToTheElevator[i].FloorIntention);
                                    currentQueryToTheElevator.Remove(currentQueryToTheElevator[i]);
                                }
                            }
                        }
                        var peopleToExit = item.PeopleInsideElevator.Where(p => p.FloorIntention == currentFloor).ToList();
                        for (int i = 0; i < peopleToExit.Count; ++i)
                        {
                            item.QueueFromInside.Remove(peopleToExit[i].FloorIntention);
                            item.ExitFromElevator(peopleToExit[i]);
                        }
                    }
                }
                for (int i = 0; i < person.Count; i++)
                {
                    build.Children.Remove(person[i]);
                }
            }
        }

        private void SetRun()
        {
            foreach(var item in Elevators)
            {
                item.UpDown = "UP";
            }
        }       

        private void btn_RunModel_Click(object sender, RoutedEventArgs e)
        {
            ThresholdReached = passengerGeneratorEvent;
            ThresholdReached?.Invoke(this, e);
            passengersGeneratorTimer.Tick -= passengerGeneratorEvent;
            passengersGeneratorTimer.Tick += passengerGeneratorEvent;
            passengersGeneratorTimer.Interval = TimeSpan.FromSeconds(5);
            passengersGeneratorTimer.Start();

            gameTimer.Tick -= gameElevatorEvent;
            gameTimer.Tick += gameElevatorEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(1);
            SetRun();
            gameTimer.Start();
        }


        private void btn_BackToInputMenu_Click(object sender, RoutedEventArgs e)
        {
            InputMenu inputMenu = new InputMenu();
            inputMenu.Show();
            this.Close();
        }

        private void btn_StopModel_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Tick -= gameElevatorEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(0);
            gameTimer.Stop();
        }

        private void show_ElevatorsInfo_Click(object sender, RoutedEventArgs e)
        {
            string str = "";
            foreach (var item in Elevators)
            {
                str += $"{item} elevator, max weigh {item.MaxWeigh}, current weigh: {item.CurrentWeigh}, MAX TURNED FLOOR: {item.MaxTurnedPoint()}, MIN TURNED FLOOR {item.MinTurnedPoint()} \n";
                foreach (var people in item.PeopleInsideElevator)
                {
                    str += "    " + people.Name + ", Intesion floor: " + people.FloorIntention.ID + ", weigh: " + people.Weigh.ToString() + "\n";
                }
                str += "\n";
            }
            MessageBox.Show(str);
        }

        private void btn_OutPutQueue_Click(object sender, RoutedEventArgs e)
        {
            string str = "";
            foreach (var item in Elevators) 
            {
                str += $"{item} elevator, Queue: \n";
                foreach (var people in item.QueueOfRequests)
                {
                    str += "    " + people.ID + "\n";
                }
                str += "\n";
            }
            MessageBox.Show(str);
        }
    }
}
