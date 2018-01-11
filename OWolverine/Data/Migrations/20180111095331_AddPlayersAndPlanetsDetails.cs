using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OWolverine.Data.Migrations
{
    public partial class AddPlayersAndPlanetsDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planet_Player_OwnerId",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                schema: "og",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Planet",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropIndex(
                name: "IX_Planet_OwnerId",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropColumn(
                name: "ServerId",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "ServerId",
                schema: "og",
                table: "Planet");

            migrationBuilder.AddColumn<bool>(
                name: "Acs",
                schema: "og",
                table: "Universe",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Server",
                schema: "og",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                schema: "og",
                table: "Player",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFlee",
                schema: "og",
                table: "Player",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInactive",
                schema: "og",
                table: "Player",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLeft",
                schema: "og",
                table: "Player",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVocation",
                schema: "og",
                table: "Player",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                schema: "og",
                table: "Planet",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Server",
                schema: "og",
                table: "Planet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                schema: "og",
                table: "Planet",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OwnerServer",
                schema: "og",
                table: "Planet",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                schema: "og",
                table: "Player",
                columns: new[] { "Id", "Server" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Planet",
                schema: "og",
                table: "Planet",
                columns: new[] { "Id", "Server" });

            migrationBuilder.CreateIndex(
                name: "IX_Planet_OwnerId_OwnerServer",
                schema: "og",
                table: "Planet",
                columns: new[] { "OwnerId", "OwnerServer" });

            migrationBuilder.AddForeignKey(
                name: "FK_Planet_Player_OwnerId_OwnerServer",
                schema: "og",
                table: "Planet",
                columns: new[] { "OwnerId", "OwnerServer" },
                principalSchema: "og",
                principalTable: "Player",
                principalColumns: new[] { "Id", "Server" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planet_Player_OwnerId_OwnerServer",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                schema: "og",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Planet",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropIndex(
                name: "IX_Planet_OwnerId_OwnerServer",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropColumn(
                name: "Acs",
                schema: "og",
                table: "Universe");

            migrationBuilder.DropColumn(
                name: "Server",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "IsFlee",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "IsInactive",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "IsLeft",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "IsVocation",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "Server",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropColumn(
                name: "OwnerServer",
                schema: "og",
                table: "Planet");

            migrationBuilder.AddColumn<string>(
                name: "ServerId",
                schema: "og",
                table: "Player",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "og",
                table: "Player",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                schema: "og",
                table: "Planet",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ServerId",
                schema: "og",
                table: "Planet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                schema: "og",
                table: "Player",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Planet",
                schema: "og",
                table: "Planet",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Planet_OwnerId",
                schema: "og",
                table: "Planet",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Planet_Player_OwnerId",
                schema: "og",
                table: "Planet",
                column: "OwnerId",
                principalSchema: "og",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
