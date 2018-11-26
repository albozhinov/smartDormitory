using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartDormitory.Data.Migrations
{
    public partial class ModifiedUserSensors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSensors",
                table: "UserSensors");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserSensors",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSensors",
                table: "UserSensors",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserSensors_UserId",
                table: "UserSensors",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSensors",
                table: "UserSensors");

            migrationBuilder.DropIndex(
                name: "IX_UserSensors_UserId",
                table: "UserSensors");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserSensors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSensors",
                table: "UserSensors",
                columns: new[] { "UserId", "SensorId" });
        }
    }
}
