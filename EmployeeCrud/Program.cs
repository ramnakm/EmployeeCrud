using System.Reflection;
using EmployeeCrud;
using EmployeeCrud.Models;
using EmployeeCrud.Repositories;
using EmployeeCrud.Repositories.Interfaces;
using EmployeeCrud.Services;
using EmployeeCrud.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

// Create and configure the host for the Azure Functions application
var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        // Add Application Insights telemetry for worker services
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Build the configuration from various sources
        var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

        // Register the FunctionConfiguration as a singleton service
        services.AddSingleton(new FunctionConfiguration(config));

        // Add the EmployeeContext for database operations
        services.AddDbContext<EmployeeContext>();

        // Register the EmployeeRepository as the implementation for IRepository<Employee>
        services.AddScoped<IRepository<Employee>, EmployeeRepository>();

        // Register the EmployeeService as the implementation for IEmployeeService
        services.AddScoped<IEmployeeService, EmployeeService>();

        // Configure Swagger for API documentation
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Swagger Document",
                Version = "v1",
                Description = "Swagger UI for Azure Functions"
            });

            // Customize operation IDs for Swagger documentation
            c.CustomOperationIds(apiDesc =>
            {
                return apiDesc.TryGetMethodInfo(out MethodInfo mInfo) ? mInfo.Name : default(Guid).ToString();
            });
        });
    })
    .Build();

// Run the host
host.Run();