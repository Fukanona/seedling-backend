﻿using SeedlingOnlineJudge.Infrastructure.File;
using SeedlingOnlineJudge.Model;
using SeedlingOnlineJudge.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SeedlingOnlineJudge.Database
{
    public class IDatabase
    {
        public IDatabase()
        {
        }
        public void Save<T>(T data) where T : PData<T>
        {
            var path = PathManager.GetPath(FoldersPath.Base, PData<T>.Folder);

            Helper.CreateFolderIfNecessary(path);

            var file = PathManager.GetPath(path, $"{data.GetPDataKey()}.json");

            File.WriteAllText(file, JsonSerializer.Serialize(data), System.Text.Encoding.UTF8);
        }

        public void Save<T>(List<T> datas) where T : PData<T>
        {
            foreach (var data in datas)
                Save<T>(data);
        }

        public T Read<T>(string key, bool prefix = true) where T : PData<T>
        {
            string file = key;
            if(prefix)
                file = PathManager.GetPath(FoldersPath.Base, PData<T>.Folder, $"{key}.json");
            T data;
            try
            {
                data = JsonSerializer.Deserialize<T>(File.ReadAllText(file));
            } catch(Exception ex)
            {
                return null;
            }

            return data;
        }

        public List<T> ReadAll<T>(string basePath = "") where T : PData<T>
        {
            basePath = PathManager.GetPath(FoldersPath.Base, PData<T>.Folder, basePath); 
            List<T> datas = new List<T>();
            string[] allFiles;
            try
            {
                allFiles = Directory.GetFiles(basePath);
            } catch(Exception ex)
            {
                return null;
            }
            foreach(var file in allFiles)
            {
                T data;
                try
                {
                    data = Read<T>(file, false);
                } catch(Exception ex)
                {
                    data = null;
                }
                if (data != null) datas.Add(data);
            }
            return datas;
        }
    }
}
