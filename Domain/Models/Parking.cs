using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalagring_CarPark_EFCore.Domain.Models
{
    class Parking
    {
        public int Id { get; set; }

        public Person Person { get; set; }

        public Vehicle Vehicle { get; set; }

        public DateTime ArrivedAt { get; set; }

        public DateTime? DepartedAt { get; set; }
        public Parking()
        {
            
        }
       
        public Parking(Person person, Vehicle vehicle)
        {
            Person = person;
            Vehicle = vehicle;
            ArrivedAt = DateTime.Now;
        }
        public Parking(Person person, Vehicle vehicle, DateTime arrivedAt, DateTime? departedAt)
            : this(person, vehicle)
        {
            ArrivedAt = arrivedAt;
            DepartedAt = departedAt;
        }
       
        //internal void Departed()
        //{
        //    throw new NotImplementedException();
        //}
        //public Parking(int id, int personId, int vehicleId, DateTime arrivedAt)
        //{
        //    Id = id;
        //    PersonId = personId;
        //    VehicleId = vehicleId;
        //    ArrivedAt = arrivedAt;
        //}
    }
}