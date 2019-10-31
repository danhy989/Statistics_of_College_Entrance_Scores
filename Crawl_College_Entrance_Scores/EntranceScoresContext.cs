using Crawl_College_Entrance_Scores.entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crawl_College_Entrance_Scores
{

	public class EntranceScoresContext : DbContext
	{
		public EntranceScoresContext()
		{
		}

		public EntranceScoresContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<CollegeEntity> collegeEntities { get; set; }
		public DbSet<MajorEntity> majorEntities { get; set; }

		public DbSet<MajorCollege> majorColleges { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			string host = Environment.GetEnvironmentVariable("db_entrance_scores_host");
			string username = Environment.GetEnvironmentVariable("db_entrance_scores_username");
			string password = Environment.GetEnvironmentVariable("db_entrance_scores_password");
			optionsBuilder.UseNpgsql(
				@"Host="+host+";Port=5432;Username="+username+";Password="+password+";Database=uit;");
			optionsBuilder.EnableSensitiveDataLogging();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Configure default schema
			modelBuilder.HasDefaultSchema("Entrance_Scores");
		}
		

	}
}

