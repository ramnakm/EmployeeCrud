using EmployeeCrud.Models;
using EmployeeCrud.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace EmployeeCrud.Tests
{
    public class EmployeeTests
    {
        /// <summary>
        /// Validates the given model and returns the validation results.
        /// </summary>
        /// <param name="model">The model to validate.</param>
        /// <param name="results">The validation results.</param>
        /// <returns>True if the model is valid, otherwise false.</returns>
        private bool ValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, null, null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        /// <summary>
        /// Tests that the Employee's salary should be a non-negative value.
        /// </summary>
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

        /// <summary>
        /// Tests that the Employee's department should be required.
        /// </summary>
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

        /// <summary>
        /// Tests that the Employee's address should be required.
        /// </summary>
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

        /// <summary>
        /// Tests that the Employee model is valid.
        /// </summary>
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
        /// <summary>
        /// Validates the given model and returns the validation results.
        /// </summary>
        /// <param name="model">The model to validate.</param>
        /// <param name="results">The validation results.</param>
        /// <returns>True if the model is valid, otherwise false.</returns>
        private bool ValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, null, null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        /// <summary>
        /// Tests that the Person's name should be required.
        /// </summary>
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

        /// <summary>
        /// Tests that the Person's name should not exceed the maximum length.
        /// </summary>
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

        /// <summary>
        /// Tests that the Person's position should be required.
        /// </summary>
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

        /// <summary>
        /// Tests that the Person's position should not exceed the maximum length.
        /// </summary>
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

        /// <summary>
        /// Tests that the Person model is valid.
        /// </summary>
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

    /// <summary>
    /// Tests for the Department class.
    /// </summary>
    public class DepartmentTests
    {
        /// <summary>
        /// Tests that the Department class should set and get properties correctly.
        /// </summary>
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
        /// <summary>
        /// Tests that the Address class should set and get properties correctly.
        /// </summary>
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
