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
        public DbSet<Employee> Employees { get; set; }

        public EmployeeContext(FunctionConfiguration config)
        {
            _config = config;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasPartitionKey("Id");
            modelBuilder.Entity<Employee>().ToContainer("Employee");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                _config.EmpAccountEndpoint,
                _config.EmpAccountKey,
                _config.EmpDatabaseName);
        }
    }
}
