using Statistics_College_Entrance_Scores.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Payload
{
	public class JsonMajorCollege
	{
		public string collegeCode { get; set; }
		public string collegeName { get; set; }
		public string majorCode { get; set; }
		public string majorName { get; set; }
		public IList<JsonScore> scores { get; set; }

	}
}
