using Crawl_College_Entrance_Scores.entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(
				@"Host=localhost;Port=5432;Username=postgres;Password=123456;Database=uit;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Configure default schema
			modelBuilder.HasDefaultSchema("Entrance_Scores");
			modelBuilder.Entity<MajorCollege>()
			.HasKey(c => new { c.MajorEntityId, c.CollegeEntityId , c.year});
		}
		

	}
}

