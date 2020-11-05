using ElevatorModelBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorModelBL.Controllers
{
    public class QueryController
    {
        public List<Query> Queries = new List<Query>();

        public KeyValuePair<Elevator,List<Person>> Add(Person person, List<Elevator> elevators)
        {
            Query FloorQuery = Queries.Where(p => p.NumberOfFloor == person.CurrentFloor).FirstOrDefault();
          
            if(FloorQuery == null)
            {
                FloorQuery = new Query();
                List<Person> list = new List<Person>();
                list.Add(person);
                FloorQuery.NumberOfFloor = person.CurrentFloor;
                FloorQuery.PeopleInQueue.Add(elevators[0], list);
                for(int i = 1; i < elevators.Count ;++i)
                {
                    FloorQuery.PeopleInQueue.Add(elevators[i], new List<Person>());
                }
                Queries.Add(FloorQuery);
            }
            else
            {
                var minFulledElevator = FloorQuery.PeopleInQueue.First();
                foreach (var item in FloorQuery.PeopleInQueue)
                {
                    if (minFulledElevator.Value.Count > item.Value.Count)
                    {
                        minFulledElevator = item;
                    }
                }
                minFulledElevator.Value.Add(person);
                return minFulledElevator;
            }
            return FloorQuery.PeopleInQueue.First();
        }
    }
}