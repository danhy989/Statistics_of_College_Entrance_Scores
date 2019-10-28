using Microsoft.EntityFrameworkCore.Migrations;

namespace Crawl_College_Entrance_Scores.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Entrance_Scores");

            migrationBuilder.CreateTable(
                name: "collegeEntities",
                schema: "Entrance_Scores",
                columns: table => new
                {
                    code = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collegeEntities", x => x.code);
                });

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
                name: "MajorCollege",
                schema: "Entrance_Scores",
                columns: table => new
                {
                    CollegeEntityId = table.Column<string>(nullable: false),
                    MajorEntityId = table.Column<string>(nullable: false),
                    year = table.Column<int>(nullable: false),
                    groupCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MajorCollege", x => new { x.MajorEntityId, x.CollegeEntityId, x.year });
                    table.ForeignKey(
                        name: "FK_MajorCollege_collegeEntities_CollegeEntityId",
                        column: x => x.CollegeEntityId,
                        principalSchema: "Entrance_Scores",
                        principalTable: "collegeEntities",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MajorCollege_majorEntities_MajorEntityId",
                        column: x => x.MajorEntityId,
                        principalSchema: "Entrance_Scores",
                        principalTable: "majorEntities",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MajorCollege_CollegeEntityId",
                schema: "Entrance_Scores",
                table: "MajorCollege",
                column: "CollegeEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MajorCollege",
                schema: "Entrance_Scores");

            migrationBuilder.DropTable(
                name: "collegeEntities",
                schema: "Entrance_Scores");

            migrationBuilder.DropTable(
                name: "majorEntities",
                schema: "Entrance_Scores");
        }
    }
}
