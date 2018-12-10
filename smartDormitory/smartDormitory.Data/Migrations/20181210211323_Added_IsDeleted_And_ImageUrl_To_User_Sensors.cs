using Microsoft.EntityFrameworkCore.Migrations;

namespace smartDormitory.Data.Migrations
{
    public partial class Added_IsDeleted_And_ImageUrl_To_User_Sensors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "UserSensors",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserSensors",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "UserSensors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserSensors");
        }
    }
}
