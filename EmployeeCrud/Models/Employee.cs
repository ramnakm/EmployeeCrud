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
    // Employee class inheriting from Person
    public class Employee : Person
    {
        private decimal _salary;
        private Department _department;
        private Address _address;

        // Salary property with validation to ensure non-negative value
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative value.")]
        public decimal Salary
        {
            get => _salary;
            set => _salary = value;
        }

        // Department property with validation to ensure it is required
        [Required(ErrorMessage = "Department is required.")]
        public Department Department
        {
            get => _department;
            set => _department = value;
        }

        // Address property with validation to ensure it is required
        [Required(ErrorMessage = "Address is required.")]
        public Address Address
        {
            get => _address;
            set => _address = value;
        }
    }

    // Department class representing an employee's department
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    // Address class representing an employee's address
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
