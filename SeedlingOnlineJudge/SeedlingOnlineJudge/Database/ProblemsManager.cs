using SeedlingOnlineJudge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Database
{
    public class ProblemsManager : IDatabase
    {
        public ProblemsManager() { }

        public ProblemDescription GetProblemById(string problemId)
        {
            return Read<ProblemDescription>(problemId);
        }

        public List<string> GetAllProblemsIds()
        {
            return ReadAll<ProblemDescription>()?.Select(item => item.Id).ToList();
        }

        public List<ProblemDescription> GetAllProblems()
        {
            return ReadAll<ProblemDescription>();
        }

        private void SetNewIdToProblem(ProblemDescription newProblem)
        {
            var lastId = GetAllProblemsIds()?.LastOrDefault();
            if (lastId == null)
                lastId = "0";
            int newID = Convert.ToInt32(lastId) + 1;

            newProblem.Id = newID.ToString();
        }

        public ProblemDescription AddProblem(ProblemDescription newProblem)
        {
            SetNewIdToProblem(newProblem);

            Save(newProblem);

            return newProblem;
        }
    }
}
