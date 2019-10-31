using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Crawl_College_Entrance_Scores.entity;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace Crawl_College_Entrance_Scores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrawlDataController : ControllerBase
    {

		private static ConcurrentStack<MajorCollege> majorColleges = new ConcurrentStack<MajorCollege>();

		private ICollection<MajorEntity> majorEntities = new LinkedList<MajorEntity>();

		private ICollection<CollegeEntity> collegeEntities = new LinkedList<CollegeEntity>();

		public CrawlDataController()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			using (var db = new EntranceScoresContext())
			{
				this.initListValueFromDB(db);
			}
			
		}

		private void initListValueFromDB(EntranceScoresContext db)
		{
			using (db)
			{
				this.majorEntities = db.majorEntities.ToList();
				this.collegeEntities = db.collegeEntities.ToList();
			}
		}

		private static string convertToUnSign3(string s)
		{
			Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			string temp = s.Normalize(NormalizationForm.FormD);
			return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
		}

		private void ProcessCrawling(string url,string collegeCode,int year,EntranceScoresContext db)
		{
			string[] log = new string[1000];
			HtmlAgilityPack.HtmlWeb webSite = new HtmlAgilityPack.HtmlWeb();
			HtmlAgilityPack.HtmlDocument document = webSite.Load(url);

			try
			{
				var html = document.DocumentNode.SelectSingleNode("//div[@class='resul-seah']");

				var tableNode = html.SelectNodes("//table");

				int i = 0;

				Console.WriteLine("Found: " + tableNode[0].Id);
				foreach (HtmlNode row in tableNode[0].SelectNodes("//tr[@class='bg_white']"))
				{
					try
					{
						//major.Code = row.SelectNodes("td")[1].InnerText;
						//major.Name = row.SelectNodes("td")[2].InnerText;
						//major.GroupCode = row.SelectNodes("td")[3].InnerText;
						//major.Score = double.Parse(row.SelectNodes("td")[4].InnerText);

						MajorCollege majorCollege = new MajorCollege();

						string majorCode = row.SelectNodes("td")[1].InnerText;

						if (majorCode == "")
						{
							continue;
						}

						CollegeEntity collegeEntity = null;
						MajorEntity majorEntity = null;
						try
						{
							collegeEntity = db.collegeEntities.DefaultIfEmpty().Single(c => c.code.Equals(collegeCode));
							majorEntity = db.majorEntities.DefaultIfEmpty().Single(c => c.code.Equals(majorCode));
							if (collegeEntity == null || majorEntity == null)
							{
								continue;
							}
							majorCollege.MajorEntity = majorEntity;
							majorCollege.CollegeEntity = collegeEntity;
							majorCollege.groupCode = row.SelectNodes("td")[3].InnerText;
							majorCollege.year = year;
							majorCollege.score = double.Parse(row.SelectNodes("td")[4].InnerText);

							majorColleges.Push(majorCollege);
						}
						catch (InvalidOperationException e)
						{
							/*Process new major which not be stored database 

								//string newMajorName = row.SelectNodes("td")[2].InnerText;
								//MajorEntity newMajor = new MajorEntity();
								//newMajor.code = majorCode;
								//newMajor.name = newMajorName;

								//majorCollege.MajorEntity = newMajor;
								//majorCollege.CollegeEntity = collegeEntity;
								//majorCollege.groupCode = row.SelectNodes("td")[3].InnerText;
								//majorCollege.year = year;
								//majorCollege.score = double.Parse(row.SelectNodes("td")[4].InnerText);

								//majorColleges.Push(majorCollege);

							*/

						}
						i++;
					}
					catch (FormatException e)
					{
						System.Console.WriteLine(e.Message);
					}
				}
			}catch(NullReferenceException e)
			{
				System.Console.WriteLine(e.Message);
			}
			
		}

		[HttpGet("{year}")]
		public ActionResult<string> Get(int year)
		{
			using (var db = new EntranceScoresContext())
			{
				Parallel.ForEach(collegeEntities, college => Process(college,year,db));

				//Add majorCollege to database
				db.majorColleges.AddRange(majorColleges);

				db.SaveChanges();

				return "Crawling data from " + year + " OK";
			}
		}

		private void Process(CollegeEntity college,int year,EntranceScoresContext db)
		{
			var code = college.code;
			var name = college.name;
			var nameUnSign3 = convertToUnSign3(name.ToLower());
			var kq = nameUnSign3.Replace(" ", "-") + "-" + code.ToUpper() + ".html?y=" + year;
			var urlCrawl = "https://diemthi.tuyensinh247.com/diem-chuan/" + kq;
			ProcessCrawling(urlCrawl, code,year, db);
			Console.WriteLine(year+"-"+name+" done!");
		}

		private void testDateSingle()
		{
			using(var db = new EntranceScoresContext())
			{
				var collegeEntity = db.collegeEntities.DefaultIfEmpty().Single(c => c.code.Equals("ANH"));
				var majorEntity = db.majorEntities.DefaultIfEmpty().Single(c => c.code.Equals("714"));
				var majorCollege = new MajorCollege();
				majorCollege.CollegeEntity = collegeEntity;
				majorCollege.MajorEntity = majorEntity;
				majorCollege.score = 17.5;
				majorCollege.groupCode = "sdsd";
				majorCollege.year = 2019;
				db.majorColleges.Add(majorCollege);
				db.SaveChanges();
			}
		}
	}
}