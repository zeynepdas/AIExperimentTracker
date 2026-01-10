using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Net9RestApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AIProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIProjects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experiments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    ModelName = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    AIProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiments_AIProjects_AIProjectId",
                        column: x => x.AIProjectId,
                        principalTable: "AIProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    ExperimentId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metrics_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsDeleted", "PasswordHash", "Role", "UpdatedAt", "Username" },
                values: new object[] { 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@test.com", false, "admin123", "Admin", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin" });

            migrationBuilder.InsertData(
                table: "AIProjects",
                columns: new[] { "Id", "CreatedAt", "Description", "IsDeleted", "Name", "UpdatedAt", "UserId" },
                values: new object[] { 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Initial seeded AI project", false, "Seed Project", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Experiments",
                columns: new[] { "Id", "AIProjectId", "CreatedAt", "IsDeleted", "ModelName", "Name", "Notes", "Status", "UpdatedAt" },
                values: new object[] { 1, 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "ResNet50", "Baseline Experiment", null, "Completed", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Metrics",
                columns: new[] { "Id", "CreatedAt", "ExperimentId", "IsDeleted", "Name", "UpdatedAt", "Value" },
                values: new object[] { 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, "Accuracy", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.92000000000000004 });

            migrationBuilder.CreateIndex(
                name: "IX_AIProjects_UserId",
                table: "AIProjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_AIProjectId",
                table: "Experiments",
                column: "AIProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_ExperimentId",
                table: "Metrics",
                column: "ExperimentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metrics");

            migrationBuilder.DropTable(
                name: "Experiments");

            migrationBuilder.DropTable(
                name: "AIProjects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
