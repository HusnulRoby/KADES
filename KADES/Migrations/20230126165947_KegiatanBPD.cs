using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace KADES.Migrations
{
    public partial class KegiatanBPD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KegiatanBPD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    KEGIATAN = table.Column<string>(type: "longtext", nullable: false),
                    KOORDINATOR = table.Column<string>(type: "longtext", nullable: false),
                    DURASI = table.Column<int>(type: "int", nullable: false),
                    TGL_MULAI = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TGL_BERAKHIR = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KegiatanBPD", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KegiatanBPD");
        }
    }
}
