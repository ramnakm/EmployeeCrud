using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Helpers;
using Microsoft.Extensions.Configuration;

namespace EmployeeCrud
{
    // FunctionConfiguration class to hold configuration settings for the function
    public class FunctionConfiguration
    {
        // Endpoint for the Employee account
        public string EmpAccountEndpoint { get; }

        // Key for the Employee account
        public string EmpAccountKey { get; }

        // Name of the Employee database
        public string EmpDatabaseName { get; }

        // Constructor to initialize configuration settings
        public FunctionConfiguration(IConfiguration config) 
        {
            // Retrieve the Employee account endpoint from the configuration
            EmpAccountEndpoint = config["EmpAccountEndpoint"];

            // Retrieve the Employee account key from Azure Key Vault
            EmpAccountKey = KeyVaultHelper.GetSecretAsync(config["KeyVaultUrl"], "Cosmos-EmpDb-AccKey").Result;

            // Retrieve the Employee database name from the configuration
            EmpDatabaseName = config["EmpDatabaseName"];
        }
    }
}
