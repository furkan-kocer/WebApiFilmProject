using System.Security.Cryptography;
using System.Text;

namespace FilmProject.DataAccess.Helpers
{
    public class HelperService
    {
        public static string GenerateUniqueCode(string name)
        {
            // Generate a random salt
            var salt = Guid.NewGuid().ToString();

            // Combine the film name with the salt
            var input = $"{name}-{salt}";

            // Use SHA256 to create a hash from the combined input
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Convert hash to Base64 and replace URL-unsafe characters
            var filmCode = Convert.ToBase64String(bytes)
                            .Replace("+", "")  // Remove "+"
                            .Replace("/", "")  // Remove "/"
                            .Replace("=", ""); // Remove "="


            // Optionally, trim the hash to a shorter length for readability
            return filmCode.Substring(0, 20); // e.g., a 20-character unique code
        }
    }
}
