using Microsoft.EntityFrameworkCore.Migrations;

namespace smartDormitory.Data.Migrations
{
    public partial class Added_Sensor_ModifiedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Sensors",
                newName: "IcbSensorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IcbSensorId",
                table: "Sensors",
                newName: "Guid");
        }
    }
}
