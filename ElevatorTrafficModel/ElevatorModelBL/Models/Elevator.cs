using ElevatorModelBL.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorModelBL.Models
{
    public class Elevator
    {
        private Random random = new Random();
        public Elevator() { }
     
        private string id { get; set; }
      
        private List<Person> peopleInsideElevator = new List<Person>();
        private List<Floor> queueOfRequests = new List<Floor>();
        private List<Floor> queueFromInside = new List<Floor>();
        private List<Person> allPeoplesUsedElevator = new List<Person>();
        private ElevatorType typeOfElevator {get;set;}
        private int elevatorSpeed { get; set; }
        private string upDown { get; set; }
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public int MaxWeigh => (int)TypeOfElevator;
        public ElevatorType TypeOfElevator
        {
            get
            {
                return typeOfElevator;
            }
            set
            {
                typeOfElevator = value;
            }
        }
        public List<Person> AllPeoplesUsedElevator
        {
            get
            {
                return allPeoplesUsedElevator;
            }
            set
            {
                allPeoplesUsedElevator = value;
            }
        }
        [JsonIgnore]
        public List<Person> PeopleInsideElevator
        {
            get
            {
                return peopleInsideElevator;
            }
            set
            {
                peopleInsideElevator = value;
            }
        }
        [JsonIgnore]
        public List<Floor> QueueOfRequests
        {
            get
            {
                return queueOfRequests;
            }
            set
            {
                queueOfRequests = value;
            }
        }
        [JsonIgnore]
        public List<Floor> QueueFromInside
        {
            get
            {
                return queueFromInside;
            }
            set
            {
                queueFromInside = value;
            }
        }
        [JsonIgnore]
        public string UpDown
        {
            get
            {
                return upDown;
            }
            set
            {
                upDown = value;
            }
        }
        [JsonIgnore]
        public int ElevatorSpeed {
            get 
            {
                return elevatorSpeed;
            }
            set 
            {
                elevatorSpeed = value;
            }
        }
        [JsonIgnore]
        public double CurrentWeigh
        {
            get
            {
                double currentWeigh = 0;
                foreach (var item in PeopleInsideElevator)
                {
                    currentWeigh += item.Weigh;
                }
                return currentWeigh;
            }
        }


        public Floor MaxTurnedPoint()
        {
            Floor maxQueue = QueueOfRequests.FirstOrDefault();
            Floor maxInside = QueueFromInside.FirstOrDefault();

            //if (maxQueue == null && maxInside==null)
            //{
            //    return null;
            //}            

            if (maxQueue == null && maxInside != null)
            {
                int maxFromInsideTemp = int.Parse(QueueFromInside.First().ID[5].ToString());

                foreach (var item in QueueFromInside)
                {
                    int tempItem = int.Parse(item.ID[5].ToString());
                    if (maxFromInsideTemp < tempItem)
                    {
                        maxFromInsideTemp = tempItem;
                        maxInside = item;
                    }
                }
                return maxInside;
            }

            if (maxQueue == null && QueueFromInside.Count == 0)
            {
                return null;
            }


            int maxFromQueue= int.Parse(QueueOfRequests.First().ID[5].ToString());

            foreach (var item in QueueOfRequests)
            {
                int tempItem = int.Parse(item.ID[5].ToString());

                if (maxFromQueue < tempItem)
                {
                    maxFromQueue = tempItem;
                    maxQueue = item;
                }
            }


            if (QueueFromInside.Count == 0)
            {
                return maxQueue;
            }

         
            int maxFromInside = int.Parse(QueueFromInside.First().ID[5].ToString());

            foreach (var item in QueueFromInside)
            {
                int tempItem = int.Parse(item.ID[5].ToString());

                if (maxFromInside < tempItem)
                {
                    maxFromInside = tempItem;
                    maxInside = item;
                }
            }


            if (QueueOfRequests.Count == 0)
            {
                return maxInside;
            }


            if (maxQueue.ID[5] > maxInside.ID[5])
            {
                return maxQueue;
            }
            else
            {
                return maxInside;
            }
        }

        public Floor MinTurnedPoint()
        {
            Floor minQueue = QueueOfRequests.FirstOrDefault();
            Floor minInside = QueueFromInside.FirstOrDefault();

            if (minQueue == null && minInside != null)
            {
                int minFromInsideTemp = int.Parse(QueueFromInside.First().ID[5].ToString());

                foreach (var item in QueueFromInside)
                {
                    int tempItem = int.Parse(item.ID[5].ToString());
                    if (minFromInsideTemp > tempItem)
                    {
                        minFromInsideTemp = tempItem;
                        minInside = item;
                    }
                }
                return minInside;
            }

            if (minQueue == null && QueueFromInside.Count == 0)
            {
                return null;
            }

            int minFromQueue = int.Parse(QueueOfRequests.First().ID[5].ToString());

            foreach (var item in QueueOfRequests)
            {
                int tempItem = int.Parse(item.ID[5].ToString());

                if (minFromQueue > tempItem)
                {
                    minFromQueue = tempItem;
                    minQueue = item;
                }
            }

            if (QueueFromInside.Count == 0)
            {
                return minQueue;
            }

           
            int minFromInside = int.Parse(QueueFromInside.First().ID[5].ToString());

            foreach (var item in QueueFromInside)
            {
                int tempItem = int.Parse(item.ID[5].ToString());

                if (minFromInside > tempItem)
                {
                    minFromInside = tempItem;
                    minInside = item;
                }
            }


            if (QueueOfRequests.Count == 0)
            {
                return minInside;
            }


            if ((int)minQueue.ID[5] > (int)minInside.ID[5])
            {
                return minInside;
            }
            else
            {
                return minQueue;
            }
        }

        public void Filling(Person person)
        {
            this.PeopleInsideElevator.Add(person);
        }

        public void ExitFromElevator(Person person)
        {

            PeopleInsideElevator.Remove(person);

        }


        //public Floor GetCurrentDirection()
        //{
        //    if(QueueOfRequests.Count !=0)
        //    {
        //        return QueueOfRequests.FirstOrDefault();
        //    }
        //    else
        //    {
        //        return QueueFromInside.First();
        //    }
        //}
           
        public override string ToString()
        {
            return $"Elvator id: {this.ID}";
        }
    }
}
