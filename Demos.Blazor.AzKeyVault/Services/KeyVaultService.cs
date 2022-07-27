using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System.Text;

namespace Demos.Blazor.AzKeyVault.Services
{
    public class KeyVaultService
    {
        private readonly Lazy<KeyClient> _keyClient;
        private KeyClient KeyClient => _keyClient.Value;

        public KeyVaultService()
        {
            _keyClient = new Lazy<KeyClient>(() =>
            {
                var kvName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME")!;
                var kvUri = "https://" + kvName + ".vault.azure.net";

                return new KeyClient(new Uri(kvUri), new DefaultAzureCredential());
            });
        }

        public async Task<string> SignData(string key, string data, string algorithm)
        {
            var crypto = KeyClient.GetCryptographyClient(key);
            var bytes = Encoding.UTF8.GetBytes(data);
            var result = await crypto.SignDataAsync(algorithm, bytes);
            var base64Signature = Convert.ToBase64String(result.Signature);

            return base64Signature;
        }

        public async Task<bool> VerifyData(string key, string data, string signature, string algorithm)
        {
            var crypto = KeyClient.GetCryptographyClient(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signatureBytes = Convert.FromBase64String(signature);
            var result = await crypto.VerifyDataAsync(algorithm, dataBytes, signatureBytes);

            return result.IsValid;
        }

        public async Task<string> Encrypt(string key, string plain, string algorithm)
        {
            var crypto = KeyClient.GetCryptographyClient(key);
            var palinBytes = Encoding.UTF8.GetBytes(plain);
            var result = await crypto.EncryptAsync(algorithm, palinBytes);
            var base64Ciphertext = Convert.ToBase64String(result.Ciphertext);

            return base64Ciphertext;
        }

        public async Task<string> Decrypt(string key, string cipher, string algorithm)
        {
            var crypto = KeyClient.GetCryptographyClient(key);
            var cipherBytes = Convert.FromBase64String(cipher);
            var result = await crypto.DecryptAsync(algorithm, cipherBytes);
            var base64plain = Encoding.UTF8.GetString(result.Plaintext);

            return base64plain;
        }

        public async Task<List<string>> ListKeys()
        {
            var keys = new List<string>();
            var result = KeyClient.GetPropertiesOfKeysAsync();
            await foreach (var resultKey in result)
            {
                keys.Add(resultKey.Name);
            }

            return keys;
        }

        public Task<List<string>> ListSignatureAlgorithms()
        {
            var signatureAlgorithms = new List<SignatureAlgorithm>()
            {
                SignatureAlgorithm.ES256,
                SignatureAlgorithm.ES256K,
                SignatureAlgorithm.ES384,
                SignatureAlgorithm.ES512,
                SignatureAlgorithm.PS256,
                SignatureAlgorithm.PS384,
                SignatureAlgorithm.PS512,
                SignatureAlgorithm.RS256,
                SignatureAlgorithm.RS384,
                SignatureAlgorithm.RS512,
            };

            var signatureAlgorithmList = signatureAlgorithms
                .Select(s => s.ToString())
                .ToList();

            return Task.FromResult(signatureAlgorithmList);
        }

        public Task<List<string>> ListEncryptionAlgorithms()
        {
            var encryptionAlgorithms = new List<EncryptionAlgorithm>()
            {
               EncryptionAlgorithm.A128Cbc,
               EncryptionAlgorithm.A128CbcPad,
               EncryptionAlgorithm.A128Gcm,
               EncryptionAlgorithm.A192Cbc,
               EncryptionAlgorithm.A192CbcPad,
               EncryptionAlgorithm.A256Cbc,
               EncryptionAlgorithm.A256CbcPad,
               EncryptionAlgorithm.Rsa15,
               EncryptionAlgorithm.RsaOaep,
               EncryptionAlgorithm.RsaOaep256
            };

            var encryptionAlgorithmList = encryptionAlgorithms
                .Select(s => s.ToString())
                .ToList();

            return Task.FromResult(encryptionAlgorithmList);
        }
    }
}
