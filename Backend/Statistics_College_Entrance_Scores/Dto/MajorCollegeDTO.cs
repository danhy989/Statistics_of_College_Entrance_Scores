using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Dto
{
	public class MajorCollegeDTO
	{
		public MajorCollegeDTO(string collegeId, string majorId)
		{
			this.collegeId = collegeId;
			this.majorId = majorId;
		}

		public MajorCollegeDTO()
		{

		}

		public string collegeId { get; set; }
		public string majorId { get; set; }
	}
}
