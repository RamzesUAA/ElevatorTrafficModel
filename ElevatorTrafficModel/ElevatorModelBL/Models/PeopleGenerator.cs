using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Windows;

namespace ElevatorModelBL.Models
{
    public class PersonName
    {
        public string Name { get; set; }
    }
    public class PeopleGenerator
    {
        Random rnd = new Random();
        static List<PersonName> Names = new List<PersonName>();
        JsonFileService jsonFileService = new JsonFileService();

        public PeopleGenerator()
        {
            DefaultNameDeserializer();
        }
        public PeopleGenerator(string path)
        {
            var isCreated = ChoosedNameDeserializer(path);
            if (isCreated == false)
            {
                if(DefaultNameDeserializer()== false)
                {
                    return;
                }
            }
        }

        public bool ChoosedNameDeserializer(string path)
        {
            try
            {
                if(new FileInfo(path).Length == 0)
                {
                    throw new Exception("The file cannot be empty.");
                }
                Names = jsonFileService.Open(path);
            }
            catch (FormatException fex)
            {
                MessageBox.Show(fex.Message);

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }


        private bool DefaultNameDeserializer()
        {
            try
            {
                var exePath = AppDomain.CurrentDomain.BaseDirectory;
                var path = Path.Combine(exePath, "DataFiles\\NamesJSON.txt");
                if(new FileInfo(path).Exists == false)
                {
                    throw new Exception("File NamesJSON.txt doesn`t exists. You should add this file into the root of application.");
                }
                Names = jsonFileService.Open(path);

            }
            catch (FormatException fex)
            {
                //Invalid json format
                MessageBox.Show(fex.Message);
                return false;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
          
        }

        public List<Person> GetPassangers(List<Floor> floors)
        {
            List<Person> people = new List<Person>();

            int count = rnd.Next(10, 32);
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
                if (Names.Count == 0 || Names[0].Name == null)
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
