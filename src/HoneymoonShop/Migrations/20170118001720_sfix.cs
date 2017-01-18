using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HoneymoonShop.Migrations
{
    public partial class sfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suit",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ManuID = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    StyleID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Suit_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Suit_Manu_ManuID",
                        column: x => x.ManuID,
                        principalTable: "Manu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Suit_Style_StyleID",
                        column: x => x.StyleID,
                        principalTable: "Style",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuitColor",
                columns: table => new
                {
                    SuitID = table.Column<int>(nullable: false),
                    ColorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuitColor", x => new { x.SuitID, x.ColorID });
                    table.ForeignKey(
                        name: "FK_SuitColor_Color_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Color",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuitColor_Suit_SuitID",
                        column: x => x.SuitID,
                        principalTable: "Suit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuitFeature",
                columns: table => new
                {
                    SuitID = table.Column<int>(nullable: false),
                    FeatureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuitFeature", x => new { x.SuitID, x.FeatureID });
                    table.ForeignKey(
                        name: "FK_SuitFeature_Feature_FeatureID",
                        column: x => x.FeatureID,
                        principalTable: "Feature",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuitFeature_Suit_SuitID",
                        column: x => x.SuitID,
                        principalTable: "Suit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suit_CategoryID",
                table: "Suit",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Suit_ManuID",
                table: "Suit",
                column: "ManuID");

            migrationBuilder.CreateIndex(
                name: "IX_Suit_StyleID",
                table: "Suit",
                column: "StyleID");

            migrationBuilder.CreateIndex(
                name: "IX_SuitColor_ColorID",
                table: "SuitColor",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_SuitColor_SuitID",
                table: "SuitColor",
                column: "SuitID");

            migrationBuilder.CreateIndex(
                name: "IX_SuitFeature_FeatureID",
                table: "SuitFeature",
                column: "FeatureID");

            migrationBuilder.CreateIndex(
                name: "IX_SuitFeature_SuitID",
                table: "SuitFeature",
                column: "SuitID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuitColor");

            migrationBuilder.DropTable(
                name: "SuitFeature");

            migrationBuilder.DropTable(
                name: "Suit");
        }
    }
}
