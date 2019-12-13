using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieRecommender.Migrations
{
    public partial class dbUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Movies_MovieId",
                table: "Rate");

            migrationBuilder.DropForeignKey(
                name: "FK_Rate_User_UserId",
                table: "Rate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rate",
                table: "Rate");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Rate",
                newName: "Rates");

            migrationBuilder.RenameIndex(
                name: "IX_Rate_UserId",
                table: "Rates",
                newName: "IX_Rates_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rate_MovieId",
                table: "Rates",
                newName: "IX_Rates_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rates",
                table: "Rates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Movies_MovieId",
                table: "Rates",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Users_UserId",
                table: "Rates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Movies_MovieId",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Users_UserId",
                table: "Rates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rates",
                table: "Rates");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Rates",
                newName: "Rate");

            migrationBuilder.RenameIndex(
                name: "IX_Rates_UserId",
                table: "Rate",
                newName: "IX_Rate_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rates_MovieId",
                table: "Rate",
                newName: "IX_Rate_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rate",
                table: "Rate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Movies_MovieId",
                table: "Rate",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_User_UserId",
                table: "Rate",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
