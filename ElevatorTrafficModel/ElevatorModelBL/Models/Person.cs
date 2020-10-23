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
        public Person(string Name, int FloorIntention, int CurrentFloor, float Weigh)
        {
            this.Name = Name;
            this.FloorIntention = FloorIntention;
            this.CurrentFloor = CurrentFloor;
            this.Weigh = Weigh;
        }
        private string Name { get; set; }
        private int FloorIntention { get; set; }
        private int CurrentFloor { get; set; }
        private float Weigh { get; set; }
    }
}
