using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Payload
{
	public class JsonScoreCollegeGroupByYears
	{

		public JsonScoreCollegeGroupByYears() { }

		public JsonScoreCollegeGroupByYears(string collegeCode, string collegeName, double score, string groupCode, string info = "")
		{
			this.collegeCode = collegeCode;
			this.collegeName = collegeName;
			this.score = score;
			this.groupCode = groupCode;
			this.info = info;
		}

		public string collegeCode { get; set; }
		public string collegeName { get; set; }
		public double score { get; set; }
		public string groupCode { get; set; }
		public string info { get; set; }
	}


	public class JsonCollegesInMajorGroupByYears
	{

		public JsonCollegesInMajorGroupByYears()
		{

		}

		public JsonCollegesInMajorGroupByYears(int year, List<JsonScoreCollegeGroupByYears> colleges)
		{
			this.year = year;
			this.colleges = colleges;
		}

		public int year { get; set; }
		public List<JsonScoreCollegeGroupByYears> colleges { get; set; }
	}


	public class JsonMajorGroupByYears
	{
	
		public JsonMajorGroupByYears() { }

		public JsonMajorGroupByYears(string majorCode, string majorName, List<JsonCollegesInMajorGroupByYears> colleges)
		{
			this.majorCode = majorCode;
			this.majorName = majorName;
			this.colleges = colleges;
		}

		public string majorCode { get; set; }
		public string majorName { get; set; }
		public List<JsonCollegesInMajorGroupByYears> colleges { get; set; }
	}
}
