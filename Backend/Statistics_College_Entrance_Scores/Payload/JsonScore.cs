using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Dto
{
    public class JsonGuessScore
    {
        public JsonGuessScore()
        {
               
        }

        public JsonGuessScore(string collegeCode,string collegeName,string majorCode,string majorName,List<JsonScore> jsonScores)
        {
            this.collegeCode = collegeCode;
            this.collegeName = collegeName;
            this.majorCode = majorCode;
            this.majorName = majorName;
            this.jsonScores = jsonScores;
        }

        public string collegeCode { get; set; }
        public string collegeName { get; set; }
        public string majorCode { get; set; }
        public string majorName { get; set; }
        public List<JsonScore> jsonScores { get; set; }
    }

    public class JsonScore
    {
        public JsonScore()
        {

        }

        public JsonScore(int year, double score, string groupCode,string info="")
        {
            this.year = year;
            this.score = score;
            this.groupCode = groupCode;
            this.info = info;
        }

        public int year { get; set; }
        public double score { get; set; }
        public string groupCode { get; set; }

        public string info { get; set; }
    }
}
