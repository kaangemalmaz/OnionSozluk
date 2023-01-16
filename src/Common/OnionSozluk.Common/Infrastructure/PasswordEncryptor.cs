using System.Security.Cryptography;
using System.Text;

namespace OnionSozluk.Common.Infrastructure
{
    public static class PasswordEncryptor
    {
        public static string Encrpt(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password); // string to bytes
                byte[] hashBytes = md5.ComputeHash(inputBytes); // hash

                return Convert.ToHexString(hashBytes); // byte to string
            }
        }
    }
}
