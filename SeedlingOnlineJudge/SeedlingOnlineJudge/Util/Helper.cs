using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Util
{
    public static class Helper
    {
        public static void CreateFolderIfNecessary(string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        public class StrToIntAscComparator : IComparer<string>
        {
            public int Compare(string sx, string sy)
            {
                int x = Convert.ToInt32(sx);
                int y = Convert.ToInt32(sy);
                if (x == 0 || y == 0)
                {
                    return 0;
                }

                // CompareTo() method 
                return x.CompareTo(y);

            }
        }
    }
}
