using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace EmployeeCrud.Helpers
{
    // Static helper class for interacting with Azure Key Vault
    public static class KeyVaultHelper
    {
        // Asynchronously retrieves a secret value from Azure Key Vault
        public static async Task<string> GetSecretAsync(string keyVaultUrl, string secretName)
        {
            // Create a SecretClient to interact with the Key Vault
            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

            // Retrieve the secret from the Key Vault
            KeyVaultSecret secret = await client.GetSecretAsync(secretName);

            return secret.Value;
        }
    }
}