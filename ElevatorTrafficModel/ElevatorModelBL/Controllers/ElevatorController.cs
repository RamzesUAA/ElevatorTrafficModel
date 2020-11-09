using ElevatorModelBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ElevatorModelBL.Controllers
{
    public class ElevatorController
    {
        QueryController  Queries;
        public ElevatorController()
        {
        }
        public void elevatorController(Query query, Elevator elevator)
        {
            var queryToCurrentElevator = query.PeopleInQueue[elevator];
            foreach(var item in queryToCurrentElevator)
            {
                if(ElevatorFilling(elevator, item))
                {
                    queryToCurrentElevator.Remove(item);
                }
            }
        }

        public Dictionary<Elevator, List<Floor>> ElevatorQuery = new Dictionary<Elevator, List<Floor>>();
        public bool ElevatorFilling(Elevator elevator, Person person)
        {
            if (elevator.CurrentWeigh + person.Weigh < elevator.MaxWeigh && string.Compare(elevator.CurrentDestination.ID, person.FloorIntention.ID)>0)
            {
                elevator.PeopleInsideElevator.Add(person);
                return true;
            }

            return false;
        }


        // TODO: реалізувати створення черги, подивитися відео про логіку викликів ліфтів
        public bool MakeElevatorRequest(Person person)
        {
            var elevator = person.FloorIntention;
            return true;
        }
    }
}
