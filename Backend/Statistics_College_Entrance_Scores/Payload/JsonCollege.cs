using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Dto
{
    public class ScoreMajor
    {
        public ScoreMajor()
        {
        }

        public ScoreMajor(string majorCode, string majorName, IList<JsonScore> scores)
        {
            this.majorCode = majorCode;
            this.majorName = majorName;
            this.scores = scores;
        }

        public string majorCode { get; set; }
        public string majorName { get; set; }
        public IList<JsonScore> scores { get; set; } = new List<JsonScore>();
    }

    public class JsonCollege
    {
        public JsonCollege()
        {
        }

        public JsonCollege(string collegeCode, string collegeName, IList<int> years, IList<ScoreMajor> majors)
        {
            this.collegeCode = collegeCode;
            this.collegeName = collegeName;
            this.years = years;
            this.majors = majors;
        }

        public string collegeCode { get; set; }
        public string collegeName { get; set; }
        public IList<int> years { get; set; } = new List<int>();
        public IList<ScoreMajor> majors { get; set; } = new List<ScoreMajor>();
    }
}
