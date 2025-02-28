using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Models;
using EmployeeCrud.Repositories.Interfaces;
using EmployeeCrud.Services.Interfaces;

namespace EmployeeCrud.Services
{
    // EmployeeService class inheriting from the generic Service class and implementing the IEmployeeService interface
    public class EmployeeService : Service<Employee> , IEmployeeService
    {
        // Constructor to initialize the EmployeeService with the repository
        public EmployeeService(IRepository<Employee> repository) : base(repository) { }
    }
}
