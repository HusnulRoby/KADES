using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace KADES.Migrations
{
    public partial class KegiatanPKK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KegiatanPKK",
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
                    table.PrimaryKey("PK_KegiatanPKK", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PKK",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    KODE_JABATAN = table.Column<string>(type: "longtext", nullable: false),
                    NAMA = table.Column<string>(type: "longtext", nullable: false),
                    JENIS_KELAMIN = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NIK = table.Column<string>(type: "longtext", nullable: false),
                    NO_TELP = table.Column<string>(type: "longtext", nullable: false),
                    ALAMAT = table.Column<string>(type: "longtext", nullable: false),
                    TGL_PENGANGKATAN = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TGL_PEMBERHENTIAN = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "longtext", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PKK", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KegiatanPKK");

            migrationBuilder.DropTable(
                name: "PKK");
        }
    }
}
