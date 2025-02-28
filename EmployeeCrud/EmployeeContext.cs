using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCrud
{
    public class EmployeeContext : DbContext
    {
        private readonly FunctionConfiguration _config;

        // DbSet property for Employee entities
        public DbSet<Employee> Employees { get; set; }

        // Constructor to initialize the configuration
        public EmployeeContext(FunctionConfiguration config)
        {
            _config = config;
        }

        // Configure the model creation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasPartitionKey("Id");
            modelBuilder.Entity<Employee>().ToContainer("Employee");
        }

        // Configure the database options
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use Cosmos DB with the provided configuration
            optionsBuilder.UseCosmos(
                _config.EmpAccountEndpoint,
                _config.EmpAccountKey,
                _config.EmpDatabaseName);
        }
    }
}
