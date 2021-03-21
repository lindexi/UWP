using Microsoft.EntityFrameworkCore.Migrations;

namespace OTAManager.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LatestApplicationUpdateInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationId = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    UpdateContext = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LatestApplicationUpdateInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LatestApplicationUpdateInfo_ApplicationId",
                table: "LatestApplicationUpdateInfo",
                column: "ApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LatestApplicationUpdateInfo");
        }
    }
}
