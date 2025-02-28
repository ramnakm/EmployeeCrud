using EmployeeCrud.Models;
using EmployeeCrud.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeCrud.Tests
{
    /// <summary>
    /// Tests for the EmployeeFunction class.
    /// </summary>
    public class EmployeeFunctionTests
    {
        private readonly Mock<ILogger<EmployeeFunction>> _loggerMock;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly EmployeeFunction _employeeFunction;

        public EmployeeFunctionTests()
        {
            // Initialize mocks and the EmployeeFunction instance
            _loggerMock = new Mock<ILogger<EmployeeFunction>>();
            _employeeServiceMock = new Mock<IEmployeeService>();
            _employeeFunction = new EmployeeFunction(_loggerMock.Object, _employeeServiceMock.Object);
        }

        /// <summary>
        /// Tests that CreateEmployee returns a BadRequestObjectResult when validation fails.   
        /// </summary>
        [Fact]
        public async Task CreateEmployee_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange: Create an invalid Employee object with negative salary
            var employee = new Employee { Salary = -1 };
            var json = JsonConvert.SerializeObject(employee);
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Body).Returns(memoryStream);

            // Act: Call the CreateEmployee function
            var result = await _employeeFunction.CreateEmployee(httpRequest.Object);

            // Assert: Verify that the result is a BadRequestObjectResult with a validation error message
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Validation failed", badRequestResult.Value.ToString());
        }

        /// <summary>
        /// Tests that CreateEmployee returns an OkObjectResult with the created Employee object.
        /// </summary>
        [Fact]
        public async Task CreateEmployee_ReturnsOkResult()
        {
            // Arrange: Create a valid Employee object
            var employee = new Employee
            {
                Id = "1",
                Name = "John Doe",
                Position = "BSA",
                Salary = 50000,
                Department = new Department { DepartmentId = 1, DepartmentName = "HR" },
                Address = new Address { Street = "123 Main St", City = "Anytown", State = "CA", ZipCode = "12345" }
            };
            var json = JsonConvert.SerializeObject(employee);
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Body).Returns(memoryStream);
            _employeeServiceMock.Setup(x => x.Create(It.IsAny<Employee>())).ReturnsAsync(employee);

            // Act: Call the CreateEmployee function
            var result = await _employeeFunction.CreateEmployee(httpRequest.Object);

            // Assert: Verify that the result is an OkObjectResult with the created Employee object
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(employee.Id, returnValue.Id);
        }

        /// <summary>
        /// Tests that GetEmployeeById returns an OkObjectResult with the Employee object.
        /// </summary>
        [Fact]
        public async Task GetEmployeeById_ReturnsOkResult()
        {
            // Arrange: Create an Employee object with ID "1"
            var employee = new Employee { Id = "1", Salary = 50000 };
            _employeeServiceMock.Setup(x => x.GetById("1")).ReturnsAsync(employee);
            var httpRequest = new Mock<HttpRequest>();

            // Act: Call the GetEmployeeById function
            var result = await _employeeFunction.GetEmployeeById(httpRequest.Object, "1");

            // Assert: Verify that the result is an OkObjectResult with the Employee object
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(employee.Id, returnValue.Id);
        }

        /// <summary>
        /// Tests that GetEmployees returns an OkObjectResult with a list of Employee objects.
        /// </summary>
        [Fact]
        public async Task GetEmployees_ReturnsOkResult()
        {
            // Arrange: Create a list of Employee objects
            var employees = new List<Employee> { new Employee { Id = "1", Salary = 50000 } };
            _employeeServiceMock.Setup(x => x.GetAll()).ReturnsAsync(employees);
            var httpRequest = new Mock<HttpRequest>();

            // Act: Call the GetEmployees function
            var result = await _employeeFunction.GetEmployees(httpRequest.Object);

            // Assert: Verify that the result is an OkObjectResult with the list of Employee objects
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Employee>>(okResult.Value);
            Assert.Single(returnValue);
        }

        /// <summary>
        /// Tests that UpdateEmployee returns a BadRequestObjectResult when validation fails.
        /// </summary>
        [Fact]
        public async Task UpdateEmployee_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange: Create an invalid Employee object with negative salary
            var employee = new Employee { Salary = -1 };
            var json = JsonConvert.SerializeObject(employee);
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Body).Returns(memoryStream);

            // Act: Call the UpdateEmployee function
            var result = await _employeeFunction.UpdateEmployee(httpRequest.Object, "1");

            // Assert:  
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Validation failed", badRequestResult.Value.ToString());
        }

        /// <summary>
        /// Tests that UpdateEmployee returns an OkObjectResult with the updated Employee object.
        /// </summary>
        [Fact]
        public async Task UpdateEmployee_ReturnsOkResult()
        {
            // Arrange: Create a valid Employee object
            var employee = new Employee
            {
                Id = "1",
                Name = "John Doe",
                Position = "BSA",
                Salary = 50000,
                Department = new Department { DepartmentId = 1, DepartmentName = "HR" },
                Address = new Address { Street = "123 Main St", City = "Anytown", State = "CA", ZipCode = "12345" }
            };
            var json = JsonConvert.SerializeObject(employee);
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Body).Returns(memoryStream);
            _employeeServiceMock.Setup(x => x.Update(It.IsAny<Employee>())).ReturnsAsync(employee);

            // Act: Call the UpdateEmployee function
            var result = await _employeeFunction.UpdateEmployee(httpRequest.Object, "1");

            // Assert: Verify that the result is an OkObjectResult with the updated Employee object
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(employee.Id, returnValue.Id);
        }

        /// <summary>
        /// Tests that DeleteEmployee returns a NoContentResult.
        /// </summary>
        [Fact]
        public async Task DeleteEmployee_ReturnsNoContentResult()
        {
            // Arrange: Create an HttpRequest object and mock the Delete method
            var httpRequest = new Mock<HttpRequest>();
            _employeeServiceMock.Setup(x => x.Delete("1")).Returns(Task.CompletedTask);

            // Act: Call the DeleteEmployee function    
            var result = await _employeeFunction.DeleteEmployee(httpRequest.Object, "1");

            // Assert: Verify that the result is a NoContentResult  
            Assert.IsType<NoContentResult>(result);
        }
    }
}