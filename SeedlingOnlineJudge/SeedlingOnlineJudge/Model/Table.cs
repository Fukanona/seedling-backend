using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Model
{
    public abstract class Table<T> where T : class
    {
        internal static string Folder => $"tables/{typeof(T).Name.ToLower()}";
        public abstract string GetTableKey();
    }
}
