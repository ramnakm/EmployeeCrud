using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EmployeeCrud
{
    public class FunctionConfiguration
    {
        public string EmpAccountEndpoint { get; }
        public string EmpAccountKey { get; }
        public string EmpDatabaseName { get; }

        public FunctionConfiguration(IConfiguration config) 
        {
            EmpAccountEndpoint = config["EmpAccountEndpoint"];
            EmpAccountKey = config["EmpAccountKey"];
            EmpDatabaseName = config["EmpDatabaseName"];
        }
    }
}
