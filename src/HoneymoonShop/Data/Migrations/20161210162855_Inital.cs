using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HoneymoonShop.Data.Migrations
{
    public partial class Inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Feature",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Manu",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manu", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Neckline",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Neckline", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Silhouette",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Silhouette", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Style",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Style", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Dress",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ColorID = table.Column<int>(nullable: false),
                    ManuID = table.Column<int>(nullable: false),
                    NecklineID = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    SilhouetteID = table.Column<int>(nullable: false),
                    StyleID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dress", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dress_Color_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Color",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dress_Manu_ManuID",
                        column: x => x.ManuID,
                        principalTable: "Manu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dress_Neckline_NecklineID",
                        column: x => x.NecklineID,
                        principalTable: "Neckline",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dress_Silhouette_SilhouetteID",
                        column: x => x.SilhouetteID,
                        principalTable: "Silhouette",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dress_Style_StyleID",
                        column: x => x.StyleID,
                        principalTable: "Style",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DressFeature",
                columns: table => new
                {
                    DressID = table.Column<int>(nullable: false),
                    FeatureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DressFeature", x => new { x.DressID, x.FeatureID });
                    table.ForeignKey(
                        name: "FK_DressFeature_Dress_DressID",
                        column: x => x.DressID,
                        principalTable: "Dress",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DressFeature_Feature_FeatureID",
                        column: x => x.FeatureID,
                        principalTable: "Feature",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dress_ColorID",
                table: "Dress",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_Dress_ManuID",
                table: "Dress",
                column: "ManuID");

            migrationBuilder.CreateIndex(
                name: "IX_Dress_NecklineID",
                table: "Dress",
                column: "NecklineID");

            migrationBuilder.CreateIndex(
                name: "IX_Dress_SilhouetteID",
                table: "Dress",
                column: "SilhouetteID");

            migrationBuilder.CreateIndex(
                name: "IX_Dress_StyleID",
                table: "Dress",
                column: "StyleID");

            migrationBuilder.CreateIndex(
                name: "IX_DressFeature_DressID",
                table: "DressFeature",
                column: "DressID");

            migrationBuilder.CreateIndex(
                name: "IX_DressFeature_FeatureID",
                table: "DressFeature",
                column: "FeatureID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DressFeature");

            migrationBuilder.DropTable(
                name: "Dress");

            migrationBuilder.DropTable(
                name: "Feature");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Manu");

            migrationBuilder.DropTable(
                name: "Neckline");

            migrationBuilder.DropTable(
                name: "Silhouette");

            migrationBuilder.DropTable(
                name: "Style");
        }
    }
}
