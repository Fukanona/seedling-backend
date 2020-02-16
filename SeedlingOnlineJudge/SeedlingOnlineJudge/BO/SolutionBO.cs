using Microsoft.AspNetCore.Http;
using SeedlingOnlineJudge.Infrastructure.CMD;
using SeedlingOnlineJudge.Infrastructure.File;
using SeedlingOnlineJudge.Model;
using SeedlingOnlineJudge.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.BO
{
    public class SolutionBO
    {
        private readonly FileManager _fileManager;

        public SolutionBO(FileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public void CompileFile(User user, string problemId)
        {
            var pathToSolution = PathManager.GetPath(FoldersPath.SolutionLocation, user.Username);
            var completePathToSolution = PathManager.GetPath(pathToSolution, $"{problemId}.cpp");
            var outPath = PathManager.GetPath(pathToSolution, "a.exe");
            CmdManager.RunCommand($"g++ {completePathToSolution} -o {outPath} -std=c++11");
        }

        public void CreateUserOutput(User user, string problemId)
        {
            var completePathToIn = PathManager.GetPath(FoldersPath.IOLocation, $"{problemId}.in");

            var pathToSolution = PathManager.GetPath(FoldersPath.SolutionLocation, user.Username);

            var useroutPath = PathManager.GetPath(pathToSolution, $"userout-{problemId}.txt");

            CmdManager.RunCommand(@$"{pathToSolution}\a.exe < {completePathToIn} > {useroutPath}");
        }

        public void CreateResult(User user, string problemId)
        {
            var completePathToOut = PathManager.GetPath(FoldersPath.IOLocation, $"{problemId}.out");

            var pathToSolution = PathManager.GetPath(FoldersPath.SolutionLocation, user.Username);

            var useroutPath = PathManager.GetPath(pathToSolution, $"userout-{problemId}.txt");
            var resPath = PathManager.GetPath(pathToSolution, $"res-{problemId}.txt");

            CmdManager.RunCommand(@$"fc {completePathToOut} {useroutPath} > {resPath}");
        }

        public bool ValidateSolution(User user, string problemId)
        {
            var pathToRes = PathManager.GetPath(FoldersPath.SolutionLocation, user.Username, $"res-{problemId}.txt");
            var res = _fileManager.Read(pathToRes);
            if (res.Contains("FC: no differences encountered")) return true;
            return false;
        }

        public void CleanAll(User user, string problemId)
        {
            var pathToSolution = PathManager.GetPath(FoldersPath.SolutionLocation, user.Username);
            var useroutPath = PathManager.GetPath(pathToSolution, $"userout-{problemId}.txt");
            var resPath = PathManager.GetPath(pathToSolution, $"res-{problemId}.txt");
            var outPath = PathManager.GetPath(pathToSolution, "a.exe");

            CmdManager.RunCommand($"del {outPath}");
            CmdManager.RunCommand($"del {useroutPath}");
            CmdManager.RunCommand($"del {resPath}");
        }

        public bool RunUserSolution(User user, string problemId)
        {
            CompileFile(user, problemId);
            CreateUserOutput(user, problemId);
            CreateResult(user, problemId);
            bool res = ValidateSolution(user, problemId);
            CleanAll(user, problemId);

            return res;
        }
    }
}
