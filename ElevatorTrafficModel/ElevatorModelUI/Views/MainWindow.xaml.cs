using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ElevatorModelBL.Controllers;
using ElevatorModelBL.Models;
using ElevatorModelBL.Enums;
using System.Data;


namespace ElevatorModelUI
{
    /// <summary>
    /// Interaction logic for WorkingSpace.xaml
    /// </summary>
    /// 

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




        PeopleGenerator generator;
        QueryController QueryController;
        List<Elevator> Elevators;
        List<Person> People;
        List<Floor> Floors;
        int FloorsCount;

        DispatcherTimer gameTimer = new DispatcherTimer();
        DispatcherTimer passengersGeneratorTimer = new DispatcherTimer();
        DispatcherTimer tableUpdate = new DispatcherTimer();

        DefaultDialogService defaultDialogService = new DefaultDialogService();
        JsonFileService jsonFileService = new JsonFileService();
        public event EventHandler PassengersHandler;
        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            var Result = MessageBox.Show("Do you want use default data? Be careful, you will not be able to change names after current decision.", 
                "File reading",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)
            {
                generator = new PeopleGenerator();
            }
            else
            {
                if (defaultDialogService.OpenFileDialog())
                {
                    generator = new PeopleGenerator(defaultDialogService.FilePath);
                }
                else
                {
                    generator = new PeopleGenerator();
                }
            }

            People = new List<Person>();
            Floors = new List<Floor>();

            myCanvas.Focus();
            RunDataTableRefreshing();


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

                Rectangle item = build.Children.OfType<Rectangle>().Where(p=>(string)p.Tag == "floorItem").LastOrDefault();
                Canvas.SetLeft(floor, 0);
                if (counter != 1)
                {
                    Canvas.SetBottom(floor, Canvas.GetBottom(item) + 75);

                }
                else
                {
                    Canvas.SetBottom(floor, 0);

                }

                build.Children.Add(floor);
                Floors.Add(new Floor() { ID = floor.Name });

                List<Rectangle> pictures = new List<Rectangle>();
                List<Rectangle> windows = new List<Rectangle>();
                for (int j = 0; j < Elevators.Count; ++j)
                {
                    pictures.Add(new Rectangle()
                    {
                        Width = 30,
                        Height = 20,
                        Fill = PictureSprite,
                        Tag = "pictureItem"
                    });

                    windows.Add(new Rectangle()
                    {
                        Width = 25,
                        Height = 35,
                        Fill = WindowSprite,
                        Tag = "windowsItem"
                    });
                }

