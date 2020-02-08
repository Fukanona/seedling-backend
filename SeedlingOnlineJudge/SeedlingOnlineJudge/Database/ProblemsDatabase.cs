using SeedlingOnlineJudge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Database
{
    public class ProblemsDatabase
    {
        List<ProblemDto> _problems = new List<ProblemDto>
        {
            new ProblemDto
            {
                Id = "1",
                Name = "SOJ Problem 1",
                Description = "Easy problem",
                Level = ProblemLevel.EASY.ToString()
            },
            new ProblemDto
            {
                Id = "2",
                Name = "SOJ Problem 2",
                Description = "Medium problem",
                Level = ProblemLevel.MEDIUM.ToString()
            },
            new ProblemDto
            {
                Id = "3",
                Name = "SOJ Problem 3",
                Description = "Hard problem",
                Level = ProblemLevel.HARD.ToString()
            },
            new ProblemDto
            {
                Id = "4",
                Description = "Insane problem",
                Name = "SOJ Problem 4",
                Level = ProblemLevel.INSANE.ToString()
            }
        };

        public ProblemsDatabase() { }

        public ProblemDto GetProblemById(string problemId)
        {
            return _problems.FirstOrDefault(item => item.Id.Equals(problemId));
        }

        private void SetNewIdToProblem(ProblemDto newProblem)
        {
            var lastId = _problems.Last().Id;
            int newID = Convert.ToInt32(lastId) + 1;

            newProblem.Id = newID.ToString();
        }

        public ProblemDto AddProblem(ProblemDto newProblem)
        {
            SetNewIdToProblem(newProblem);

            _problems.Add(newProblem);

            return newProblem;
        }
    }
}
