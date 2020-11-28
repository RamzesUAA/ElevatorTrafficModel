using ElevatorModelBL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorModelBL.Controllers
{
    /// <summary>
    /// Class that provide all entities for managing real life queue to the elevator.
    /// </summary>
    public class QueueController
    {
        /// <summary>
        /// Attribute that contain all information about queues to certain elevator.
        /// </summary>
        public List<PeopleQueue> Queues { get; set; } = new List<PeopleQueue>();
        public QueueController(IReadOnlyCollection<Elevator> elevators, IEnumerable<Floor> floors)
        {
            foreach(var floor in floors)
            {
                var floorQuery = new PeopleQueue {NumberOfFloor = floor};
                foreach (var elevator in elevators)
                {
                    var list = new List<Person>();
                    floorQuery.PeopleInQueue.Add(elevator, list);
                }
                Queues.Add(floorQuery);
            }    
        }
        /// <summary>
        /// Function that adds people to the queue.
        /// </summary>
        /// <param name="people"></param>
        public void Add(List<Person> people)
        {
            var peopleToGoByLadder = new List<Person>();
            foreach(var person in people)
            {
                var floorQuery = Queues.FirstOrDefault(p => p.NumberOfFloor == person.CurrentFloor);

                if (floorQuery == null) continue;
                var minFulledElevator = floorQuery.PeopleInQueue.First();
                foreach (var item in floorQuery.PeopleInQueue.Where(item => minFulledElevator.Value.Count > item.Value.Count))
                {
                    minFulledElevator = item;
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
        /// <summary>
        /// Function that returns queue.
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public PeopleQueue GetQuery(Floor floor)
        {
            return Queues.FirstOrDefault(p => p.NumberOfFloor == floor);
        }
    }
}