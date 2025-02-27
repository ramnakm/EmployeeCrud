using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Models.Interfaces;
using Xunit.Sdk;

namespace EmployeeCrud.Models
{
    public class Employee : Person
    {
        private decimal _salary;
        private Department _department;
        private Address _address;

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative value.")]
        public decimal Salary
        {
            get => _salary;
            set => _salary = value;
        }

        [Required(ErrorMessage = "Department is required.")]
        public Department Department
        {
            get => _department;
            set => _department = value;
        }

        [Required(ErrorMessage = "Address is required.")]
        public Address Address
        {
            get => _address;
            set => _address = value;
        }
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
