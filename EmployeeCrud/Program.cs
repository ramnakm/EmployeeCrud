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

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Swagger Document",
                Version = "v1",
                Description = "Swagger UI for Azure Functions"
            });
            c.CustomOperationIds(apiDesc =>
            {
                return apiDesc.TryGetMethodInfo(out MethodInfo mInfo) ? mInfo.Name : default(Guid).ToString();
            });
        });
    })
    .Build();

host.Run();