using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crawl_College_Entrance_Scores.entity;
using Microsoft.AspNetCore.Mvc;

namespace Crawl_College_Entrance_Scores.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		// GET api/values
		[HttpGet]
		public ActionResult<IEnumerable<CollegeEntity>> Get()
		{
			using (var db = new EntranceScoresContext())
			{
				var blogs = db.collegeEntities.ToList();
				foreach (var name in blogs){

				}

				return blogs;
			}
			
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
			return "value";
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
