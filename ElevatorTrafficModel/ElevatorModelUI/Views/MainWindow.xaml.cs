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

        Generator generator = new Generator();
        QueryController QueryController;
        //ElevatorController ElevatorController = new ElevatorController();
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
                    Canvas.SetBottom(floor, Canvas.GetBottom(item) + 50);
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



        private void btn_GeneratePassangers_Click(object sender, RoutedEventArgs e)
        {
            List<Person> people = generator.GetPassangers(Floors);

            string str = "";
            foreach (var item in people) //, MAX TURNED FLOOR: {item.MaxTurnedPoint()}, MIN TURNED FLOOR {item.MinTurnedPoint()}
            {
                str += $"{item.Name}, current floor: {item.CurrentFloor}\n";

            }
            MessageBox.Show(str);

            
            foreach (var person1 in people)
            {
                QueryController.Add(person1, Elevators);
            }
            MakeRequests(people);


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
                            ToolTip = new ToolTip{ Content = personInQueue.Name + ", floor intension: " + personInQueue.FloorIntention + ", weight: " + personInQueue.Weigh},
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

        void DirectionSwitch(Elevator item)
        {
            if(item.UpDown == "Up")
            {
                item.UpDown = "Down";
            }
            else
            {
                item.UpDown = "Up";
            }
        }
        // TODO: витягнути з інтернету багато різних імен, сериалізувати їх в JSON формат і потім провести десериалізацію і рандомно присвоювати кожному об'єкту певне ім'я
        // зробити простий запис даних про стоврених пасажирів ( ім'я, вага, намір їхати на певний поверх, поточний поверх)
        // інформацію про ліфти (тип, вантажопідйомність)

        /// <summary>
        /// Логіка викликів ліфіта полягає в тому, що кожен ліфт рухається вниз і вгору, при чому він доїжджає до максимально чи мінімально викликаного поверху.
        /// На проміжних поверхах, він може відповідно висаджувати, або приймати в себе нових пасажирів, якшо їхій intensionFloor співпадає з напрямком руху ліфта, 
        /// та сумарна вага ліфта дозволяє прийняти нового пасажира
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        /*
    private void gameElevatorEvent(object sender, EventArgs e)
    {
        foreach(var item in Elevators)
        {
            var elevator = build.Children.OfType<Rectangle>().Where(p => p.Name == item.ID).First();
            Canvas.SetBottom(elevator, Canvas.GetBottom(elevator) + item.ElevatorSpeed);
            int directionFloor = int.Parse(item.CurrentDestination.ID[5].ToString());

           // MessageBox.Show(maxFloor.ToString());
            if (Canvas.GetBottom(elevator) < 0 || Canvas.GetBottom(elevator)  > 40 * (directionFloor -1)+ 9) 
            {
                DirectionSwitch(item);
                item.ElevatorSpeed = -item.ElevatorSpeed;
            }
            List<Rectangle> person = new List<Rectangle>();
            foreach (var floor in build.Children.OfType<Rectangle>().Where(p=>(string)p.Tag == "floorItem"))
            {
                floor.Stroke = Brushes.Black;

                Rect elevatorHitBox = new Rect(Canvas.GetLeft(elevator), Canvas.GetBottom(elevator), elevator.Width, elevator.Height);
                Rect floorHitBox = new Rect(Canvas.GetLeft(floor), Canvas.GetBottom(floor), floor.Width, floor.Height);

                if (elevatorHitBox.IntersectsWith(floorHitBox))
                {
                    // TODO: перевірити, чи хтось хоче виходити на поточному поверсі, якщо ні, то не зупинятись, або, якщо є вільно ще 70 кг
                    // то тоді зупинитись і перевірити першу людину в черзі, чи її вага є менша за 70 кг і чи вона хоче їхати в тому самому напрямку, куди і рухається 

                    var currentFloor = Floors.Where(p => p.ID == (string)floor.Name).First();
                    var currentQuery = QueryController.GetQuery(currentFloor);
                    List<Person> currentQueryToTheElevator = currentQuery.PeopleInQueue[item];

                    if (currentQueryToTheElevator == null)
                    {
                        continue;
                    }

                    for (int i = 0; i < currentQueryToTheElevator.Count; ++i)
                    {
                        if (ElevatorController.ElevatorFilling(item, currentQueryToTheElevator.First()) == true)
                        {
                            MessageBox.Show(currentQueryToTheElevator.First().Name.ToString());
                            person = build.Children.OfType<Rectangle>().Where(p => (string)p.Name == currentQueryToTheElevator.First().Name).ToList();
                            currentQueryToTheElevator.Remove(currentQueryToTheElevator.First());
                        }
                        else
                        {
                            break;
                        }
                        //checkInside(currentQueryToTheElevator);
                    }
                    var peopleToExit = item.PeopleInsideElevator.Where(p => p.FloorIntention == currentFloor).ToList();
                    for (int i = 0; i < peopleToExit.Count; ++i)
                    {
                        ElevatorController.ExitFromElevator(currentFloor, peopleToExit.First(), item);
                    }

                    // TODO: зробити розсинхрон ліфів + додати перевірку, щоб вони доїжджали до максимального поверха, який є викликаний, а не до максимального пверху в будинку

                    // MessageBox.Show($"{elevator.Name} on the {floor.Name}");
                    // elevatorSpeed = 0;
                    // Canvas.SetTop(player, Canvas.GetTop(floor) - player.Height);
                }

            }
            for (int i = 0; i < person.Count; i++)
            {
                build.Children.Remove(person.First());
            }
        }
    }
    */

        // TODO: розділити рух ліфта вниз і вгору !!!!!!! це має виправити баг з зависанням між поверхами і безкінечним виходом за карту


        /*
    void MoveUp(Elevator elevator)
    {
        elevator.ElevatorSpeed = 1;
    }
    void MoveDown(Elevator elevator)
    {
        elevator.ElevatorSpeed = -1;
    }
    void Stop(Elevator elevator)
    {
        elevator.ElevatorSpeed = 0;
    }


    private void gameElevatorEvent(object sender, EventArgs e)
    {
        foreach (var item in Elevators)
        {

            var elevator = build.Children.OfType<Rectangle>().Where(p => p.Name == item.ID).First();
            Canvas.SetBottom(elevator, Canvas.GetBottom(elevator) + item.ElevatorSpeed);
            List<Rectangle> PersonToDeleteFromModel = new List<Rectangle>();
            foreach (var floor in build.Children.OfType<Rectangle>().Where(p => (string)p.Tag == "floorItem"))
            {
                floor.Stroke = Brushes.Black;
                Rect elevatorHitBox = new Rect(Canvas.GetLeft(elevator), Canvas.GetBottom(elevator), elevator.Width, elevator.Height);
                Rect floorHitBox = new Rect(Canvas.GetLeft(floor), Canvas.GetBottom(floor), floor.Width, floor.Height);

                if (elevatorHitBox.IntersectsWith(floorHitBox))
                {
                    var currentFloor = Floors.Where(p => p.ID == (string)floor.Name).First();
                    int CURRENT_DIRECTION = item.GetCurrentDirection().ID[5];
                    if (currentFloor.ID[5] < CURRENT_DIRECTION)
                    {
                        item.UpDown = "UP";
                        MoveUp(item);
                    }else if(currentFloor.ID[5] > CURRENT_DIRECTION)
                    {
                        item.UpDown = "DOWN";
                        MoveDown(item);
                    }else if(item.GetCurrentDirection() == null)
                    {
                        Stop(item);
                    }

                    var currentQuery = QueryController.GetQuery(currentFloor);
                    List<Person> currentQueryToTheElevator = currentQuery.PeopleInQueue[item];
                    if (currentQueryToTheElevator == null)
                    {
                        continue;
                    }

                    for(int i = 0; i < currentQueryToTheElevator.Count; ++i)
                    {
                        if (ElevatorController.ElevatorFilling(item, currentQueryToTheElevator.First()) == true)
                        {
                            item.QueueOfRequests.Remove(currentFloor);
                            item.QueueFromInside.Add(currentQueryToTheElevator.First().FloorIntention);
                            MessageBox.Show(currentQueryToTheElevator.First().Name.ToString());
                            PersonToDeleteFromModel = build.Children.OfType<Rectangle>().Where(p => (string)p.Name == currentQueryToTheElevator.First().Name).ToList();
                            currentQueryToTheElevator.Remove(currentQueryToTheElevator.First());
                        }
                        else
                        {
                            break;
                        }
                    }
                    var peopleToExit = item.PeopleInsideElevator.Where(p => p.FloorIntention == currentFloor).ToList();
                    for (int i = 0; i < peopleToExit.Count; ++i)
                    {
                        item.QueueFromInside.Remove(peopleToExit.First().FloorIntention);
                        ElevatorController.ExitFromElevator(currentFloor, peopleToExit.First(), item);
                    }
                }
            }
            for (int i = 0; i < PersonToDeleteFromModel.Count; i++)
            {
                build.Children.Remove(PersonToDeleteFromModel.First());
            }
        }
    }
    */

        


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


                        if(item.MaxTurnedPoint()==null && item.PeopleInsideElevator.Count() == 0)
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


                        //if(currentFloor.ID[5] == item.MinTurnedPoint().ID[5])
                        //{
                        //    MessageBox.Show("Returned");
                        //    item.UpDown = "UP";
                        //    item.ElevatorSpeed = 1;
                        //}


                        var currentQuery = QueryController.GetQuery(currentFloor);
                        List<Person> currentQueryToTheElevator = currentQuery.PeopleInQueue[item];

                        //if (currentQueryToTheElevator == null)
                        //{
                        //    continue;
                        //}

                        for(int i = 0; i < currentQueryToTheElevator.Count; ++i)
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




        /*
    private void gameElevatorEvent(object sender, EventArgs e)
    {
        foreach (var item in Elevators)
        {

            var elevator = build.Children.OfType<Rectangle>().Where(p => p.Name == item.ID).First();
            Canvas.SetBottom(elevator, Canvas.GetBottom(elevator) + item.ElevatorSpeed);
            List<Rectangle> person = new List<Rectangle>();
            foreach (var floor in build.Children.OfType<Rectangle>().Where(p => (string)p.Tag == "floorItem"))
            {
                floor.Stroke = Brushes.Black;
                Rect elevatorHitBox = new Rect(Canvas.GetLeft(elevator), Canvas.GetBottom(elevator), elevator.Width, elevator.Height);
                Rect floorHitBox = new Rect(Canvas.GetLeft(floor), Canvas.GetBottom(floor), floor.Width, floor.Height);

                if (elevatorHitBox.IntersectsWith(floorHitBox))
                {
                    Floor CurrentDirection = item.GetCurrentDirection();

                    var currentFloor = Floors.Where(p => p.ID == (string)floor.Name).First();
                    int? directionFloor;

                    if (CurrentDirection != null)
                    {
                        directionFloor = int.Parse(CurrentDirection.ID[5].ToString());
                    }
                    else
                    {

                        item.ElevatorSpeed = 0;
                        directionFloor = null;

                    }

                    // int currentFloorInt = int.Parse(currentFloor.ID[5].ToString());
                    Floor isTurnedFloor = item.GetTurnedPoint();
                    if ((Canvas.GetBottom(elevator) < directionFloor || Canvas.GetBottom(elevator) > 40 * (directionFloor - 1) + 9) && currentFloor == isTurnedFloor || currentFloor == Floors.Last())
                    {
                        DirectionSwitch(item);
                        item.ElevatorSpeed = -1;
                    }

                    var currentQuery = QueryController.GetQuery(currentFloor);
                    List<Person> currentQueryToTheElevator = currentQuery.PeopleInQueue[item];

                    if (currentQueryToTheElevator == null)
                    {
                        continue;
                    }

                    for (int i = 0; i < currentQueryToTheElevator.Count; ++i)
                    {
                        if (ElevatorController.ElevatorFilling(item, currentQueryToTheElevator.First()) == true)
                        {
                            item.QueueOfRequests.Remove(currentFloor);
                            item.QueueFromInside.Add(currentQueryToTheElevator.First().FloorIntention);
                            MessageBox.Show(currentQueryToTheElevator.First().Name.ToString());
                            person = build.Children.OfType<Rectangle>().Where(p => (string)p.Name == currentQueryToTheElevator.First().Name).ToList();
                            currentQueryToTheElevator.Remove(currentQueryToTheElevator.First());
                        }
                        else
                        {
                            break;
                        }
                    }
                    var peopleToExit = item.PeopleInsideElevator.Where(p => p.FloorIntention == currentFloor).ToList();
                    for (int i = 0; i < peopleToExit.Count; ++i)
                    {
                        item.QueueFromInside.Remove(peopleToExit.First().FloorIntention);
                        ElevatorController.ExitFromElevator(currentFloor, peopleToExit.First(), item);
                    }

                }

            }
            for (int i = 0; i < person.Count; i++)
            {
                build.Children.Remove(person.First());
            }
        }
    }*/


        private void btn_RunModel_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Tick -= gameElevatorEvent;
            gameTimer.Tick += gameElevatorEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(1);
            gameTimer.Start();
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
            foreach (var item in Elevators) //, MAX TURNED FLOOR: {item.MaxTurnedPoint()}, MIN TURNED FLOOR {item.MinTurnedPoint()}
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
            foreach (var item in Elevators) //, MAX TURNED FLOOR: {item.MaxTurnedPoint()}, MIN TURNED FLOOR {item.MinTurnedPoint()}
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
