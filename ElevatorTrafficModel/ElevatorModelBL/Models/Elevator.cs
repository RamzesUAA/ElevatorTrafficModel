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
        public Elevator(ElevatorType type)
        {
            ID = random.Next(1, 321);
            TypeOfElevator = type;
        }
        private int ID { get; set; }
        private ElevatorType TypeOfElevator {get;set;}
        private List<Person> PeopleInsideElevator { get; set; }
        private int MaxWeigh() => (int)TypeOfElevator;

    }
}
