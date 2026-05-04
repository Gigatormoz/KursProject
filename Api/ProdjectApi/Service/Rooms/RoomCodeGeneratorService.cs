using System.Security.Cryptography;

namespace ProdjectApi.Service.Rooms
{
    public class RoomCodeGenerator
    {
        private const int Length = 12;

        private static readonly char[] Chars = "ABCDEFGHJKMNPQRSTUVWXYZ1234567890abcdefghijkmnpqrstuvwxwyz".ToCharArray();

        public string Generate()
        {
            var result = new char[Length];
            using var rng = RandomNumberGenerator.Create();

            for (int i = 0; i < Length; i++)
            {
                var bytes = new byte[1];
                rng.GetBytes(bytes);
                result[i] = Chars[bytes[0] % Chars.Length];
            }

            return new string(result);
        }
    }
}
