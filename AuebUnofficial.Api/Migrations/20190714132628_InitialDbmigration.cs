using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuebUnofficial.Api.Migrations
{
    public partial class InitialDbmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RSSAnouncements",
                columns: table => new
                {
                    Kind = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: false),
                    LastUpdatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSSAnouncements", x => x.Kind);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "RSSAnouncements");
        }
    }
}
