using EmployeeCrud.Models;
using EmployeeCrud.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace EmployeeCrud.Tests
{
    public class EmployeeTests
    {
        private bool ValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, null, null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        [Fact]
        public void Employee_Salary_ShouldBeNonNegative()
        {
            // Arrange
            var employee = new Employee { Salary = -1 };

            // Act
            var isValid = ValidateModel(employee, out var results);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, v => v.ErrorMessage == "Salary must be a non-negative value.");
        }

        [Fact]
        public void Employee_Department_ShouldBeRequired()
        {
            // Arrange
            var employee = new Employee { Department = null };

            // Act
            var isValid = ValidateModel(employee, out var results);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, v => v.ErrorMessage == "Department is required.");
        }

        [Fact]
        public void Employee_Address_ShouldBeRequired()
        {
            // Arrange
            var employee = new Employee { Address = null };

            // Act
            var isValid = ValidateModel(employee, out var results);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, v => v.ErrorMessage == "Address is required.");
        }

        [Fact]
        public void Employee_ValidModel_ShouldPassValidation()
        {
            // Arrange
            var employee = new Employee
            {
                Id = "1",
                Name = "John Doe",
                Position = "BSA",
                Salary = 50000,
                Department = new Department { DepartmentId = 1, DepartmentName = "HR" },
                Address = new Address { Street = "123 Main St", City = "Anytown", State = "CA", ZipCode = "12345" }
            };

            // Act
            var isValid = ValidateModel(employee, out var results);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }
    }

    public class PersonTests
    {
        private bool ValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, null, null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        [Fact]
        public void Person_Name_ShouldBeRequired()
        {
            // Arrange
            var person = new Person { Name = null };

            // Act
            var isValid = ValidateModel(person, out var results);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, v => v.ErrorMessage == "Name is required.");
        }

        [Fact]
        public void Person_Name_ShouldNotExceedMaxLength()
        {
            // Arrange
            var person = new Person { Name = new string('a', 101) };

            // Act
            var isValid = ValidateModel(person, out var results);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, v => v.ErrorMessage == "Name length can't be more than 100 characters.");
        }

        [Fact]
        public void Person_Position_ShouldBeRequired()
        {
            // Arrange
            var person = new Person { Position = null };

            // Act
            var isValid = ValidateModel(person, out var results);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, v => v.ErrorMessage == "Position is required.");
        }

        [Fact]
        public void Person_Position_ShouldNotExceedMaxLength()
        {
            // Arrange
            var person = new Person { Position = new string('a', 51) };

            // Act
            var isValid = ValidateModel(person, out var results);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, v => v.ErrorMessage == "Position length can't be more than 50 characters.");
        }

        [Fact]
        public void Person_ValidModel_ShouldPassValidation()
        {
            // Arrange
            var person = new Person
            {
                Name = "John Doe",
                Position = "Manager"
            };

            // Act
            var isValid = ValidateModel(person, out var results);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }
    }

    public class DepartmentTests
    {
        [Fact]
        public void Department_ShouldSetAndGetProperties()
        {
            // Arrange
            var department = new Department
            {
                DepartmentId = 1,
                DepartmentName = "HR"
            };

            // Act & Assert
            Assert.Equal(1, department.DepartmentId);
            Assert.Equal("HR", department.DepartmentName);
        }
    }

    public class AddressTests
    {
        [Fact]
        public void Address_ShouldSetAndGetProperties()
        {
            // Arrange
            var address = new Address
            {
                Street = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "12345"
            };

            // Act & Assert
            Assert.Equal("123 Main St", address.Street);
            Assert.Equal("Anytown", address.City);
            Assert.Equal("CA", address.State);
            Assert.Equal("12345", address.ZipCode);
        }
    }
}
