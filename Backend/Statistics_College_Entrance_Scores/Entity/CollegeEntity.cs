using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.entity
{
	public class CollegeEntity
	{
		[Key]
		public string code { get; set; }
		public string name { get; set; }

		public long province_id { get; set; }

		public Province province { get; set; }

		public ICollection<MajorCollege> MajorColleges { get; set; }

		public CollegeEntity(){}
	}
}
