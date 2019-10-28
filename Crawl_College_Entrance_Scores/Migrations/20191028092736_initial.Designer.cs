﻿// <auto-generated />
using Crawl_College_Entrance_Scores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Crawl_College_Entrance_Scores.Migrations
{
    [DbContext(typeof(EntranceScoresContext))]
    [Migration("20191028092736_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Entrance_Scores")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Crawl_College_Entrance_Scores.entity.CollegeEntity", b =>
                {
                    b.Property<string>("code")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("name");

                    b.HasKey("code");

                    b.ToTable("collegeEntities");
                });

            modelBuilder.Entity("Crawl_College_Entrance_Scores.entity.MajorCollege", b =>
                {
                    b.Property<string>("MajorEntityId");

                    b.Property<string>("CollegeEntityId");

                    b.Property<int>("year");

                    b.Property<string>("groupCode");

                    b.HasKey("MajorEntityId", "CollegeEntityId", "year");

                    b.HasIndex("CollegeEntityId");

                    b.ToTable("MajorCollege");
                });

            modelBuilder.Entity("Crawl_College_Entrance_Scores.entity.MajorEntity", b =>
                {
                    b.Property<string>("code")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("name");

                    b.HasKey("code");

                    b.ToTable("majorEntities");
                });

            modelBuilder.Entity("Crawl_College_Entrance_Scores.entity.MajorCollege", b =>
                {
                    b.HasOne("Crawl_College_Entrance_Scores.entity.CollegeEntity", "CollegeEntity")
                        .WithMany("MajorColleges")
                        .HasForeignKey("CollegeEntityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Crawl_College_Entrance_Scores.entity.MajorEntity", "MajorEntity")
                        .WithMany("MajorColleges")
                        .HasForeignKey("MajorEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
