using System;
using System.Security.Cryptography;
using System.Text;

namespace RWA_Web_API_.Models
{
    public class Encryption
    {
        public static string CreateSalt(int size)
        {
            byte[] buff = new byte[size];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256 sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool Equals(string plainTextInput, string hashedInput, string salt)
        {
            string newHashedPin = GenerateHash(plainTextInput, salt);
            return newHashedPin.Equals(hashedInput);
        }

        public static bool ValidatePassword(string password, string salt, string hashedPassword)
        {
            string newPasswordHash = GenerateHash(password, salt);
            return newPasswordHash.Equals(hashedPassword);
        }
    }
}
