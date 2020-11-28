using ElevatorModelBL.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorModelBL.Models
{
    /// <summary>
    /// Class Elevator describe attributes and method as elevator entity has.
    /// </summary>
    public class Elevator
    {
        /// <summary>
        /// A unique identifier for access to the desired elevator.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Attribute that returns weight of certain elevator.
        /// </summary>
        public int MaxWeight => (int)TypeOfElevator;
        public ElevatorType TypeOfElevator { get; set; }
        /// <summary>
        /// List of person that save all information about who have visited current elevator.
        /// </summary>
        public List<Person> AllPeoplesUsedElevator { get; set; } = new List<Person>();

        /// <summary>
        /// A list of people who contain information about the people inside the elevator.
        /// </summary>
        [JsonIgnore]
        public List<Person> PeopleInsideElevator { get; set; } = new List<Person>();
        /// <summary>
        /// Queue of requests that returns query from outside or add new queries.
        /// </summary>
        [JsonIgnore]
        public List<Floor> QueueOfRequests { get; set; } = new List<Floor>();

        /// <summary>
        /// Queue of requests from inside that contain people`s intention directions.
        /// </summary>
        [JsonIgnore]
        public List<Floor> QueueFromInside { get; set; } = new List<Floor>();

        /// <summary>
        /// Direction of moving elevator in certain time.
        /// </summary>
        [JsonIgnore]
        public string UpDown { get; set; }
        /// <summary>
        /// Current speed of elevator. In the way of setting negative number elevator moves into down, if stetted number is positive elevator will move into up side.
        /// </summary>
        [JsonIgnore]
        public int ElevatorSpeed { get; set; }
        /// <summary>
        /// Returns current weight
        /// </summary>
        [JsonIgnore]
        public double CurrentWeight
        {
            get
            {
                double currentWeigh = 0;
                foreach (var item in PeopleInsideElevator)
                {
                    currentWeigh += item.Weight;
                }
                return currentWeigh;
            }
        }
        /// <summary>
        /// Method that returns turned floor for the elevator which are moving to the up side.
        /// </summary>
        /// <returns></returns>
        public Floor MaxTurnedPoint()
        {
            var maxQueue = QueueOfRequests.FirstOrDefault();
            var maxInside = QueueFromInside.FirstOrDefault();       

            switch (maxQueue)
            {
                case null when maxInside != null:
                {
                    var maxFromInsideTemp = int.Parse(QueueFromInside.First().Id[5].ToString());

                    foreach (var item in QueueFromInside)
                    {
                        var tempItem = int.Parse(item.Id[5].ToString());
                        if (maxFromInsideTemp >= tempItem) continue;
                        maxFromInsideTemp = tempItem;
                        maxInside = item;
                    }
                    return maxInside;
                }
                case null when QueueFromInside.Count == 0:
                    return null;
            }

            var maxFromQueue= int.Parse(QueueOfRequests.First().Id[5].ToString());
            foreach (var item in QueueOfRequests)
            {
                var tempItem = int.Parse(item.Id[5].ToString());

                if (maxFromQueue >= tempItem) continue;
                maxFromQueue = tempItem;
                maxQueue = item;
            }

            if (QueueFromInside.Count == 0)
            {
                return maxQueue;
            }

            var maxFromInside = int.Parse(QueueFromInside.First().Id[5].ToString());
            foreach (var item in QueueFromInside)
            {
                var tempItem = int.Parse(item.Id[5].ToString());

                if (maxFromInside >= tempItem) continue;
                maxFromInside = tempItem;
                maxInside = item;
            }

            if (QueueOfRequests.Count == 0)
            {
                return maxInside;
            }

            if (maxInside != null && maxQueue != null && maxQueue.Id[5] > maxInside.Id[5])
            {
                return maxQueue;
            }

            return maxInside;
        }
        /// <summary>
        /// Method that returns turned floor for the elevator which are moving to the down side.
        /// </summary>
        /// <returns></returns>
        public Floor MinTurnedPoint()
        {
            var minQueue = QueueOfRequests.FirstOrDefault();
            var minInside = QueueFromInside.FirstOrDefault();

            switch (minQueue)
            {
                case null when minInside != null:
                {
                    var minFromInsideTemp = int.Parse(QueueFromInside.First().Id[5].ToString());

                    foreach (var item in QueueFromInside)
                    {
                        var tempItem = int.Parse(item.Id[5].ToString());
                        if (minFromInsideTemp <= tempItem) continue;
                        minFromInsideTemp = tempItem;
                        minInside = item;
                    }
                    return minInside;
                }
                case null when QueueFromInside.Count == 0:
                    return null;
            }

            var minFromQueue = int.Parse(QueueOfRequests.First().Id[5].ToString());
            foreach (var item in QueueOfRequests)
            {
                var tempItem = int.Parse(item.Id[5].ToString());

                if (minFromQueue <= tempItem) continue;
                minFromQueue = tempItem;
                minQueue = item;
            }
            if (QueueFromInside.Count == 0)
            {
                return minQueue;
            }

            var minFromInside = int.Parse(QueueFromInside.First().Id[5].ToString());
            foreach (var item in QueueFromInside)
            {
                var tempItem = int.Parse(item.Id[5].ToString());

                if (minFromInside <= tempItem) continue;
                minFromInside = tempItem;
                minInside = item;
            }

            if (QueueOfRequests.Count == 0)
            {
                return minInside;
            }

            return minInside != null && minQueue != null && (int)minQueue.Id[5] > (int)minInside.Id[5] ? minInside : minQueue;
        }
        /// <summary>
        /// Adding new person into the elevator.
        /// </summary>
        /// <param name="person"></param>
        public void Filling(Person person)
        {
            this.PeopleInsideElevator.Add(person);
        }
        /// <summary>
        /// Removing person from the elevator.
        /// </summary>
        /// <param name="person"></param>
        public void ExitFromElevator(Person person)
        {

            PeopleInsideElevator.Remove(person);

        }
        /// <summary>
        /// Overridden method ToString() that return id of the elevator. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Elevator id: {this.Id}";
        }
    }
}
