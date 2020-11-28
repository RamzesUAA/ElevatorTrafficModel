namespace ElevatorModelBL.Enums
{
    /// <summary>
    /// Enum with three types of elevator. In the way of casting this enum to Int32 we can get a max weight.
    /// </summary>
    public enum ElevatorType
    {
        Hydraulic = 1200,
        MachineRoom = 380,
        Traction = 670,
    }
}