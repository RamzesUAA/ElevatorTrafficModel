using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ElevatorModelBL.Models
{
    public class DefaultDialogService : IDialogService
    {
        private string filePath { get; set; }
        public string FilePath { get {  return filePath; } set { filePath = value; } }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if(saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }
    }
}