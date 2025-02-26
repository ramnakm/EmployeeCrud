using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Models;

namespace EmployeeCrud.Repositories
{
    public class EmployeeRepository : Repository<Employee>
    {
        public EmployeeRepository(EmployeeContext dbContext) : base(dbContext) { }
    }
}
