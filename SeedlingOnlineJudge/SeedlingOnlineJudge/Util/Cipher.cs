using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Util
{
    public class Cipher
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "9rrO8m1lPwRVVb4dyoCAl1ZszlwjgvRl";

        public Cipher(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public string Encrypt(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Protect(input);
        }

        public string Decrypt(string cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Unprotect(cipherText);
        }
    }
}
