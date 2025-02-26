using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Models.Interfaces;

namespace EmployeeCrud.Models
{
    public class Employee : Entity
    {
        public  string Name { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public Department Department { get; set; }
        public Address Address { get; set; }
    }

    // Department class
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    // Address class
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
