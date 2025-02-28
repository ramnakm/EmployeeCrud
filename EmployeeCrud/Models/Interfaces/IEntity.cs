using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCrud.Models.Interfaces
{
    // IEntity interface representing a generic entity with an Id property
    public interface IEntity
    {
        public string Id { get; set; }
    }
}
