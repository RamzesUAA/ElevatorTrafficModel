using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorModelBL.Models
{
    public class Floor
    {
        private string id;
        public string ID { get { return id; } set { id = value; } }

        public override string ToString()
        {
            return ID;
        }
    }
}
