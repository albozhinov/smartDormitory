using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartDormitory.Data.Migrations
{
    public partial class Added_Sensors_ModifiedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "URL",
                table: "Sensors",
                newName: "Url");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Sensors",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "Sensors",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Guid",
                table: "Sensors",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Sensors",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Sensors",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Sensors");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Sensors",
                newName: "URL");

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "Sensors",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "Sensors",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Guid",
                table: "Sensors",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Sensors",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
