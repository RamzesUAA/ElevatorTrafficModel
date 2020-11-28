using ElevatorModelBL.Interfaces;
using Microsoft.Win32;

namespace ElevatorModelBL.Services
{
    /// <summary>
    /// Class which realize IDialogService. This class provides needed methods for managing by File Dialog.
    /// </summary>
    public class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }
        public bool OpenFileDialog()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return false;
            FilePath = openFileDialog.FileName;
            return true;
        }

        public bool SaveFileDialog()
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() != true) return false;
            FilePath = saveFileDialog.FileName;
            return true;
        }
    }
}