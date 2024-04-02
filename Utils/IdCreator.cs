using System;
using System.Security.Cryptography;
using System.Text;

namespace ZealEducation.Utils
{
    public static class IdCreator
    {

        public static string ConcatenateId(params object[] values)
        {
            string id = string.Join("_", values);
            return id;
        }

        public static string HashId(params object[] values)
        {
            //Generate an unique string
            string uniqueString = string.Join("_", values);
            uniqueString += DateTime.Now.ToString();

            // Hash the unique string to ensure uniqueness
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(uniqueString));
            return BitConverter.ToString(bytes).Replace("-", "")[..8];
        }
    }
}
