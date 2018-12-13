using Microsoft.EntityFrameworkCore.Migrations;

namespace smartDormitory.Data.Migrations
{
    public partial class Added_ReceiveEmails_To_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReceiveEmails",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiveEmails",
                table: "AspNetUsers");
        }
    }
}
