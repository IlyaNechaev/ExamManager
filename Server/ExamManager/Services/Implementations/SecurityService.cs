using System.Security.Cryptography;
using System.Text;

namespace ExamManager.Services
{
    public class SecurityService : ISecurityService
    {
        public string Encrypt(string source)
        {
            string? hash = null;
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }

            return hash;
        }

        public string GeneratePassword(int passwordLength)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < passwordLength; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString().ToLower();
        }

        public bool ValidatePassword(string password, string hash)
        {
            return hash.Equals(Encrypt(password));
        }
    }
}
