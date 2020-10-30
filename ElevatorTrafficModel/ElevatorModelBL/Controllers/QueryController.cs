using ElevatorModelBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorModelBL.Controllers
{
    public class QueryController
    {
        List<Query> queries = new List<Query>();


        public void Add(Person person)
        {
            Query minQuery = queries.Where(p => p.NumberOfFloor == person.CurrentFloor).FirstOrDefault();
          
            if(minQuery == null)
            {

            }
            else
            {
                var min = minQuery.PeopleInQueue.Values.Count;
                foreach (var item in queries.Where(p => p.NumberOfFloor == person.CurrentFloor))
                {
                    if (minQuery.PeopleInQueue.Count > item.CountPeopleInQueue)
                    {
                        minQuery = item;
                    }
                }

               
            }
            
            
        }

    }
}
