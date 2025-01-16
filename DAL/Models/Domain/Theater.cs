using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain
{
    public class Theater
    {
        public int TheaterId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public int AddressId { get; set; }

        // Navigation Property
        public Address Address { get; set; }

    }
}
