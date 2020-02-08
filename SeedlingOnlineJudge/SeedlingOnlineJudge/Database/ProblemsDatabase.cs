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
                Description = "Easy problem",
                Level = ProblemLevel.EASY.ToString()
            },
            new ProblemDto
            {
                Id = "2",
                Description = "Medium problem",
                Level = ProblemLevel.MEDIUM.ToString()
            },
            new ProblemDto
            {
                Id = "3",
                Description = "Hard problem",
                Level = ProblemLevel.HARD.ToString()
            },
            new ProblemDto
            {
                Id = "4",
                Description = "Insane problem",
                Level = ProblemLevel.INSANE.ToString()
            }
        };

        public ProblemsDatabase() { }

        public ProblemDto GetProblemById(string problemId)
        {
            return _problems.FirstOrDefault(item => item.Id.Equals(problemId));
        }
    }
}
