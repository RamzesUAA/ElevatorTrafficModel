using ElevatorModelBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
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
            int current = int.Parse(person.CurrentFloor.ID[5].ToString());
            int intesionFloor = int.Parse(person.FloorIntention.ID[5].ToString());
            if ((elevator.CurrentWeigh + person.Weigh) < elevator.MaxWeigh && intesionFloor > current  && elevator.UpDown == "Up")
            {
                elevator.PeopleInsideElevator.Add(person);
                return true;
            }else if((elevator.CurrentWeigh + person.Weigh) < elevator.MaxWeigh && intesionFloor < current && elevator.UpDown == "Down")
            {
                elevator.PeopleInsideElevator.Add(person);
                return true;
            }

            return false;
        }

        public void ExitFromElevator(Floor floor, Person person, Elevator elevator)
        {
            if(person.FloorIntention.ID == floor.ID)
            {
                elevator.PeopleInsideElevator.Remove(person);
            }
        }


        // TODO: реалізувати створення черги, подивитися відео про логіку викликів ліфтів
        public bool MakeElevatorRequest(Person person)
        {
            var elevator = person.FloorIntention;
            return true;
        }
    }
}
