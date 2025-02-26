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


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        services.AddSingleton(new FunctionConfiguration(config));

        services.AddDbContext<EmployeeContext>();
        services.AddScoped<IRepository<Employee>, EmployeeRepository>();
        services.AddScoped<IEmployeeService, EmployeeService>();
    })
    .Build();

host.Run();