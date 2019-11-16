
using CryptoHelper;

namespace Sample.Data.Access.Helpers
{

    public static class EncryptionHelper
    {
        public static string WithCrypt(this string text) => Crypto.HashPassword(text);      

        public static bool VerifyCrypt(this string hashedPassword, string plainText) => Crypto.VerifyHashedPassword(hashedPassword, plainText);
     
    }
}
