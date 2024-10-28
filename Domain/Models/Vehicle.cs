using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalagring_CarPark_EFCore.Domain.Models
{
    class Vehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Notes { get; set; }

        public Vehicle()
        {

        }

        public Vehicle(string registrationNumber, string notes)
        {
            RegistrationNumber = registrationNumber;
            Notes = notes;
        }

        public Vehicle(int id, string registrationNumber, string notes)
           : this(registrationNumber, notes)
        {
            Id = id;
        }
    }
}