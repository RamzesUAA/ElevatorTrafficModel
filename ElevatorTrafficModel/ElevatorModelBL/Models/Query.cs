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
        private int NumberOfFloor { get; set; }
        private List<Person> PeopleInQueue { get; set; }
        private int CountPeopleInQueue () => PeopleInQueue.Count;
    }
}
