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
        List<Floor> QueueOfRequests = new List<Floor>();
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
        public string CurrentDestination
        {
            get
            {
                return PeopleInsideElevator.Max(p => p.FloorIntention.ID);
            }
        }

    }
}
