using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;
using ResourceAllocation.Domain.Models;

#nullable disable

namespace ResourceAllocation.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:status", "cancelled,confirmed,expired,held")
                .Annotation("Npgsql:PostgresExtension:btree_gist", ",,");

            migrationBuilder.CreateTable(
                name: "resource_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resource_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resources",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resources", x => x.id);
                    table.ForeignKey(
                        name: "fk_resources_resource_types_type_id",
                        column: x => x.type_id,
                        principalTable: "resource_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "allocations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    resource_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period = table.Column<NpgsqlRange<DateTime>>(type: "tsrange", nullable: false),
                    status = table.Column<Status>(type: "status", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_allocations", x => x.id);
                    table.ForeignKey(
                        name: "fk_allocations_resources_resource_id",
                        column: x => x.resource_id,
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_allocations_resource_id",
                table: "allocations",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_resources_type_id",
                table: "resources",
                column: "type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "allocations");

            migrationBuilder.DropTable(
                name: "resources");

            migrationBuilder.DropTable(
                name: "resource_types");
        }
    }
}
