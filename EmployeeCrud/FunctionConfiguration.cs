using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Helpers;
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
            EmpAccountKey = KeyVaultHelper.GetSecretAsync(config["KeyVaultUrl"], "Cosmos-EmpDb-AccKey").Result;
            EmpDatabaseName = config["EmpDatabaseName"];
        }
    }
}
