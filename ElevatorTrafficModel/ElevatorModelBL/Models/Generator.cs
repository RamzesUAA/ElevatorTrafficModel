using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorModelBL.Models
{
    public class PersonName
    {
        public string Name { get; set; }
    }
    public class Generator
    {
        public Generator()
        {
            NameDeserializer();
        }

        Random rnd = new Random();
        List<PersonName> Names = new List<PersonName>();
        private void NameDeserializer()
        {
            string jsonFile;
            using (var fs = new StreamReader("C:\\Users\\Roman\\source\\repos\\ElevatorTrafficModel\\ElevatorTrafficModel\\NamesJSON.txt"))
            {
                jsonFile = fs.ReadToEnd();
            };
            Names = JsonConvert.DeserializeObject<List<PersonName>>(jsonFile);
        }

        public List<Person> GetPassangers(List<Floor> floors)
        {
            List<Person> people = new List<Person>();

            int count = rnd.Next(15, 35);
            int countOfPersons = 0;
            for(int i=0; i < count; ++i)
            {
                countOfPersons++;
                int random = rnd.Next(0, floors.Count);
                int intentionRandom = rnd.Next(0, floors.Count - 1);
                while(random == intentionRandom)
                {
                    intentionRandom = rnd.Next(0, floors.Count - 1);
                }
                string name = "";
                if (Names.Count == 1)
                {
                    name = GetRandomText();
                }
                else
                {
                    name = Names[rnd.Next(0, Names.Count-1)].Name;
                }
                

                var person = new Person()
                {
                    Sex = countOfPersons % 2 == 0 ? "Man" : "Woman",
                    Name = name,
                    CurrentFloor = floors[random],
                    FloorIntention = floors[intentionRandom],
                    Weigh = (float)rnd.NextDouble() * (115 - 35) + 35,
                };
                Names.Remove(Names.Where(p=>p.Name == name).FirstOrDefault());
                people.Add(person);
            }
            return people;
        }

        private string GetRandomText()
        {
            return "Person" + rnd.Next(1,9999999);
        }
    }
}