                Rectangle lastAddedPicture = build.Children.OfType<Rectangle>().Where(p => (string)p.Tag == "floorItem").LastOrDefault();
                int leftPadding = 53;
                for (int j = 0; j < Elevators.Count; ++j)
                {
                    Canvas.SetLeft(pictures[j], leftPadding);
                    Canvas.SetLeft(windows[j], leftPadding-35);
                    leftPadding += 170; 
                    Canvas.SetBottom(pictures[j], Canvas.GetBottom(lastAddedPicture) + 35);
                    Canvas.SetBottom(windows[j], Canvas.GetBottom(lastAddedPicture) + 25);

                    build.Children.Add(pictures[j]);
                    build.Children.Add(windows[j]);
                }
            }
        }

      
        private void MakeElevators()
        {
            int countOfElevators = 0;
            foreach (var Elevator in Elevators)
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
        // DONE: додати трайкетч на перевірку файлу з некоректними даними,  і перевіряти трайкетчем чи існує файл, якщо файлу не існує, то виводити повідомлення через трай кетч.

        // TODO: реалізувати продовження руху ліфта після зупинки


        //private void btn_GeneratePassangers_Click(object sender, RoutedEventArgs e)
        //{
        //    List<Person> people = generator.GetPassangers(Floors);

        //    string str = "";
        //    foreach (var item in people) 
        //    {
        //        str += $"{item.Name}, current floor: {item.CurrentFloor}\n";

        //    }



        //    QueryController.Add(people, Elevators);

        //    MakeRequests(people);


        //    foreach (var item in QueryController.Queries)
        //    {
        //        foreach(var elevator in item.PeopleInQueue)
        //        {
        //            int countInQueue = 0;
        //            foreach (var reload in elevator.Value)
        //            {
        //                build.Children.Remove(build.Children.OfType<Rectangle>().Where(p => p.Name == reload.Name.ToString()).FirstOrDefault());
        //            }
        //            foreach (var personInQueue in elevator.Value)
        //            {


        //                Rectangle person = new Rectangle()
        //                {
        //                    Name = personInQueue.Name,
        //                    Height = 40,
        //                    Width = 25,
        //                    Fill = personInQueue.Sex == "Woman" ? GirlSprite : BoySprite,
        //                    Tag = "passenger" + elevator.Key.ID + item.NumberOfFloor,
        //                    ToolTip = new ToolTip{ Content = personInQueue.Name + ", floor intension: " + personInQueue.FloorIntention + ", weight: " + personInQueue.Weigh},
        //                };


        //                var currentPosition = build.Children.OfType<Rectangle>().Where(p => p.Name == personInQueue.CurrentFloor.ToString()).FirstOrDefault();
        //                var currentElevatorQueue = build.Children.OfType<Rectangle>().Where(p => p.Name == elevator.Key.ID).FirstOrDefault();

        //                Canvas.SetLeft(person, Canvas.GetLeft(currentElevatorQueue)- 30- countInQueue * 25);
        //                Canvas.SetBottom(person, Canvas.GetBottom(currentPosition) + 7);
        //                build.Children.Add(person);
        //                countInQueue++;
        //            }

        //        }

        //    }
        //    foreach(var person in people)
        //    {
        //        People.Add(person);
        //    }
        //}

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
                                    item.AllPeoplesUsedElevator.Add(currentQueryToTheElevator[i]);
                                    item.QueueOfRequests.Remove(currentFloor);
                                    item.QueueFromInside.Add(currentQueryToTheElevator[i].FloorIntention);
                                    currentQueryToTheElevator.Remove(currentQueryToTheElevator[i]);
                                }
                                else if(currentQueryToTheElevator[i].FloorIntention.ID[5] < currentFloor.ID[5] && item.UpDown == "DOWN")
                                {
                                    person.Add(build.Children.OfType<Rectangle>().Where(p => (string)p.Name == currentQueryToTheElevator[i].Name).First());
                                    item.Filling(currentQueryToTheElevator[i]);
                                    item.AllPeoplesUsedElevator.Add(currentQueryToTheElevator[i]);
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
                item.ElevatorSpeed = 1;
                item.UpDown = "UP";
            }
        }       

        

        private void btn_BackToInputMenu_Click(object sender, RoutedEventArgs e)
        {
            InputMenu inputMenu = new InputMenu();
            inputMenu.Show();
            this.Close();
        }
   
       
        public class DataForGread
        {
            public string Name { get; set; }
            public List<Person> PopleInside { get; set; }
            public double CurrentWeigh { get; set; }
            public double MaxWeight { get; set; }
            public ElevatorType TypeOfElevator { get; set; }
            public string FloorDirection { get; set; }
        }


        void RunDataTableRefreshing()
        {
            tableUpdate.Tick -= DataTableRefreshing;
            tableUpdate.Tick += DataTableRefreshing;
            tableUpdate.Interval = TimeSpan.FromMilliseconds(700);
            tableUpdate.Start();
        }

        void StopDataTableRefreshing()
        {
            tableUpdate.Tick -= DataTableRefreshing;
            tableUpdate.Interval = TimeSpan.FromSeconds(0);
            tableUpdate.Stop();
        }

        public List<DataForGread> DataGridElevators { get; set; }

        private void DataTableRefreshing(object sender, EventArgs e)
        {
            DataGridElevators = new List<DataForGread>();
            foreach (var item in Elevators)
            {
                DataGridElevators.Add(new DataForGread() { 
                    Name = item.ID, 
                    PopleInside = item.PeopleInsideElevator, 
                    CurrentWeigh = Math.Round(item.CurrentWeigh,2), 
                    MaxWeight = item.MaxWeigh,
                    TypeOfElevator = item.TypeOfElevator,
                    FloorDirection = item.UpDown
                });
            }
            myGrid.ItemsSource = DataGridElevators;
        }


        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Restart_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
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

        private void btn_RunElevators_Click(object sender, RoutedEventArgs e)
        {
            RunDataTableRefreshing();
            gameTimer.Tick -= gameElevatorEvent;
            gameTimer.Tick += gameElevatorEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(5);
            SetRun();
            gameTimer.Start();
        }

        private void btn_StopElevators_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Tick -= gameElevatorEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(0);
            gameTimer.Stop();
            StopDataTableRefreshing();
        }

        private void btn_RunPassengerGenerator_Click(object sender, RoutedEventArgs e)
        {
            RunDataTableRefreshing();
            PassengersHandler = passengerGeneratorEvent;
            PassengersHandler?.Invoke(this, e);
            passengersGeneratorTimer.Tick -= passengerGeneratorEvent;
            passengersGeneratorTimer.Tick += passengerGeneratorEvent;
            passengersGeneratorTimer.Interval = TimeSpan.FromSeconds(5);
            passengersGeneratorTimer.Start();
        }

        private void btn_StopPassengerGenerator_Click(object sender, RoutedEventArgs e)
        {
            passengersGeneratorTimer.Tick -= passengerGeneratorEvent;
            passengersGeneratorTimer.Interval = TimeSpan.FromSeconds(0);
            passengersGeneratorTimer.Stop();
            StopDataTableRefreshing();
        }

        private void btn_Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is a simulation model of elevators in high-rise buildings." +
                " The user are able to specify the number of floors in the building, the types of elevators and their number." +
                " The building can randomly generate future passengers with a given weight and with the intention to go to one or another floor." +
                " Passengers can line up in the elevators on the principle of the most even distribution in the queues. When loading the elevator, " +
                "the total weight that can be transported is taken into account. In case of elevator overload, its movement will not be allowed." +
                " If the elevator is not fully loaded, it can serve a call on one of the intermediate floors of its path. Elevator calls are grouped into queues with priorities" +
                ". The program can graphically display the proposed model.",
                "Model of elevators", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btn_AboutUs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Creator: Roman Alberda\nGroup: SI-21\nCompany: RamzesStudio\nFeedback: +380-96-465-4324", "Model of elevators", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btn_LoadFileWithNames(object sender, RoutedEventArgs e)
        {
            if (defaultDialogService.OpenFileDialog())
            {
                MessageBox.Show(defaultDialogService.FilePath);
                generator.ChoosedNameDeserializer(defaultDialogService.FilePath);

            }
        }

        private void btn_SaveInfoIntoFile(object sender, RoutedEventArgs e)
        {
            var Result = MessageBox.Show("Do you want to save data in default file?", "File reading", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)
            {
                var exePath = AppDomain.CurrentDomain.BaseDirectory;
                var path = System.IO.Path.Combine(exePath, "Result.txt");

                jsonFileService.Save(path, Elevators);
            }
            else
            {
                if (defaultDialogService.SaveFileDialog())
                {
                    var path = defaultDialogService.FilePath;
                    jsonFileService.Save(path, Elevators);
                }
                else
                {
                    var exePath = AppDomain.CurrentDomain.BaseDirectory;
                    var path = System.IO.Path.Combine(exePath, "Result.txt");
                    jsonFileService.Save(path, Elevators);
                }
            }
        }
    }
}
