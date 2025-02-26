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
    public class EmployeeService : Service<Employee> , IEmployeeService
    {
        public EmployeeService(IRepository<Employee> repository) : base(repository) { }
    }
}
