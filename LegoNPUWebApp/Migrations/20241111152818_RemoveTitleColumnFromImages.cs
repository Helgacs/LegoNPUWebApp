﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegoNPUWebApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTitleColumnFromImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}