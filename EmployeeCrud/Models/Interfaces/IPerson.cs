using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCrud.Models.Interfaces
{
    public interface IPerson
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
