using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawl_College_Entrance_Scores.dto
{
    public class MajorCollegeDTO
    {
        private long id {get; set;}
        private string collegeID {get; set;}
        private string majorID {get; set;}
        private int year {get; set;}
        private string groupCode {get; set;}
        private double score {get; set;}
    }
}