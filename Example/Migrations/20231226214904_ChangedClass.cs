using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example.Migrations
{
    /// <inheritdoc />
    public partial class ChangedClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Cities",
                newName: "UpdateDateOffset");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Cities",
                newName: "StartDateOffset");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Cities",
                newName: "EndDateOffset");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateDateOffset",
                table: "Cities",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "StartDateOffset",
                table: "Cities",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndDateOffset",
                table: "Cities",
                newName: "EndDate");
        }
    }
}
