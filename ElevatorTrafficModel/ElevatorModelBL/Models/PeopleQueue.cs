using System.Collections.Generic;

namespace ElevatorModelBL.Models
{
    /// <summary>
    /// Class that provide all entities for entity of the real life queue.
    /// </summary>
    public class PeopleQueue
    {
        /// <summary>
        /// Location of the queue
        /// </summary>
        public Floor NumberOfFloor { get; set; }
        /// <summary>
        /// Count of the people in the certain queue.
        /// </summary>
        public Dictionary<Elevator, List<Person>>  PeopleInQueue = new Dictionary<Elevator, List<Person>>();
    }
}
