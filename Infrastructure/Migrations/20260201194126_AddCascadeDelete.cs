using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organisations_OrganisationId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganisationId",
                table: "Projects",
                column: "OrganisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Organisations_OrganisationId",
                table: "Projects",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organisations_OrganisationId",
                table: "Users",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Organisations_OrganisationId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organisations_OrganisationId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OrganisationId",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organisations_OrganisationId",
                table: "Users",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id");
        }
    }
}
