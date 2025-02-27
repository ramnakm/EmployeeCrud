using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace EmployeeCrud.Models.Interfaces
{
    public class Person : Entity
    {
        private string _name;
        private string _position;

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100 characters.")]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [Required(ErrorMessage = "Position is required.")]
        [StringLength(50, ErrorMessage = "Position length can't be more than 50 characters.")]
        public string Position
        {
            get => _position;
            set => _position = value;
        }
    }
}
