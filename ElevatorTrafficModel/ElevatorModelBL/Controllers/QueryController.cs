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
        public QueryController(List<Elevator> elevators, List<Floor> floors)
        {
            foreach(var floor in floors)
            {
                Query FloorQuery = new Query();
                FloorQuery.NumberOfFloor = floor;
                foreach (var elevator in elevators)
                {
                    List<Person> list = new List<Person>();
                    FloorQuery.PeopleInQueue.Add(elevator, list);
                    
                }
                Queries.Add(FloorQuery);
            }    
        }
        public KeyValuePair<Elevator,List<Person>> Add(Person person, List<Elevator> elevators)
        {
            Query FloorQuery = Queries.Where(p => p.NumberOfFloor == person.CurrentFloor).FirstOrDefault();

            //if(FloorQuery == null)
            //{
            //    FloorQuery = new Query();
            //    List<Person> list = new List<Person>();
            //    list.Add(person);
            //    FloorQuery.NumberOfFloor = person.CurrentFloor;
            //    FloorQuery.PeopleInQueue.Add(elevators[0], list);
            //    for(int i = 1; i < elevators.Count ;++i)
            //    {
            //        FloorQuery.PeopleInQueue.Add(elevators[i], new List<Person>());
            //    }
            //    Queries.Add(FloorQuery);
            //}

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

            //return FloorQuery.PeopleInQueue.First();
        }


        public Query GetQuery(Floor floor)
        {
            return Queries.Where(p => p.NumberOfFloor == floor).FirstOrDefault();
        }


        public KeyValuePair<Elevator, List<Person>> Remove(Person person, List<Elevator> elevators)
        {
            return new KeyValuePair<Elevator, List<Person>>();
        }
    }
}