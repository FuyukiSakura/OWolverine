using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OWolverine.Data.Migrations
{
    public partial class AddScoreInfoSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "og",
                table: "Player",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ScoreId",
                schema: "og",
                table: "Player",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HighScore",
                schema: "og",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Economy = table.Column<int>(type: "int", nullable: false),
                    Honor = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Military = table.Column<int>(type: "int", nullable: false),
                    MilitaryBuit = table.Column<int>(type: "int", nullable: false),
                    MilitaryDestroyed = table.Column<int>(type: "int", nullable: false),
                    MilitaryLost = table.Column<int>(type: "int", nullable: false),
                    Research = table.Column<int>(type: "int", nullable: false),
                    Ship = table.Column<int>(type: "int", nullable: false),
                    ShipNumber = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HighScore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScoreHistory",
                schema: "og",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NewValue = table.Column<int>(type: "int", nullable: false),
                    OldValue = table.Column<int>(type: "int", nullable: false),
                    ScoreId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreHistory_HighScore_ScoreId",
                        column: x => x.ScoreId,
                        principalSchema: "og",
                        principalTable: "HighScore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_ScoreId",
                schema: "og",
                table: "Player",
                column: "ScoreId",
                unique: true,
                filter: "[ScoreId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreHistory_ScoreId",
                schema: "og",
                table: "ScoreHistory",
                column: "ScoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_HighScore_ScoreId",
                schema: "og",
                table: "Player",
                column: "ScoreId",
                principalSchema: "og",
                principalTable: "HighScore",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_HighScore_ScoreId",
                schema: "og",
                table: "Player");

            migrationBuilder.DropTable(
                name: "ScoreHistory",
                schema: "og");

            migrationBuilder.DropTable(
                name: "HighScore",
                schema: "og");

            migrationBuilder.DropIndex(
                name: "IX_Player_ScoreId",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "og",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "ScoreId",
                schema: "og",
                table: "Player");
        }
    }
}
