using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorModelBL.Models
{
    public class Person
    {
        public Person(string Name, Floor FloorIntention, Floor CurrentFloor, float Weigh)
        {
            this.Name = Name;
            this.FloorIntention = FloorIntention;
            this.CurrentFloor = CurrentFloor;
            this.Weigh = Weigh;
        }
        public string Name { get; set; }
        public Floor FloorIntention { get; set; }
        public Floor CurrentFloor { get; set; }
        public float Weigh { get; set; }
    }
}
