using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawl_College_Entrance_Scores
{
	public class Major
	{
		private string code;
		private string name;
		private string groupCode;
		private double score;

		public Major(string code, string name, string groupCode, double score)
		{
			this.code = code;
			this.name = name;
			this.groupCode = groupCode;
			this.score = score;
		}

		public Major()
		{

		}

		public string Code { get => code; set => code = value; }
		public string Name { get => name; set => name = value; }
		public string GroupCode { get => groupCode; set => groupCode = value; }
		public double Score { get => score; set => score = value; }
	}
}
