using System.ComponentModel.DataAnnotations;

namespace ProdjectApi.Service.Contracts
{
    public interface IPasswordService
    {
        string GeneratePasswordHash(string password);
        bool VerifyPassword(string plainPassword, string storedHash);
    }
}
