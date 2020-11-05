using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorModelBL.Models
{
    public class Query
    {
        public Query()
        {

        }
        public Floor NumberOfFloor { get; set; }
        public Dictionary<Elevator, List<Person>>  PeopleInQueue = new Dictionary<Elevator, List<Person>>();
        public int CountPeopleInQueue => PeopleInQueue.Values.Count;
    }
}
