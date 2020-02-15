using SeedlingOnlineJudge.Model;
using SeedlingOnlineJudge.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Database
{
    public class UserManager : IDatabase
    {
        private readonly Cipher _cipher;
        public UserManager(Cipher cipher)
        {
            _cipher = cipher;
        }

        public User GetUserByUsername(string username)
        {
            return Read<User>(username);
        }

        public void SaveUser(User user)
        {
            if (user == null)
                throw new Exception("user cannot be null");
            if(string.IsNullOrWhiteSpace(user.Username))
                throw new Exception("username cannot be null");
            if(string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("password cannot be null");

            EncryptPassword(user);

            Save<User>(user);
        }

        public User GetUser(string username)
        {
            var user = Read<User>(username);

            return user;
        }

        public bool ValidateUser(string username, string password)
        {
            var user = GetUser(username);
            if (user == null) return false;
            var decrypedPassword = _cipher.Decrypt(user.Password);
            if (!decrypedPassword.Equals(password))
                return false;
            return true;
        }

        public User UpdateUser(string username, User updatedUser)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("username cannot be null");

            var user = UserExist(username);
            if (user == null)
                throw new Exception($"user {username} not found");

            EncryptPassword(updatedUser);

            user.Copy(updatedUser);

            Save<User>(user);

            return user;
        }

        public void EncryptPassword(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
                return;
            var encryptedPass = _cipher.Encrypt(user.Password);
            user.Password = encryptedPass;
        }

        public string DecryptPassword(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
                return "";
            var decryptedPass = _cipher.Decrypt(user.Password);

            return decryptedPass;
        }

        public User UserExist(string username)
        {
            var res = Read<User>(username);

            return res;
        }
    }
}
