using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaqulukaNercerewhelbeba.Migrations
{
    public partial class lindexigithubio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerUrl = table.Column<string>(nullable: true),
                    BlogRss = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PublishedBlogList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Blog = table.Column<string>(nullable: true),
                    MatterMost = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedBlogList", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_BlogRss",
                table: "Blog",
                column: "BlogRss");

            migrationBuilder.CreateIndex(
                name: "IX_PublishedBlogList_Blog_MatterMost",
                table: "PublishedBlogList",
                columns: new[] { "Blog", "MatterMost" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "PublishedBlogList");
        }
    }
}
