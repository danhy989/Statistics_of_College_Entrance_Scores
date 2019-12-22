using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Statistics_College_Entrance_Scores.entity;

namespace Statistics_College_Entrance_Scores.dto
{
    public class CollegeDTO
    {
        public CollegeDTO(string collegeCode, List<int> years)
        {
            this.collegeCode = collegeCode;
            this.years = years;
        }

        public string collegeCode { get; set; }
        public List<int> years { get; set; }
    }

    public class ScoreCollegeComparedDTO
    {
        public ScoreCollegeComparedDTO(string majorCode, IList<string> collegeCodes, int year)
        {
            this.majorCode = majorCode;
            this.collegeCodes = collegeCodes;
            this.year = year;
        }

        public string majorCode { get; set; }
        public IList<string> collegeCodes { get; set; }
        public int year { get; set; }
    }
}