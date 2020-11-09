using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorModelBL.Models
{
    public class Generator
    {
        Random rnd = new Random();
        public List<Person> GetPassangers(List<Floor> floors)
        {
            List<Person> people = new List<Person>();
            int count = rnd.Next(5, 25);
            
            for(int i=0; i < count; ++i)
            {
                int random = rnd.Next(0, floors.Count);
                int intentionRandom = rnd.Next(0, floors.Count - 1);
                while(random == intentionRandom)
                {
                    intentionRandom = rnd.Next(0, floors.Count - 1);
                }
                var person = new Person()
                {
                    Name = GetRandomText(),
                    CurrentFloor = floors[random],
                    FloorIntention = floors[intentionRandom],
                    Weigh = (float)rnd.NextDouble() * (115 - 35) + 35,
                };
                people.Add(person);
            }
            return people;
        }

        private string GetRandomText()
        {
            return "Person" + GetHashCode();
        }
    }
}
