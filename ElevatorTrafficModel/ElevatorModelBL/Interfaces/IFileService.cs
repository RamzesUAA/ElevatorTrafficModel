using ElevatorModelBL.Models;
using System.Collections.Generic;
using ElevatorModelBL.Additional_models;

namespace ElevatorModelBL.Interfaces
{
    /// <summary>
    /// Interface for managing by file service. That interface are used for serialization and
    /// deserialization of objects. In the way of realizing this interface
    /// base class should declare Open() and Save() methods.
    /// </summary>
    public interface IFileService
    {
        List<PersonName> Open(string filename);
        void Save(string filename, List<Elevator> phoneList);
    }
}