using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HoneymoonShop.Migrations
{
    public partial class ColorFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dress_Color_ColorID",
                table: "Dress");

            migrationBuilder.DropIndex(
                name: "IX_Dress_ColorID",
                table: "Dress");

            migrationBuilder.DropColumn(
                name: "ColorID",
                table: "Dress");

            migrationBuilder.CreateTable(
                name: "DressColor",
                columns: table => new
                {
                    DressID = table.Column<int>(nullable: false),
                    ColorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DressColor", x => new { x.DressID, x.ColorID });
                    table.ForeignKey(
                        name: "FK_DressColor_Color_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Color",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DressColor_Dress_DressID",
                        column: x => x.DressID,
                        principalTable: "Dress",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DressColor_ColorID",
                table: "DressColor",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_DressColor_DressID",
                table: "DressColor",
                column: "DressID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DressColor");

            migrationBuilder.AddColumn<int>(
                name: "ColorID",
                table: "Dress",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Dress_ColorID",
                table: "Dress",
                column: "ColorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Dress_Color_ColorID",
                table: "Dress",
                column: "ColorID",
                principalTable: "Color",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
