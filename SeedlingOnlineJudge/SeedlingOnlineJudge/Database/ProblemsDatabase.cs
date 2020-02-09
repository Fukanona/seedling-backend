using SeedlingOnlineJudge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Database
{
    public class ProblemsDatabase : IDatabase
    {
        public ProblemsDatabase() { }

        public ProblemDto GetProblemById(string problemId)
        {
            return Read<ProblemDto>(problemId);
        }

        public List<string> GetAllProblemsIds()
        {
            return ReadAll<ProblemDto>().Select(item => item.Id).ToList();
        }

        private void SetNewIdToProblem(ProblemDto newProblem)
        {
            var lastId = GetAllProblemsIds().Last();
            int newID = Convert.ToInt32(lastId) + 1;

            newProblem.Id = newID.ToString();
        }

        public ProblemDto AddProblem(ProblemDto newProblem)
        {
            SetNewIdToProblem(newProblem);

            Save(newProblem);

            return newProblem;
        }
    }
}
