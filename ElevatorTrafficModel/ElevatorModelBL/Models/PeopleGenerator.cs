using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ElevatorModelBL.Additional_models;
using ElevatorModelBL.Services;

namespace ElevatorModelBL.Models
{
    /// <summary>
    /// Class that describe generator of the people.
    /// </summary>
    public class PeopleGenerator
    {
        private readonly Random _rnd = new Random();
        private static List<PersonName> _names = new List<PersonName>();
        private readonly JsonFileService _jsonFileService = new JsonFileService();
        public PeopleGenerator()
        {
            DefaultNameDeserializer();
        }
        public PeopleGenerator(string path)
        {
            var isCreated = SelectedNameDeserializer(path);
            if (isCreated == false)
            {
                DefaultNameDeserializer();
            }
        }
        /// <summary>
        /// Deserializing selected file with names of people.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool SelectedNameDeserializer(string path)
        {
            try
            {
                if(new FileInfo(path).Length == 0)
                {
                    throw new Exception("The file cannot be empty.");
                }
                _names = _jsonFileService.Open(path);
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
        /// <summary>
        /// Deserializing default file with names of people.
        /// </summary>
        /// <returns></returns>
        private bool DefaultNameDeserializer()
        {
            try
            {
                var exePath = AppDomain.CurrentDomain.BaseDirectory;
                var path = Path.Combine(exePath, "Data files\\NamesJSON.txt");
                if(new FileInfo(path).Exists == false)
                {
                    throw new Exception("File NamesJSON.txt does not exists. You should add this file into the root of application.");
                }
                _names = _jsonFileService.Open(path);

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
        /// <summary>
        /// Method that randomly generate list of passengers.
        /// </summary>
        /// <param name="floors"></param>
        /// <returns></returns>
        public List<Person> GetPassengers(List<Floor> floors)
        {
            var people = new List<Person>();

            var count = _rnd.Next(10, 32);
            var countOfPersons = 0;
            for(var i=0; i < count; ++i)
            {
                countOfPersons++;
                var random = _rnd.Next(0, floors.Count);
                var intentionRandom = _rnd.Next(0, floors.Count);
                while(random == intentionRandom)
                {
                    intentionRandom = _rnd.Next(0, floors.Count);
                }
                var name = "";
                if (_names.Count == 0 || _names[0].Name == null)
                {
                    name = GetRandomText();
                }
                else
                {
                    name = _names[_rnd.Next(0, _names.Count-1)].Name;
                }

                var person = new Person()
                {
                    Sex = countOfPersons % 2 == 0 ? "Man" : "Woman",
                    Name = name,
                    CurrentFloor = floors[random],
                    FloorIntention = floors[intentionRandom],
                    Weight = (float)_rnd.NextDouble() * (115 - 35) + 35,
                };
                _names.Remove(_names.FirstOrDefault(p => p.Name == name));
                people.Add(person);
            }
            return people;
        }
        /// <summary>
        /// Overridden method that returns word "Person" which concatenated with randomly generated number.
        /// </summary>
        /// <returns></returns>
        private string GetRandomText()
        {
            return "Person" + _rnd.Next(1,999999999);
        }
    }
}
