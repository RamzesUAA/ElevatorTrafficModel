using ElevatorModelBL.Models;
using System.Collections.Generic;

namespace ElevatorModelBL.Interfaces
{
    public interface IFileService
    {
        List<PersonName> Open(string filename);
        void Save(string filename, List<Elevator> phoneList);
    }
}