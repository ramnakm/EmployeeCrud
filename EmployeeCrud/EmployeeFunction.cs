using System.ComponentModel.DataAnnotations;
using System.Net;
using EmployeeCrud.Models;
using EmployeeCrud.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace EmployeeCrud
{
    /// <summary>
    /// This class contains the functions for handling employee operations.
    /// </summary>
    public class EmployeeFunction
    {
        private readonly ILogger<EmployeeFunction> _logger;

        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeFunction"/> class.
        /// </summary>
        public EmployeeFunction(ILogger<EmployeeFunction> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        /// <summary>
        /// This function creates a new employee. OpenAPI attributes are used to generate the Swagger UI.
        /// </summary>
        /// <param name="req">The HTTP request containing the employee data.</param>
        /// <returns>An IActionResult indicating the result of the operation</returns>
        [OpenApiOperation(operationId: "Run", tags: new[] { "Create Employee" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Employee), Required = true, Description = "Employee object that needs to be added")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        [Function("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employees")] HttpRequest req)
        {
            using var reader = new StreamReader(req.Body);
            var empJson = await reader.ReadToEndAsync();
            try
            {
                // Deserialize the request body to an Employee object
                var employee = JsonConvert.DeserializeObject<Employee>(empJson);
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(employee);

                // Validate the Employee object
                if (!Validator.TryValidateObject(employee, validationContext, validationResults, true))
                {
                    var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                    return new BadRequestObjectResult($"Validation failed: {errors}");
                }

                // Create the employee using the service
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

        /// <summary>
        /// This function retrieves an employee by their ID. OpenAPI attributes are used to generate the Swagger UI.
        /// </summary>
        /// <param name="req">The HTTP request.</param>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>An IActionResult containing the employee data or a not found result.</returns>
        [OpenApiOperation(operationId: "Run", tags: new[] { "Get Employee By Id" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The employee id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        [Function("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employees/{id}")] HttpRequest req, string id)
        {
            try
            {
                // Retrieve the employee by ID using the service.
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

        /// <summary>
        /// This function retrieves all employees. OpenAPI attributes are used to generate the Swagger UI.
        /// </summary>
        /// <param name="req">The HTTP request.</param>
        /// <returns>An IActionResult containing the list of employees.</returns>
        [OpenApiOperation(operationId: "Run", tags: new[] { "Get Employees" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        [Function("GetEmployees")]
        public async Task<IActionResult> GetEmployees([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employees")] HttpRequest req)
        {
            try
            {
                // Retrieve all employees using the service
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

        /// <summary>
        /// This function updates an employee by their ID. OpenAPI attributes are used to generate the Swagger UI.
        /// </summary>
        /// <param name="req">The HTTP request containing the updated employee data.</param>
        /// <param name="id">The ID of the employee to update.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [OpenApiOperation(operationId: "Run", tags: new[] { "Update Employee" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Employee), Required = true, Description = "Employee object that needs to be updated")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The employee id")]    
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        [Function("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "employees/{id}")] HttpRequest req, string id)
        {
            using var reader = new StreamReader(req.Body);
            var empJson = await reader.ReadToEndAsync();
            try
            {
                // Deserialize the request body to an Employee object
                var employee = JsonConvert.DeserializeObject<Employee>(empJson);
                employee.Id = id;
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(employee);

                // Validate the Employee object
                if (!Validator.TryValidateObject(employee, validationContext, validationResults, true))
                {
                    var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                    return new BadRequestObjectResult($"Validation failed: {errors}");
                }

                // Update the employee using the service
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

        /// <summary>
        /// This function deletes an employee by their ID. OpenAPI attributes are used to generate the Swagger UI.
        /// </summary>
        /// <param name="req">The HTTP request.</param>
        /// <param name="id">The ID of the employee to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [OpenApiOperation(operationId: "Run", tags: new[] { "Delete Employee" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The employee id")]    
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        [Function("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "employees/{id}")] HttpRequest req, string id)
        {
            try
            {
                // Delete the employee using the service
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