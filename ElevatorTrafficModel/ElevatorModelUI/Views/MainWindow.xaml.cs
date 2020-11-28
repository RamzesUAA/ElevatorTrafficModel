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
using ElevatorModelBL.Services;
using ElevatorModelBL.Additional_models;

namespace ElevatorModelUI
{
    public partial class MainWindow : Window
    {
        private readonly ImageBrush _hydraulicElevator = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/HydraulicElevator.png"))
        };

        private readonly ImageBrush _machineRoomElevator = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/MachineRoomElevator.png"))
        };

        private readonly ImageBrush _tractionElevator = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/TractionElevator.png"))
        };

        private readonly ImageBrush _floorSprite = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Floor.png"))
        };

        private readonly ImageBrush _boySprite = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Boy.png"))
        };

        private readonly ImageBrush _girlSprite = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Girl.png"))
        };

        private readonly ImageBrush _pictureSprite = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/PictureWall.jpg"))
        };

        private readonly ImageBrush _windowSprite = new ImageBrush()
        {
            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Window.jpg"))
        };


        private readonly PeopleGenerator _generator;
        private readonly QueueController _queryController;
        private readonly List<Elevator> _elevators;
        private readonly List<Floor> _floors;
        public List<DataForTable> DataGridElevators { get; set; }
        private readonly int _floorsCount;

        private readonly DispatcherTimer _gameTimer = new DispatcherTimer();
        private readonly DispatcherTimer _passengersGeneratorTimer = new DispatcherTimer();
        private readonly DispatcherTimer _tableUpdate = new DispatcherTimer();

        private readonly DefaultDialogService _defaultDialogService = new DefaultDialogService();
        private readonly JsonFileService _jsonFileService = new JsonFileService();
        public event EventHandler PassengersHandler;
        /// <summary>
        /// Startup point of the program.
        /// </summary>
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            var result = MessageBox.Show("Do you want to use default data?", 
                "File reading",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _generator = new PeopleGenerator();
            }
            else
            {
                _generator = _defaultDialogService.OpenFileDialog() ? new PeopleGenerator(_defaultDialogService.FilePath) : new PeopleGenerator();
            }

            _floors = new List<Floor>();

            myCanvas.Focus();
            RunDataTableRefreshing();


        }
        public MainWindow(List<Elevator> elevators, int countOfFloors):this()
        {
            _elevators = elevators;
            _floorsCount = countOfFloors;
            MakeFloors();
            MakeElevators();
            _queryController = new QueueController(_elevators, _floors);
        }
        /// <summary>
        /// Method which generate floors on the map.
        /// </summary>
        private void MakeFloors()
        {
            var counter = 0;

            for (var i = 0; i < _floorsCount; ++i)
            {
                counter++;
                var floor = new Rectangle()
                {
                    Name = "floor" + counter,
                    Height = 10,
                    Width = 800,
                    Fill = _floorSprite,
                    Tag = "floorItem"
                };

                var item = build.Children.OfType<Rectangle>().LastOrDefault(p => (string)p.Tag == "floorItem");
                Canvas.SetLeft(floor, 0);
                if (counter != 1)
                {
                    Canvas.SetBottom(floor, Canvas.GetBottom(item ?? throw new InvalidOperationException()) + 75);
                }
                else
                {
                    Canvas.SetBottom(floor, 0);
                }

                build.Children.Add(floor);
                _floors.Add(new Floor() { Id = floor.Name });

                var pictures = new List<Rectangle>();
                var windows = new List<Rectangle>();
                for (int j = 0; j < _elevators.Count; ++j)
                {
                    pictures.Add(new Rectangle()
                    {
                        Width = 30,
                        Height = 20,
                        Fill = _pictureSprite,
                        Tag = "pictureItem"
                    });

                    windows.Add(new Rectangle()
                    {
                        Width = 25,
                        Height = 35,
                        Fill = _windowSprite,
                        Tag = "windowsItem"
                    });
                }

                var lastAddedPicture = build.Children.OfType<Rectangle>().LastOrDefault(p => (string)p.Tag == "floorItem");
                var leftPadding = 53;
                for (var j = 0; j < _elevators.Count; ++j)
                {
                    Canvas.SetLeft(pictures[j], leftPadding);
                    Canvas.SetLeft(windows[j], leftPadding-35);
                    leftPadding += 170; 
                    Canvas.SetBottom(pictures[j], Canvas.GetBottom(lastAddedPicture ?? throw new InvalidOperationException()) + 35);
                    Canvas.SetBottom(windows[j], Canvas.GetBottom(lastAddedPicture) + 25);

                    build.Children.Add(pictures[j]);
                    build.Children.Add(windows[j]);
                }
            }
        }
        /// <summary>
        /// Method which generate elevators on the map.
        /// </summary>
        private void MakeElevators()
        {
            int countOfElevators = 0;
            foreach (var elevator in _elevators)
            {
                var newElevator = new Rectangle();
                countOfElevators++;
                switch (elevator.TypeOfElevator)
                {
                    case ElevatorType.Hydraulic:
                        newElevator.Name = elevator.Id;
                        newElevator.Height = 50;
                        newElevator.Width = 30;
                        newElevator.Fill = _hydraulicElevator;
                        newElevator.Tag = "elevatorItem";
                        break;
                    case ElevatorType.MachineRoom:
                        newElevator.Name = elevator.Id;
                        newElevator.Height = 50;
                        newElevator.Width = 30;
                        newElevator.Fill = _machineRoomElevator;
                        newElevator.Tag = "elevatorItem";
                        break;
                    case ElevatorType.Traction:
                        newElevator.Name = elevator.Id;
                        newElevator.Height = 50;
                        newElevator.Width = 30;
                        newElevator.Fill = _tractionElevator;
                        newElevator.Tag = "elevatorItem";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var item = build.Children.OfType<Rectangle>().Last();
                Canvas.SetBottom(newElevator, 0);
                if (countOfElevators != 1)
                {
                    Canvas.SetLeft(newElevator, Canvas.GetLeft(item) + 152);
                }
                else
                {
                    Canvas.SetLeft(newElevator, 146);
                }
                build.Children.Add(newElevator);
            }
        }
        /// <summary>
        /// Method which makes requests from outside of the elevators.
        /// </summary>
        /// <param name="people"></param>
        private void MakeRequests(IReadOnlyCollection<Person> people)
        {
            foreach (var elevator in _elevators)
            {
                foreach (var person in people)
                {
                    foreach (var peopleInQueue in _queryController.Queues)
                    {
                        foreach (var personInQueue in peopleInQueue.PeopleInQueue[elevator])
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
        /// <summary>
        /// Event that generates people on a map over a set period.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassengerGeneratorEvent(object sender, EventArgs e)
        {
            var people = _generator.GetPassengers(_floors);
            var str = "";
            foreach (var item in people)
            {
                str += $"{item.Name}, current floor: {item.CurrentFloor}\n";

            }
            _queryController.Add(people);

            MakeRequests(people);

            foreach (var item in _queryController.Queues)
            {
                foreach (var elevator in item.PeopleInQueue)
                {
                    var countInQueue = 0;
                    foreach (var reload in elevator.Value)
                    {
                        build.Children.Remove(build.Children.OfType<Rectangle>().FirstOrDefault(p => p.Name == reload.Name.ToString()));
                    }
                    foreach (var personInQueue in elevator.Value)
                    {
                        Rectangle person = new Rectangle()
                        {
                            Name = personInQueue.Name,
                            Height = 40,
                            Width = 25,
                            Fill = personInQueue.Sex == "Woman" ? _girlSprite : _boySprite,
                            Tag = "passenger",
                            ToolTip = new ToolTip { Content = personInQueue.Name + ", floor intention: " + personInQueue.FloorIntention + ", weight: " + personInQueue.Weight },
                        };

                        var currentPosition = build.Children.OfType<Rectangle>().FirstOrDefault(p => p.Name == personInQueue.CurrentFloor.ToString());
                        var currentElevatorQueue = build.Children.OfType<Rectangle>().FirstOrDefault(p => p.Name == elevator.Key.Id);

                        Canvas.SetLeft(person, Canvas.GetLeft(currentElevatorQueue ?? throw new InvalidOperationException()) - 30 - countInQueue * 25);
                        Canvas.SetBottom(person, Canvas.GetBottom(currentPosition ?? throw new InvalidOperationException()) + 7);
                        build.Children.Add(person);
                        countInQueue++;
                    }
                }
            }
        }
        /// <summary>
        /// Event which moves elevators on the map. In this event realized main logic of interaction between people, elevators and floor entities.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameElevatorEvent(object sender, EventArgs e)
        {
            foreach (var item in _elevators)
            {
                var rectangleElevator = build.Children.OfType<Rectangle>().First(p => p.Name == item.Id);
                Canvas.SetBottom(rectangleElevator, Canvas.GetBottom(rectangleElevator) + item.ElevatorSpeed);
                var person = new List<Rectangle>();
                foreach (var floor in build.Children.OfType<Rectangle>().Where(p => (string)p.Tag == "floorItem"))
                {
                    floor.Stroke = Brushes.Black;
                    var elevatorHitBox = new Rect(Canvas.GetLeft(rectangleElevator), Canvas.GetBottom(rectangleElevator), rectangleElevator.Width, rectangleElevator.Height);
                    var floorHitBox = new Rect(Canvas.GetLeft(floor), Canvas.GetBottom(floor), floor.Width, floor.Height);

                    if (elevatorHitBox.IntersectsWith(floorHitBox))
                    {
                        var currentFloor = _floors.First(p => p.Id == (string)floor.Name);

                        if (item.MaxTurnedPoint() == null && !item.PeopleInsideElevator.Any() && !item.QueueOfRequests.Any())
                        {
                            item.UpDown = "Stopped";
                            item.ElevatorSpeed = 0;
                        }
                        else if (item.QueueOfRequests.Count() != 0 && item.UpDown == "Stopped")
                        {
                            if (currentFloor.Id[5] > item.QueueOfRequests.First().Id[5])
                            {
                                item.UpDown = "DOWN";
                                item.ElevatorSpeed = -1;
                            }
                            else
                            {
                                item.UpDown = "UP";
                                item.ElevatorSpeed = 1;
                            }
                        }
                        else if (!item.QueueFromInside.Any() && item.QueueOfRequests.Count() != 0 && currentFloor.Id[5] == item.MaxTurnedPoint().Id[5])
                        {
                            item.UpDown = item.UpDown;
                            item.ElevatorSpeed = item.ElevatorSpeed;
                        }
                        else if (currentFloor.Id[5] == item.MaxTurnedPoint().Id[5])
                        {
                            item.UpDown = "DOWN";
                            item.ElevatorSpeed = -1;
                        }
                        else if (currentFloor.Id[5] == item.MinTurnedPoint().Id[5])
                        {
                            item.UpDown = "UP";
                            item.ElevatorSpeed = 1;
                        }
                    
                        if (currentFloor.Id[5] == _floors.Last().Id[5] && item.UpDown != "Stopped" && item.QueueOfRequests.Count() != 0 && !item.QueueFromInside.Any()) 
                        {
                            item.UpDown = "DOWN";
                            item.ElevatorSpeed = -1;
                        }
                        else if (currentFloor.Id[5] == _floors.First().Id[5] && item.UpDown != "Stopped" && item.QueueOfRequests.Count() != 0 && !item.QueueFromInside.Any())
                        {
                            item.UpDown = "UP";
                            item.ElevatorSpeed = 1;
                        }
                        else if (currentFloor.Id[5] == _floors.Last().Id[5] && item.UpDown != "Stopped" )
                        {
                            item.UpDown = "DOWN";
                            item.ElevatorSpeed = -1;
                        }
                        else if (currentFloor.Id[5] == _floors.First().Id[5] && item.UpDown != "Stopped")
                        {
                            item.UpDown = "UP";
                            item.ElevatorSpeed = 1;
                        }

                        var peopleToExit = item.PeopleInsideElevator.Where(p => p.FloorIntention == currentFloor).ToList();
                        foreach (var t in peopleToExit)
                        {
                            item.QueueFromInside.Remove(t.FloorIntention);
                            item.ExitFromElevator(t);
                        }

                        var currentQuery = _queryController.GetQuery(currentFloor);
                        var currentQueryToTheElevator = currentQuery.PeopleInQueue[item];
                        var deleteFromQueue = new List<Person>();

                        for (var j = 0; j < currentQueryToTheElevator.Count(); j++)
                        {
                            if (item.CurrentWeight + currentQueryToTheElevator[j].Weight < item.MaxWeight)
                            {
                                if(currentQueryToTheElevator[j].FloorIntention.Id[5] > currentFloor.Id[5] && item.UpDown == "UP")
                                {
                                    person.Add(build.Children.OfType<Rectangle>().FirstOrDefault(p => (string)p.Name == currentQueryToTheElevator[j].Name));
                                    item.Filling(currentQueryToTheElevator[j]);
                                    item.AllPeoplesUsedElevator.Add(currentQueryToTheElevator[j]);
                                    item.QueueOfRequests.Remove(currentFloor);
                                    item.QueueFromInside.Add(currentQueryToTheElevator[j].FloorIntention);
                                    deleteFromQueue.Add(currentQueryToTheElevator[j]);
                                }
                                else if (currentQueryToTheElevator[j].FloorIntention.Id[5] < currentFloor.Id[5] && item.UpDown == "DOWN")
                                {
                                    person.Add(build.Children.OfType<Rectangle>().FirstOrDefault(p => (string)p.Name == currentQueryToTheElevator[j].Name));
                                    item.Filling(currentQueryToTheElevator[j]);
                                    item.AllPeoplesUsedElevator.Add(currentQueryToTheElevator[j]);
                                    item.QueueOfRequests.Remove(currentFloor);
                                    item.QueueFromInside.Add(currentQueryToTheElevator[j].FloorIntention);
                                    deleteFromQueue.Add(currentQueryToTheElevator[j]);
                                }
                            }
                        }
                        for (var i = 0; i < deleteFromQueue.Count; ++i)
                        {
                            currentQueryToTheElevator.Remove(deleteFromQueue[i]);
                        }
                    }
                }
                for (var i = 0; i < person.Count; i++)
                {
                    build.Children.Remove(person[i]);
                }
            }
        }

        //------------------------------------------------------------------------------------------
        private void Btn_BackToInputMenu_Click(object sender, RoutedEventArgs e)
        {
            InputMenu inputMenu = new InputMenu();
            inputMenu.Show();
            this.Close();
        }
        //------------------------------------------------------------------------------------------
        void RunDataTableRefreshing()
        {
            _tableUpdate.Tick -= DataTableRefreshing;
            _tableUpdate.Tick += DataTableRefreshing;
            _tableUpdate.Interval = TimeSpan.FromMilliseconds(700);
            _tableUpdate.Start();
        }
        //------------------------------------------------------------------------------------------
        void StopDataTableRefreshing()
        {
            _tableUpdate.Tick -= DataTableRefreshing;
            _tableUpdate.Interval = TimeSpan.FromSeconds(0);
            _tableUpdate.Stop();
        }
        //------------------------------------------------------------------------------------------
        private void DataTableRefreshing(object sender, EventArgs e)
        {
            DataGridElevators = new List<DataForTable>();
            foreach (var item in _elevators)
            {
                DataGridElevators.Add(new DataForTable() { 
                    Name = item.Id,
                    PeopleInside = item.PeopleInsideElevator, 
                    CurrentWeigh = Math.Round(item.CurrentWeight, 2), 
                    MaxWeight = item.MaxWeight,
                    TypeOfElevator = item.TypeOfElevator,
                    FloorDirection = item.UpDown
                });
            }
            myGrid.ItemsSource = DataGridElevators;
        }
        //------------------------------------------------------------------------------------------
        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //------------------------------------------------------------------------------------------
        private void Btn_RunElevators_Click(object sender, RoutedEventArgs e)
        {
            RunDataTableRefreshing();
            _gameTimer.Tick -= GameElevatorEvent;
            _gameTimer.Tick += GameElevatorEvent;
            _gameTimer.Interval = TimeSpan.FromMilliseconds(5);
            _gameTimer.Start();
        }
        //------------------------------------------------------------------------------------------
        private void Btn_StopElevators_Click(object sender, RoutedEventArgs e)
        {
            _gameTimer.Tick -= GameElevatorEvent;
            _gameTimer.Interval = TimeSpan.FromMilliseconds(0);
            _gameTimer.Stop();
            StopDataTableRefreshing();
        }
        //------------------------------------------------------------------------------------------
        private void Btn_RunPassengerGenerator_Click(object sender, RoutedEventArgs e)
        {
            PassengersHandler = PassengerGeneratorEvent;
            PassengersHandler?.Invoke(this, e);
            _passengersGeneratorTimer.Tick -= PassengerGeneratorEvent;
            _passengersGeneratorTimer.Tick += PassengerGeneratorEvent;
            _passengersGeneratorTimer.Interval = TimeSpan.FromSeconds(5);
            _passengersGeneratorTimer.Start();
        }
        //------------------------------------------------------------------------------------------
        private void Btn_StopPassengerGenerator_Click(object sender, RoutedEventArgs e)
        {
            _passengersGeneratorTimer.Tick -= PassengerGeneratorEvent;
            _passengersGeneratorTimer.Interval = TimeSpan.FromSeconds(0);
            _passengersGeneratorTimer.Stop();
        }
        //------------------------------------------------------------------------------------------
        private void Btn_Info_Click(object sender, RoutedEventArgs e)
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
        //------------------------------------------------------------------------------------------
        private void Btn_AboutUs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Creator: Roman Alberda\nGroup: SI-21\nCompany: RamzesStudio\nFeedback: +380-96-465-4324", "Model of elevators", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //------------------------------------------------------------------------------------------
        private void Btn_LoadFileWithNames(object sender, RoutedEventArgs e)
        {
            if (_defaultDialogService.OpenFileDialog())
            {
                _generator.SelectedNameDeserializer(_defaultDialogService.FilePath);
            }
        }
        //------------------------------------------------------------------------------------------
        private void Btn_SaveInfoIntoFile(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you want to save data in default file?", "File reading", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var exePath = AppDomain.CurrentDomain.BaseDirectory;
                var path = System.IO.Path.Combine(exePath, "Result.txt");

                _jsonFileService.Save(path, _elevators);
            }
            else
            {
                if (_defaultDialogService.SaveFileDialog())
                {
                    var path = _defaultDialogService.FilePath;
                    _jsonFileService.Save(path, _elevators);
                }
                else
                {
                    var exePath = AppDomain.CurrentDomain.BaseDirectory;
                    var path = System.IO.Path.Combine(exePath, "Result.txt");
                    _jsonFileService.Save(path, _elevators);
                }
            }
        }
    }
}
