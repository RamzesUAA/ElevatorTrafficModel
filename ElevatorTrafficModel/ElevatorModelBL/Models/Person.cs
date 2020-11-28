namespace ElevatorModelBL.Models
{
    /// <summary>
    /// Class Person describe attributes and method as person entity has.
    /// </summary>
    public class Person
    {
        public string Name { get; set; }
        public string Sex { get; set; }
        public Floor FloorIntention { get; set; }
        public Floor CurrentFloor { get; set; }
        public float Weight { get; set; }
        /// <summary>
        /// Overridden method that returns short information about passenger.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Name: {this.Name}, weigh: {this.Weight}";
        }
    }
}
