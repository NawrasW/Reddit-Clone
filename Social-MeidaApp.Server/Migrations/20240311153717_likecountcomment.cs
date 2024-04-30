using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social_MeidaApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class likecountcomment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Comments");
        }
    }
}
