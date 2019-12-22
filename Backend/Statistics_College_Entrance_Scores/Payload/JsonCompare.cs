using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Payload
{
	public class JsonCompareColleges
	{
		public JsonCompareColleges(string collegeCode, string collegeName, double score, string groupCode)
		{
			this.collegeCode = collegeCode;
			this.collegeName = collegeName;
			this.score = score;
			this.groupCode = groupCode;
		}

		public JsonCompareColleges() { }

		public string collegeCode { get; set; }
		public string collegeName { get; set; }
		public double score { get; set; }
		public string groupCode { get; set; }
	}
	public class JsonCompare
	{
		public JsonCompare(string majorCode, string majorName, int year, List<JsonCompareColleges> colleges)
		{
			this.majorCode = majorCode;
			this.majorName = majorName;
			this.year = year;
			this.colleges = colleges;
		}

		public JsonCompare() { }

		public string majorCode { get; set; }
		public string majorName { get; set; }
		public int year { get; set; }
		public List<JsonCompareColleges> colleges { get; set; }
	}
}
