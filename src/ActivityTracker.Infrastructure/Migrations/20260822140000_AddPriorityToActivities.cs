using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityToActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Activities",
                type: "text",
                nullable: false,
                defaultValue: "Medium");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Activities");
        }
    }
}
