using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable

namespace DAL.Migrations
{
    public partial class posterURLtoByte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add a new temporary column to store the converted data
            migrationBuilder.AddColumn<byte[]>(
                name: "PosterUrlTemp",
                table: "Movies",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            // Step 2: Convert the data from the old column to the new column
            migrationBuilder.Sql("UPDATE Movies SET PosterUrlTemp = CONVERT(varbinary(max), PosterUrl)");

            // Step 3: Drop the old column
            migrationBuilder.DropColumn(
                name: "PosterUrl",
                table: "Movies");

            // Step 4: Rename the temporary column to the original column name
            migrationBuilder.RenameColumn(
                name: "PosterUrlTemp",
                table: "Movies",
                newName: "PosterUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add a new temporary column to store the converted data
            migrationBuilder.AddColumn<string>(
                name: "PosterUrlTemp",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // Step 2: Convert the data from the old column to the new column
            migrationBuilder.Sql("UPDATE Movies SET PosterUrlTemp = CONVERT(nvarchar(max), PosterUrl)");

            // Step 3: Drop the old column
            migrationBuilder.DropColumn(
                name: "PosterUrl",
                table: "Movies");

            // Step 4: Rename the temporary column to the original column name
            migrationBuilder.RenameColumn(
                name: "PosterUrlTemp",
                table: "Movies",
                newName: "PosterUrl");
        }
    }
}