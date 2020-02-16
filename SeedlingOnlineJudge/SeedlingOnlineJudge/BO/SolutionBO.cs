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
            var pathToSolution = @$"{FoldersPath.SolutionLocation}\{user.Username}";
            var completePathToSolution = @$"{pathToSolution}\{problemId}.cpp";
            CmdManager.RunCommand(@$"g++ {completePathToSolution} -o {pathToSolution}\a.exe -std=c++11");
        }

        public void CreateUserOutput(User user, string problemId)
        {
            var completePathToIn = @$"{FoldersPath.IOLocation}\{problemId}.in";

            var pathToSolution = @$"{FoldersPath.SolutionLocation}\{user.Username}";

            CmdManager.RunCommand(@$"{pathToSolution}\a.exe < {completePathToIn} > {pathToSolution}\userout-{problemId}.txt");
        }

        public void CreateResult(User user, string problemId)
        {
            var completePathToOut = @$"{FoldersPath.IOLocation}\{problemId}.out";

            var pathToSolution = $@"{FoldersPath.SolutionLocation}\{user.Username}";

            CmdManager.RunCommand(@$"fc {completePathToOut} {pathToSolution}\userout-{problemId}.txt > {pathToSolution}\res-{problemId}.txt");
        }

        public bool ValidateSolution(User user, string problemId)
        {
            var pathToRes= @$"{FoldersPath.SolutionLocation}\{user.Username}\res-{problemId}.txt";
            var res = _fileManager.Read(pathToRes);
            if (res.Contains("FC: no differences encountered")) return true;
            return false;
        }

        public void CleanAll(User user, string problemId)
        {
            var pathToSolution = $@"{FoldersPath.SolutionLocation}\{user.Username}";
            CmdManager.RunCommand($@"del {pathToSolution}\a.exe");
            CmdManager.RunCommand($@"del {pathToSolution}\userout-{problemId}.txt");
            CmdManager.RunCommand($@"del {pathToSolution}\res-{problemId}.txt");
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
