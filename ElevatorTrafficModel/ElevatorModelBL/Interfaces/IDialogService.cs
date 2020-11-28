namespace ElevatorModelBL.Interfaces
{
    /// <summary>
    /// Interface for managing by dialog service. In the way of realizing this interface
    /// base class should declare OpenFileDialog() and SaveFileDialog() methods.
    /// </summary>
    public interface IDialogService
    {
        string FilePath { get; set; }
        bool OpenFileDialog();
        bool SaveFileDialog();
    }
}