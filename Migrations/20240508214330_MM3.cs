using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace CarWorkshop.Migrations
{
    /// <inheritdoc />
    public partial class MM3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Tasks");

            migrationBuilder.AlterColumn<bool>(
                name: "Estimation_IsApproved",
                table: "Tickets",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<NpgsqlRange<DateTime>>(
                name: "WorkTime",
                table: "Tasks",
                type: "tstzrange",
                nullable: false,
                defaultValue: new NpgsqlTypes.NpgsqlRange<System.DateTime>(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkTime",
                table: "Tasks");

            migrationBuilder.AlterColumn<bool>(
                name: "Estimation_IsApproved",
                table: "Tickets",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<double>(
                name: "Hours",
                table: "Tasks",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
