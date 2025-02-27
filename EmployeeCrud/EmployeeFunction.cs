using System.ComponentModel.DataAnnotations;
using EmployeeCrud.Models;
using EmployeeCrud.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EmployeeCrud
{
    public class EmployeeFunction
    {
        private readonly ILogger<EmployeeFunction> _logger;

        private readonly IEmployeeService _employeeService;

        public EmployeeFunction(ILogger<EmployeeFunction> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [Function("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employees")] HttpRequest req)
        {
            using var reader = new StreamReader(req.Body);
            var empJson = await reader.ReadToEndAsync();
            try
            {
                var employee = JsonConvert.DeserializeObject<Employee>(empJson);
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(employee);

                if (!Validator.TryValidateObject(employee, validationContext, validationResults, true))
                {
                    var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                    return new BadRequestObjectResult($"Validation failed: {errors}");
                }

                var createdEmployee = await _employeeService.Create(employee);
                _logger.LogInformation($"C# HTTP trigger function processed a request to create Employee: {createdEmployee.Id}");
                return new OkObjectResult(createdEmployee);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to create employee: {empJson}";
                _logger.LogError(ex, errorMessage);
                return new BadRequestObjectResult(errorMessage);
            }
        }

        [Function("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employees/{id}")] HttpRequest req, string id)
        {
            try
            {
                var employee = await _employeeService.GetById(id);
                if (employee == null)
                {
                    return new NotFoundResult();
                }
                _logger.LogInformation($"C# HTTP trigger function processed a request to fetch an employee with id: {id}");
                return new OkObjectResult(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to fetch a employee with id: {id}");
                return new BadRequestObjectResult($"Failed to fetch a employee with id: {id}");
            }
        }

        [Function("GetEmployees")]
        public async Task<IActionResult> GetEmployees([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employees")] HttpRequest req)
        {
            try
            {
                var employees = await _employeeService.GetAll();
                _logger.LogInformation("C# HTTP trigger function processed a request to fetch all employees");
                return new OkObjectResult(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch all employees");
                return new BadRequestObjectResult("Failed to fetch all employees");
            }
        }

        [Function("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "employees/{id}")] HttpRequest req, string id)
        {
            using var reader = new StreamReader(req.Body);
            var empJson = await reader.ReadToEndAsync();
            try
            {
                var employee = JsonConvert.DeserializeObject<Employee>(empJson);
                employee.Id = id;
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(employee);

                if (!Validator.TryValidateObject(employee, validationContext, validationResults, true))
                {
                    var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                    return new BadRequestObjectResult($"Validation failed: {errors}");
                }

                var updatedEmployee = await _employeeService.Update(employee);
                _logger.LogInformation($"C# HTTP trigger function processed a request to update employee with id: {id}");
                return new OkObjectResult(updatedEmployee);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to update employee with id: {id}";
                _logger.LogError(ex, errorMessage);
                return new BadRequestObjectResult(errorMessage);
            }
        }

        [Function("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "employees/{id}")] HttpRequest req, string id)
        {
            try
            {
                await _employeeService.Delete(id);
                _logger.LogInformation($"C# HTTP trigger function processed a request to delete employee with id: {id}");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete employee with id: {id}");
                return new BadRequestObjectResult($"Failed to delete employee with id: {id}");
            }
        }
    }
}
