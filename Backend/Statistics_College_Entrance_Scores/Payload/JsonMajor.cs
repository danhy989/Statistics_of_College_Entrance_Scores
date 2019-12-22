using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Dto
{
    public class ScoreCollege
    {
        public ScoreCollege()
        {
        }

        public ScoreCollege(string collegeCode, string collegeName, IList<JsonScore> scores)
        {
            this.collegeCode = collegeCode;
            this.collegeName = collegeName;
            this.scores = scores;
        }

        public string collegeCode { get; set; }
        public string collegeName { get; set; }
        public IList<JsonScore> scores { get; set; } = new List<JsonScore>();
    }

    public class JsonMajor
    {
        public JsonMajor()
        {
        }

        public JsonMajor(string majorCode, string majorName, IList<int> years, IList<ScoreCollege> colleges)
        {
            this.majorCode = majorCode;
            this.majorName = majorName;
            this.years = years;
            this.colleges = colleges;
        }

        public string majorCode { get; set; }
        public string majorName { get; set; }
        public IList<int> years { get; set; } = new List<int>();
        public IList<ScoreCollege> colleges { get; set; } = new List<ScoreCollege>();
    }
}
