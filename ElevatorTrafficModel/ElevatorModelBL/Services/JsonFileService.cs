using System.Collections.Generic;
using System.IO;
using System.Json;
using ElevatorModelBL.Additional_models;
using ElevatorModelBL.Interfaces;
using ElevatorModelBL.Models;
using Newtonsoft.Json;

namespace ElevatorModelBL.Services
{
    /// <summary>
    /// Class which realize IFileService. This class provides needed methods for serializing and deserializing object.
    /// </summary>
    public class JsonFileService : IFileService
    {
        public List<PersonName> Open(string filename)
        {
            string jsonFile;
            using (var fs = new StreamReader(filename))
            {
                jsonFile = fs.ReadToEnd();
            }

            JsonValue.Parse(jsonFile);

            var names = JsonConvert.DeserializeObject<List<PersonName>>(jsonFile);
            return names;
        }

        public void Save(string filename, List<Elevator> elevators)
        {
            var json = JsonConvert.SerializeObject(elevators, Formatting.Indented);
            using (var streamWriter = new StreamWriter(filename, false))
            {
                streamWriter.WriteLine(json);
            }

        }
    }
}