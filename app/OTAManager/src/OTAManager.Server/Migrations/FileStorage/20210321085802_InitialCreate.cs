using Microsoft.EntityFrameworkCore.Migrations;

namespace OTAManager.Server.Migrations.FileStorage
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileStorageData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileKey = table.Column<string>(type: "TEXT", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false),
                    Md5 = table.Column<string>(type: "TEXT", nullable: false),
                    FileLength = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStorageData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileStorageData_FileKey",
                table: "FileStorageData",
                column: "FileKey");

            migrationBuilder.CreateIndex(
                name: "IX_FileStorageData_Md5",
                table: "FileStorageData",
                column: "Md5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileStorageData");
        }
    }
}
