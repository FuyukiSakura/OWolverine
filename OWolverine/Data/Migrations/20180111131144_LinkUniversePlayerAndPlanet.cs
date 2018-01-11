using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OWolverine.Data.Migrations
{
    public partial class LinkUniversePlayerAndPlanet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Server",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "ServerAlliance",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "Server",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropColumn(
                name: "OwnerServer",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                schema: "og",
                table: "Planet");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "og",
                table: "Player",
                newName: "PlayerId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "og",
                table: "Player",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "AllianceId",
                schema: "og",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                schema: "og",
                table: "Player",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ServerId",
                schema: "og",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                schema: "og",
                table: "Planet",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServerId",
                schema: "og",
                table: "Planet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                schema: "og",
                table: "Player",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Player_Id_ServerId",
                schema: "og",
                table: "Player",
                columns: new[] { "Id", "ServerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Planet",
                schema: "og",
                table: "Planet",
                columns: new[] { "Id", "ServerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Player_ServerId",
                schema: "og",
                table: "Player",
                column: "ServerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Planet_OwnerId",
                schema: "og",
                table: "Planet",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Planet_ServerId",
                schema: "og",
                table: "Planet",
                column: "ServerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Planet_Player_OwnerId",
                schema: "og",
                table: "Planet",
                column: "OwnerId",
                principalSchema: "og",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Planet_Universe_ServerId",
                schema: "og",
                table: "Planet",
                column: "ServerId",
                principalSchema: "og",
                principalTable: "Universe",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Universe_ServerId",
                schema: "og",
                table: "Player",
                column: "ServerId",
                principalSchema: "og",
                principalTable: "Universe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planet_Player_OwnerId",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropForeignKey(
                name: "FK_Planet_Universe_ServerId",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Universe_ServerId",
                schema: "og",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                schema: "og",
                table: "Player");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Player_Id_ServerId",
                schema: "og",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_ServerId",
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

            migrationBuilder.DropIndex(
                name: "IX_Planet_ServerId",
                schema: "og",
                table: "Planet");

            migrationBuilder.DropColumn(
                name: "AllianceId",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "ServerId",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "ServerId",
                schema: "og",
                table: "Planet");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                schema: "og",
                table: "Player",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "Server",
                schema: "og",
                table: "Player",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ServerAlliance",
                schema: "og",
                table: "Player",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                schema: "og",
                table: "Planet",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Server",
                schema: "og",
                table: "Planet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerServer",
                schema: "og",
                table: "Planet",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                schema: "og",
                table: "Planet",
                nullable: false,
                defaultValue: 0);

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
    }
}
