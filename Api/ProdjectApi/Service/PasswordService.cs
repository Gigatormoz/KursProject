using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ProdjectApi.Service.Contracts;
using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Service
{
    public class PasswordService : IPasswordService 
    {
        public string GeneratePasswordHash(string password)
        {
            var salt = Guid.NewGuid().ToByteArray();
            var hash = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100_000,
                    numBytesRequested: 256 / 8
                    )
                );

            return $"{Convert.ToBase64String(salt)}.{hash}";
        }

        public bool VerifyPassword(string plainPassword, string storedHash)
        {
            var parts = storedHash.Split('.');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            var computedHash = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: plainPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100_000,
                    numBytesRequested: 256 / 8
                )
            );

            return computedHash == hash;
        }
    }


}
