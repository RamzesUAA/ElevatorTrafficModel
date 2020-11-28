using System.Collections.Generic;
using ElevatorModelBL.Enums;
using ElevatorModelBL.Models;

namespace ElevatorModelBL.Additional_models
{
    /// <summary>
    /// Class that clarifies data which should be displayed in data table.
    /// </summary>
    public class DataForTable
    {
        public string Name { get; set; }
        public List<Person> PeopleInside { get; set; }
        public double CurrentWeigh { get; set; }
        public double MaxWeight { get; set; }
        public ElevatorType TypeOfElevator { get; set; }
        public string FloorDirection { get; set; }
    }
}
