using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OWolverine.Data.Migrations
{
    public partial class CreateStarMapSchema : Migration
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
                name: "Moon",
                schema: "og",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moon", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Universe",
                schema: "og",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Acs = table.Column<bool>(type: "bit", nullable: false),
                    AllianceLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DebrisFactor = table.Column<float>(type: "real", nullable: false),
                    DefToDebris = table.Column<float>(type: "real", nullable: false),
                    DeuteriumSaveFactor = table.Column<float>(type: "real", nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonutGalaxy = table.Column<bool>(type: "bit", nullable: false),
                    DonutSystem = table.Column<bool>(type: "bit", nullable: false),
                    FleetSpeed = table.Column<float>(type: "real", nullable: false),
                    Galaxies = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanetsLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlayersLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AllianceId = table.Column<int>(type: "int", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    IsFlee = table.Column<bool>(type: "bit", nullable: false),
                    IsInactive = table.Column<bool>(type: "bit", nullable: false),
                    IsLeft = table.Column<bool>(type: "bit", nullable: false),
                    IsVocation = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    ServerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.UniqueConstraint("AK_Player_PlayerId_ServerId", x => new { x.PlayerId, x.ServerId });
                    table.ForeignKey(
                        name: "FK_Player_Universe_ServerId",
                        column: x => x.ServerId,
                        principalSchema: "og",
                        principalTable: "Universe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alliance",
                schema: "og",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AllianceId = table.Column<int>(type: "int", nullable: false),
                    FounderId = table.Column<int>(type: "int", nullable: true),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alliance", x => x.Id);
                    table.UniqueConstraint("AK_Alliance_AllianceId_ServerId", x => new { x.AllianceId, x.ServerId });
                    table.ForeignKey(
                        name: "FK_Alliance_Player_FounderId",
                        column: x => x.FounderId,
                        principalSchema: "og",
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Alliance_Universe_ServerId",
                        column: x => x.ServerId,
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Coord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoonId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    PlanetId = table.Column<int>(type: "int", nullable: false),
                    ServerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planet", x => x.Id);
                    table.UniqueConstraint("AK_Planet_PlanetId_ServerId", x => new { x.PlanetId, x.ServerId });
                    table.ForeignKey(
                        name: "FK_Planet_Moon_MoonId",
                        column: x => x.MoonId,
                        principalSchema: "og",
                        principalTable: "Moon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Planet_Player_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "og",
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Planet_Universe_ServerId",
                        column: x => x.ServerId,
                        principalSchema: "og",
                        principalTable: "Universe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
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
                name: "IX_Alliance_FounderId",
                schema: "og",
                table: "Alliance",
                column: "FounderId",
                unique: true,
                filter: "[FounderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Alliance_ServerId",
                schema: "og",
                table: "Alliance",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_Planet_MoonId",
                schema: "og",
                table: "Planet",
                column: "MoonId");

            migrationBuilder.CreateIndex(
                name: "IX_Planet_OwnerId",
                schema: "og",
                table: "Planet",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Planet_ServerId",
                schema: "og",
                table: "Planet",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_AllianceId",
                schema: "og",
                table: "Player",
                column: "AllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_ServerId",
                schema: "og",
                table: "Player",
                column: "ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Alliance_AllianceId",
                schema: "og",
                table: "Player",
                column: "AllianceId",
                principalSchema: "og",
                principalTable: "Alliance",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Alliance_Player_FounderId",
                schema: "og",
                table: "Alliance");

            migrationBuilder.DropTable(
                name: "Planet",
                schema: "og");

            migrationBuilder.DropTable(
                name: "Moon",
                schema: "og");

            migrationBuilder.DropTable(
                name: "Player",
                schema: "og");

            migrationBuilder.DropTable(
                name: "Alliance",
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
