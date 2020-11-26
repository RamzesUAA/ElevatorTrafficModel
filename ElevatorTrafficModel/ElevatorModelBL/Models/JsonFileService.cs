using ElevatorModelBL.Interfaces;
using Newtonsoft.Json;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElevatorModelBL.Models
{
    public class JsonFileService : IFileService
    {
        public List<PersonName> Open(string filename)
        {
            List<PersonName> names = new List<PersonName>();
            string jsonFile;
            using (var fs = new StreamReader(filename))
            {
                jsonFile = fs.ReadToEnd();
            };

            JsonValue.Parse(jsonFile);

            names = JsonConvert.DeserializeObject<List<PersonName>>(jsonFile);
            return names;
        }

        public void Save(string filename, List<Elevator> elevators)
        {
            string json = JsonConvert.SerializeObject(elevators, Formatting.Indented);
            using (StreamWriter streamWriter = new StreamWriter(filename, false))
            {
                streamWriter.WriteLine(json);
            }

        }
    }
}