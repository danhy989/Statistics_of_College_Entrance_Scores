using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crawl_College_Entrance_Scores.entity;

namespace Crawl_College_Entrance_Scores.dto
{
    public class MajorDTO
    {
        private string code { get; set; }
        private string name { get; set; }
        public ICollection<MajorCollege> MajorColleges {get; set;}
    }
}