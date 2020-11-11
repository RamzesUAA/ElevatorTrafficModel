using ElevatorModelBL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorModelBL.Models
{
    public class Elevator
    {
        Random random = new Random();
        public Elevator()
        {
        }
        public string ID { get; set; }
        public ElevatorType TypeOfElevator {get;set;}
        public List<Person> PeopleInsideElevator = new List<Person>();
        public int MaxWeigh => (int)TypeOfElevator;
        public List<Floor> QueueOfRequests = new List<Floor>();
        public List<Floor> QueueFromInside = new List<Floor>();
        public int ElevatorSpeed { get; set; }
        public string UpDown { get; set; }
        public double CurrentWeigh
        {
            get
            {
                double currentWeigh = 0;
                foreach(var item in PeopleInsideElevator)
                {
                    currentWeigh += item.Weigh;
                }
                return currentWeigh;
            }
        }
        public Floor GetTurnedPoint()
        {
            Floor maxQueue = QueueOfRequests.FirstOrDefault();
            //int maxFromQueue = int.Parse(QueueOfRequests.First().ID[5].ToString());

            //foreach (var item in QueueOfRequests)
            //{
            //    if(maxFromQueue < (int)item.ID[5])
            //    {
            //        maxQueue = item;
            //    }
            //}


            if(QueueFromInside.Count == 0)
            {
                return maxQueue;
            }

            Floor maxInside = QueueFromInside.FirstOrDefault();
            int maxFromInside = int.Parse(QueueFromInside.First().ID[5].ToString());

            foreach (var item in QueueFromInside)
            {
                if (maxFromInside < (int)item.ID[5])
                {
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
        public Floor GetCurrentDirection()
        {
            if(QueueFromInside.Count != 0)
            {
                return QueueFromInside.FirstOrDefault();
            }
            else
            {
                return QueueOfRequests.FirstOrDefault();
            }
        }
        public Floor CurrentDestination 
        {
            get
            {
                Floor currentFloor = QueueOfRequests.FirstOrDefault();
                foreach (var item in QueueOfRequests)
                {
                    int current = int.Parse(item.ID[5].ToString());
                    int intesionFloor = int.Parse(currentFloor.ID[5].ToString());
                    if (UpDown == "Up")
                    {
                        if (intesionFloor > current)
                        {
                            currentFloor = item;
                        }
                    }
                    else if(UpDown == "Down")
                    {
                        if (intesionFloor < current)
                        {
                            currentFloor = item;
                        }
                    }
                }
                return currentFloor;
            }
        }
        public override string ToString()
        {
            return $"Elvator id: {this.ID}";
        }
        public string Some => GetPeopleInside();
        public string GetPeopleInside()
        {
            string str = "";
            foreach(var item in PeopleInsideElevator)
            {
                str += item.Name += ", ";
            }
            return str;
        }
    }
}
