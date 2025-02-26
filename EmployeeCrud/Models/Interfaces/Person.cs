using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EmployeeCrud.Models.Interfaces
{
    public abstract class Person : Entity
    {
        private string _name;
        private string _position;

        public string Id { get; set; }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Position
        {
            get => _position;
            set => _position = value;
        }
    }
}
