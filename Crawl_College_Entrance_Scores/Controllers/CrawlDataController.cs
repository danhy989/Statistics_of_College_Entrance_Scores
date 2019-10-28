using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace Crawl_College_Entrance_Scores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrawlDataController : ControllerBase
    {

		private static string convertToUnSign3(string s)
		{
			Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			string temp = s.Normalize(NormalizationForm.FormD);
			return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
		}

		private void ProcessCrawling(string url)
		{
			string[] test = new string[1000];
			HtmlAgilityPack.HtmlWeb webSite = new HtmlAgilityPack.HtmlWeb();
			HtmlAgilityPack.HtmlDocument document = webSite.Load(url);

			LinkedList<Major> majors = new LinkedList<Major>();

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
						Major major = new Major();
						major.Code = row.SelectNodes("td")[1].InnerText;
						major.Name = row.SelectNodes("td")[2].InnerText;
						major.GroupCode = row.SelectNodes("td")[3].InnerText;
						major.Score = double.Parse(row.SelectNodes("td")[4].InnerText);
						majors.AddLast(major);
						test[i] = major.Name;
						i++;
					}
					catch (FormatException e)
					{
						System.Console.WriteLine(e.Message);
					}
				}

				var prefix = "https://diemthi.tuyensinh247.com/diem-chuan/";
				var suffix = ".html";
				if (url.StartsWith(prefix) && url.EndsWith(suffix) && url.Length >= (prefix.Length + suffix.Length))
				{
					string nameFile = url.Substring(prefix.Length, url.Length - prefix.Length - suffix.Length);
					System.IO.File.WriteAllLines(@"log/" + nameFile + ".txt", test);
				}
				else
				{

				}
			}catch(NullReferenceException e)
			{
				System.Console.WriteLine(e.Message);
			}
			
		}

		[HttpGet("")]
		public ActionResult<string> Get()
		{
			using (var db = new EntranceScoresContext())
			{
				var urls = new List<string>();
				var colleges = db.collegeEntities.ToList();
				Thread[] threads = new Thread[colleges.Count];
				for(int i = 0; i < colleges.Count; i++)
				{
					var college = colleges[i];
					var code = college.code;
					var name = college.name;
					var nameUnSign3 = convertToUnSign3(name.ToLower());
					var kq = nameUnSign3.Replace(" ", "-") + "-" + code.ToUpper() + ".html";
					var urlCrawl = "https://diemthi.tuyensinh247.com/diem-chuan/" + kq;
					threads[i] = new Thread(()=>ProcessCrawling(urlCrawl));
				}

				foreach (Thread thread in threads)
				{
					thread.Start();
				}

				foreach (Thread thread in threads)
				{
					thread.Join();
				}

			}
			return "OK";
		}
	}
}