using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Util
{
    public static class Helper
    {
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

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

        public static DateTime GetDateTimeNowBrazil()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById(IsWindows ? "E. South America Standard Time" : "America/Sao_Paulo"));
        }

        public static async Task<string> SaveAFileToDiskAsync(IFormFile file, string fileName, string path)
        {
            if(file.Length > 0)
            {
                CreateFolderIfNecessary(path);
                var filePath = Path.Combine(path, fileName);                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return filePath;
            }

            return null;
        }
    }
}
