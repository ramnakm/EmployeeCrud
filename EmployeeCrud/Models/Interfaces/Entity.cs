using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EmployeeCrud.Models.Interfaces
{
    // Abstract base class representing a generic entity
    public abstract class Entity : IEntity
    {
        // Property to hold the unique identifier of the entity
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
