using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Payload
{
	public class JsonScoreMajorGroupByYears
	{
		public JsonScoreMajorGroupByYears(string majorCode, string majorName, double score, string groupCode, string info = "")
		{
			this.majorCode = majorCode;
			this.majorName = majorName;
			this.score = score;
			this.groupCode = groupCode;
			this.info = info;
		}

		public JsonScoreMajorGroupByYears() { }

		public string majorCode { get; set; }
		public string majorName { get; set; }
		public double score { get; set; }
		public string groupCode { get; set; }
		public string info { get; set; }
	}


	public class JsonMajorsInCollegeGroupByYears
	{
		public JsonMajorsInCollegeGroupByYears(int year, List<JsonScoreMajorGroupByYears> majors)
		{
			this.year = year;
			this.majors = majors;
		}

		public JsonMajorsInCollegeGroupByYears()
		{

		}

		public int year { get; set; }
		public List<JsonScoreMajorGroupByYears> majors { get; set; }
	}


	public class JsonCollegeGroupByYears
	{
		public JsonCollegeGroupByYears(string collegeCode, string collegeName, List<JsonMajorsInCollegeGroupByYears> majors)
		{
			this.collegeCode = collegeCode;
			this.collegeName = collegeName;
			this.majors = majors;
		}

		public JsonCollegeGroupByYears() { }

		public string collegeCode { get; set;}
		public string collegeName { get; set; }
		public List<JsonMajorsInCollegeGroupByYears> majors { get; set; }
	}
}
