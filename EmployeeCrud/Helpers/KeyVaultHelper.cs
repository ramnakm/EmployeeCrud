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
    public static class KeyVaultHelper
    {
        public static async Task<string> GetSecretAsync(string keyVaultUrl, string secretName)
        {
            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            KeyVaultSecret secret = await client.GetSecretAsync(secretName);
            return secret.Value;
        }
    }
}
