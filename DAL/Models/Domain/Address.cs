using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain
{
    public class Address
    {
        public int AddressId { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public int PostalCodeId { get; set; }


        // Navigation Property
        public PostalCode PostalCode { get; set; }
    }
}
