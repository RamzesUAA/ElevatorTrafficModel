using ElevatorModelBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorModelBL.Controllers
{
    public class ElevatorController
    {
        QueryController  Queries;
        public ElevatorController()
        {
        }
        List<QueueOfRequests> elevatorController = new List<QueueOfRequests>();
        public bool ElevatorFilling(Elevator elevator, Person person)
        {
            if (elevator.CurrentWeigh + person.Weigh < elevator.MaxWeigh && string.Compare(elevator.CurrentDestination, person.FloorIntention.ID)>0)
            {
                elevator.PeopleInsideElevator.Add(person);
            }

            return true;
        }

        // TODO: реалізувати створення черги, подивитися відео про логіку викликів ліфтів
        public bool MakeElevatorRequest(Floor floor)
        {


            return true;
        }
    }
}
