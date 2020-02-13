using SeedlingOnlineJudge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Database
{
    public class UserManager : IDatabase
    {
        public UserManager() { }

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
            
            Save<User>(user);
        }
    }
}
