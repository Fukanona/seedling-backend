using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Infrastructure.File
{
    public static class PathManager
    {
        public static string GetPath(params string[] path)
        {
            return Path.Combine(path);
        }
    }
}
