using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Sample.Security.Auth
{
    public class RSAKeyHelper
    {
        public static RSAParameters GenerateKey()
        {
            using RSACryptoServiceProvider key = new RSACryptoServiceProvider(2048);
            return key.ExportParameters(true);
        }
    }
}
