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
        public Floor CurrentDestination 
        {
            get
            {
                Floor maxFloor = QueueOfRequests.FirstOrDefault();
                foreach (var item in QueueOfRequests)
                {
                    if(UpDown == "Up")
                    {
                        if (string.Compare(item.ID, maxFloor.ID) > 0)
                        {
                            maxFloor = item;
                        }
                    }
                    else
                    {
                        if (string.Compare(item.ID, maxFloor.ID) < 0)
                        {
                            maxFloor = item;
                        }
                    }
                    
                }
                return maxFloor;
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
