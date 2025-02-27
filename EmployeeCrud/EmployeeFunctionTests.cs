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
    public class EmployeeFunctionTests
    {
        private readonly Mock<ILogger<EmployeeFunction>> _loggerMock;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly EmployeeFunction _employeeFunction;

        public EmployeeFunctionTests()
        {
            _loggerMock = new Mock<ILogger<EmployeeFunction>>();
            _employeeServiceMock = new Mock<IEmployeeService>();
            _employeeFunction = new EmployeeFunction(_loggerMock.Object, _employeeServiceMock.Object);
        }

        [Fact]
        public async Task CreateEmployee_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var employee = new Employee { Salary = -1 };
            var json = JsonConvert.SerializeObject(employee);
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Body).Returns(memoryStream);

            // Act
            var result = await _employeeFunction.CreateEmployee(httpRequest.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Validation failed", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task CreateEmployee_ReturnsOkResult()
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
            var json = JsonConvert.SerializeObject(employee);
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Body).Returns(memoryStream);
            _employeeServiceMock.Setup(x => x.Create(It.IsAny<Employee>())).ReturnsAsync(employee);

            // Act
            var result = await _employeeFunction.CreateEmployee(httpRequest.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(employee.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetEmployeeById_ReturnsOkResult()
        {
            // Arrange
            var employee = new Employee { Id = "1", Salary = 50000 };
            _employeeServiceMock.Setup(x => x.GetById("1")).ReturnsAsync(employee);
            var httpRequest = new Mock<HttpRequest>();

            // Act
            var result = await _employeeFunction.GetEmployeeById(httpRequest.Object, "1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(employee.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetEmployees_ReturnsOkResult()
        {
            // Arrange
            var employees = new List<Employee> { new Employee { Id = "1", Salary = 50000 } };
            _employeeServiceMock.Setup(x => x.GetAll()).ReturnsAsync(employees);
            var httpRequest = new Mock<HttpRequest>();

            // Act
            var result = await _employeeFunction.GetEmployees(httpRequest.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Employee>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var employee = new Employee { Salary = -1 };
            var json = JsonConvert.SerializeObject(employee);
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Body).Returns(memoryStream);

            // Act
            var result = await _employeeFunction.UpdateEmployee(httpRequest.Object, "1");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Validation failed", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsOkResult()
        {
            // Arrange
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
            var json = JsonConvert.SerializeObject(employee);
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Body).Returns(memoryStream);
            _employeeServiceMock.Setup(x => x.Update(It.IsAny<Employee>())).ReturnsAsync(employee);

            // Act
            var result = await _employeeFunction.UpdateEmployee(httpRequest.Object, "1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(employee.Id, returnValue.Id);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsNoContentResult()
        {
            // Arrange
            var httpRequest = new Mock<HttpRequest>();
            _employeeServiceMock.Setup(x => x.Delete("1")).Returns(Task.CompletedTask);

            // Act
            var result = await _employeeFunction.DeleteEmployee(httpRequest.Object, "1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
