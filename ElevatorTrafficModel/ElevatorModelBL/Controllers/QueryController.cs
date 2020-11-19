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
        public void Add(List<Person> people, List<Elevator> elevators)
        {
            List<Person> peopleToGoByLadder = new List<Person>();
            foreach(var person in people)
            {
                Query FloorQuery = Queries.Where(p => p.NumberOfFloor == person.CurrentFloor).FirstOrDefault();

                var minFulledElevator = FloorQuery.PeopleInQueue.First();
                foreach (var item in FloorQuery.PeopleInQueue)
                {
                    if (minFulledElevator.Value.Count > item.Value.Count)
                    {
                        minFulledElevator = item;
                    }
                }
                if(minFulledElevator.Value.Count< 4)
                {
                    minFulledElevator.Value.Add(person);
                }
                else
                {
                    peopleToGoByLadder.Add(person);
                }
            }
            foreach(var person in peopleToGoByLadder)
            {
                people.Remove(person);
            }


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