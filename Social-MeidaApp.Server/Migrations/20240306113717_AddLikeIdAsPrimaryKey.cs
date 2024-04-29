using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social_MeidaApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddLikeIdAsPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "LikeId",
                table: "Likes");

            migrationBuilder.AddColumn<int>(
                name: "LikeId",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                column: "LikeId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_LikeId",
                table: "Likes",
                column: "LikeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_LikeId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "LikeId",
                table: "Likes");

            migrationBuilder.AddColumn<int>(
                name: "LikeId",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                columns: new[] { "UserId", "CommentId", "PostId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
