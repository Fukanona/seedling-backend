using Microsoft.AspNetCore.Http;
using SeedlingOnlineJudge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Infrastructure.File
{
    public class FileManager
    {
        public FileManager()
        {
        }

        public async Task<string> SaveAsync(IFormFile file, string path, string filename)
        {
            if (file.Length > 0)
            {
                Helper.CreateFolderIfNecessary(path);
                var filePath = Path.Combine(path, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return filePath;
            }
            return null;
        }

        public string Read(string path)
        {
            using (StreamReader streamReader = new StreamReader(path, Encoding.ASCII, true))
            {
                string text = streamReader.ReadToEnd();

                return text;
            }
        }
    }
}
