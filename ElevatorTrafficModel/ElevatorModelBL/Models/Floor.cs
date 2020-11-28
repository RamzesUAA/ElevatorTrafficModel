namespace ElevatorModelBL.Models
{
    /// <summary>
    /// Class that describe entity of floor.
    /// </summary>
    public class Floor
    {
        /// <summary>
        /// A unique identifier for access to the desired floor.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Overridden method ToString() that return id of the floor. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id;
        }
    }
}
