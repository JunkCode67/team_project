using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamRolesAndFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Submitted",
                table: "Submissions",
                newName: "SubmittedAt");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "TeamMembers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "TeamMembers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Rounds",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Users_UserId",
                table: "TeamMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Users_UserId",
                table: "TeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TeamMembers");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "Submissions",
                newName: "Submitted");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Rounds",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);
        }
    }
}
