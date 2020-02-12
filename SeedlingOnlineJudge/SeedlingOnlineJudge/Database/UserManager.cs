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
            Save<User>(user);
        }
    }
}
