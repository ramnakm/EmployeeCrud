using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.DAL;
using EmployeeCrud.Models;

namespace EmployeeCrud.Repositories
{
    // EmployeeRepository class inheriting from the generic Repository class
    public class EmployeeRepository : Repository<Employee>
    {
        // Constructor to initialize the EmployeeContext
        public EmployeeRepository(EmployeeContext dbContext) : base(dbContext) { }
    }
}
