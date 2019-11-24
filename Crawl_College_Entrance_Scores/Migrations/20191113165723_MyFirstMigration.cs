using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Crawl_College_Entrance_Scores.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Entrance_Scores");

            migrationBuilder.CreateTable(
                name: "majorEntities",
                schema: "Entrance_Scores",
                columns: table => new
                {
                    code = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_majorEntities", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "provinces",
                schema: "Entrance_Scores",
                columns: table => new
                {
                    province_id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provinces", x => x.province_id);
                });

            migrationBuilder.CreateTable(
                name: "collegeEntities",
                schema: "Entrance_Scores",
                columns: table => new
                {
                    code = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    province_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collegeEntities", x => x.code);
                    table.ForeignKey(
                        name: "ForeignKey_Province_CollegeEntity",
                        column: x => x.province_id,
                        principalSchema: "Entrance_Scores",
                        principalTable: "provinces",
                        principalColumn: "province_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "majorColleges",
                schema: "Entrance_Scores",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CollegeEntityId = table.Column<string>(nullable: true),
                    MajorEntityId = table.Column<string>(nullable: true),
                    year = table.Column<int>(nullable: false),
                    groupCode = table.Column<string>(nullable: true),
                    score = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_majorColleges", x => x.id);
                    table.ForeignKey(
                        name: "FK_majorColleges_collegeEntities_CollegeEntityId",
                        column: x => x.CollegeEntityId,
                        principalSchema: "Entrance_Scores",
                        principalTable: "collegeEntities",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_majorColleges_majorEntities_MajorEntityId",
                        column: x => x.MajorEntityId,
                        principalSchema: "Entrance_Scores",
                        principalTable: "majorEntities",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_collegeEntities_province_id",
                schema: "Entrance_Scores",
                table: "collegeEntities",
                column: "province_id");

            migrationBuilder.CreateIndex(
                name: "IX_majorColleges_CollegeEntityId",
                schema: "Entrance_Scores",
                table: "majorColleges",
                column: "CollegeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_majorColleges_MajorEntityId",
                schema: "Entrance_Scores",
                table: "majorColleges",
                column: "MajorEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "majorColleges",
                schema: "Entrance_Scores");

            migrationBuilder.DropTable(
                name: "collegeEntities",
                schema: "Entrance_Scores");

            migrationBuilder.DropTable(
                name: "majorEntities",
                schema: "Entrance_Scores");

            migrationBuilder.DropTable(
                name: "provinces",
                schema: "Entrance_Scores");
        }
    }
}
