using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Models;

namespace EmployeeCrud.Services.Interfaces
{
    // Interface for employee-specific service operations
    public interface IEmployeeService : IService<Employee>
    {
    }
}
