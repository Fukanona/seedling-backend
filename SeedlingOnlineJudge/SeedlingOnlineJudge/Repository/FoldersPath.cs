using SeedlingOnlineJudge.Infrastructure.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Util
{
    public static class FoldersPath
    {
        public static readonly string Base = PathManager.GetPath("..", "..", "datas");

        public static readonly string IOLocation = PathManager.GetPath(Base, "io");
        public static readonly string SolutionLocation = PathManager.GetPath(Base, "solution");
    }
}
