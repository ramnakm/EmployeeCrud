using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EmployeeCrud.Models.Interfaces
{
    public abstract class Entity : IEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
