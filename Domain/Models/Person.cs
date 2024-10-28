using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalagring_CarPark_EFCore.Domain.Models
{
    class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public Person()
        {

        }
        public Person(string firstName, string lastName, string socialSecurityNumber, string phoneNumber, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            SocialSecurityNumber = socialSecurityNumber;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public Person(int id, string firstName, string lastName, string socialSecurityNumber, string phoneNumber, string email)
            : this(firstName, lastName, socialSecurityNumber, phoneNumber, email)
        {
            Id = id;
        }
    }
}