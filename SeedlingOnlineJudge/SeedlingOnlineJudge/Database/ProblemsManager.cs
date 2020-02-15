using SeedlingOnlineJudge.Model;
using SeedlingOnlineJudge.Util;
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

        public ProblemDescription UpdateProblem(ProblemDescription problem, ProblemDescription updatedProblem)
        {
            problem.Copy(updatedProblem);
            problem.LastUpdate = Helper.GetDateTimeNowBrazil();

            Save<ProblemDescription>(problem);

            return problem;
        }

        public Author GetProblemAuthor(string problemId)
        {
            var problem = Read<ProblemDescription>(problemId);
            if (problem == null) return null;
            return problem.Author;
        }

        public bool HasPermission(string problemId, User user)
        {
            var username = GetProblemAuthor(problemId).Username;
            return (username?.Equals(user.Username) ?? false);
        }

        public void SetProblemActive(string problemId)
        {
            var problem = GetProblemById(problemId);
            if (problem == null) return;
            problem.Active = true;

            Save<ProblemDescription>(problem);
        }

        public bool IsProblemActive(string problemId)
        {
            var problem = GetProblemById(problemId);
            if (problem == null) return false;

            return problem.Active.Value;
        }
    }
}
