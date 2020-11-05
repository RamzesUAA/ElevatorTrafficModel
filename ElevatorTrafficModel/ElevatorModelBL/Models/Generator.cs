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
        public void GetPassangers(List<Person> people, List<Floor> floors)
        {
            int count = rnd.Next(35, 100);
            for(int i=0; i < count; ++i)
            {
                int random = rnd.Next(0, floors.Count-1);
                var person = new Person()
                {
                    Name = GetRandomText(),
                    CurrentFloor = floors[random],
                    Weigh = (float)rnd.NextDouble() * (115 - 35) + 35,
                };
                people.Add(person);
            }
        }

        private string GetRandomText()
        {
            return Guid.NewGuid().ToString().Substring(0, 5);
        }
    }
}
