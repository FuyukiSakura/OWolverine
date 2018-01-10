using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OWolverine.Data.Migrations
{
    public partial class AddUniverseDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.EnsureSchema(
                name: "og");

            migrationBuilder.CreateTable(
                name: "Universe",
                schema: "og",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DebrisFactor = table.Column<float>(type: "real", nullable: false),
                    DefToDebris = table.Column<float>(type: "real", nullable: false),
                    DeuteriumSaveFactor = table.Column<float>(type: "real", nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonutGalaxy = table.Column<bool>(type: "bit", nullable: false),
                    DonutSystem = table.Column<bool>(type: "bit", nullable: false),
                    FleetSpeed = table.Column<float>(type: "real", nullable: false),
                    Galaxies = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RapidFire = table.Column<bool>(type: "bit", nullable: false),
                    Speed = table.Column<float>(type: "real", nullable: false),
                    Systems = table.Column<int>(type: "int", nullable: false),
                    TopScore = table.Column<int>(type: "int", nullable: false),
                    WreckField = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                schema: "og",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServerAlliance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniverseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Universe_UniverseId",
                        column: x => x.UniverseId,
                        principalSchema: "og",
                        principalTable: "Universe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Planet",
                schema: "og",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Coords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    PlayerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    UniverseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planet_Player_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "og",
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Planet_Universe_UniverseId",
                        column: x => x.UniverseId,
                        principalSchema: "og",
                        principalTable: "Universe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Planet_OwnerId",
                schema: "og",
                table: "Planet",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Planet_UniverseId",
                schema: "og",
                table: "Planet",
                column: "UniverseId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_UniverseId",
                schema: "og",
                table: "Player",
                column: "UniverseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Planet",
                schema: "og");

            migrationBuilder.DropTable(
                name: "Player",
                schema: "og");

            migrationBuilder.DropTable(
                name: "Universe",
                schema: "og");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
