using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.entity
{
	public class MajorCollege
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public long id { get; set; }

		[ForeignKey("CollegeEntity")]
		public string CollegeEntityId { get; set; }
	
		[ForeignKey("MajorEntity")]
		public string MajorEntityId { get; set; }

		public CollegeEntity CollegeEntity { get; set; }
		public MajorEntity MajorEntity { get; set; }

		public int year { get; set; }
		public string groupCode { get; set; }

		public double score { get; set; }

	}
}
