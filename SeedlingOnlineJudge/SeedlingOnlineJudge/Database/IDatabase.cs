using Newtonsoft.Json;
using SeedlingOnlineJudge.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Database
{
    public class IDatabase
    {
        public void Save<T>(T data) where T : ITable
        {
            var path = data.Path;
            File.AppendAllText(path, JsonConvert.SerializeObject(data));
        }

        //public List<T> Read<T>(string path) where T : ITable
        //{
        //    var file = 
        //}
    }
}
