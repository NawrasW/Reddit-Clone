using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social_MeidaApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class LikeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikeId",
                table: "Likes",
                type: "int",
                nullable: false,
              defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeId",
                table: "Likes");
        }
    }
}
