using Microsoft.EntityFrameworkCore.Migrations;

namespace smartDormitory.Data.Migrations
{
    public partial class Added_Description_To_UserSensors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserSensors",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserSensors");
        }
    }
}
