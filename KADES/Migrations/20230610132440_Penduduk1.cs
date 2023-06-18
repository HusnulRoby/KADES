using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace KADES.Migrations
{
    public partial class Penduduk1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Penduduk",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NIK = table.Column<string>(type: "longtext", nullable: false),
                    NAMA = table.Column<string>(type: "longtext", nullable: false),
                    KK = table.Column<string>(type: "longtext", nullable: false),
                    POB = table.Column<string>(type: "longtext", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    JENIS_KELAMIN = table.Column<string>(type: "longtext", nullable: false),
                    ID_DLMKELUARGA = table.Column<string>(type: "longtext", nullable: false),
                    ID_AGAMA = table.Column<string>(type: "longtext", nullable: false),
                    NIK_AYAH = table.Column<string>(type: "longtext", nullable: false),
                    NAMA_AYAH = table.Column<string>(type: "longtext", nullable: false),
                    NIK_IBU = table.Column<string>(type: "longtext", nullable: false),
                    NAMA_IBU = table.Column<string>(type: "longtext", nullable: false),
                    ALAMAT = table.Column<string>(type: "longtext", nullable: false),
                    NO_TELP = table.Column<string>(type: "longtext", nullable: false),
                    ID_DUSUN = table.Column<string>(type: "longtext", nullable: false),
                    RW = table.Column<string>(type: "longtext", nullable: false),
                    RT = table.Column<string>(type: "longtext", nullable: false),
                    PENDIDIKAN = table.Column<string>(type: "longtext", nullable: false),
                    PEKERJAAN = table.Column<string>(type: "longtext", nullable: false),
                    KAWIN = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penduduk", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RfAgama",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AGAMA = table.Column<string>(type: "longtext", nullable: false),
                    ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RfAgama", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RfDlmKeluarga",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DLMKELUARGA = table.Column<string>(type: "longtext", nullable: false),
                    ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RfDlmKeluarga", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RfDusun",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DUSUN = table.Column<string>(type: "longtext", nullable: false),
                    ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RfDusun", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Penduduk");

            migrationBuilder.DropTable(
                name: "RfAgama");

            migrationBuilder.DropTable(
                name: "RfDlmKeluarga");

            migrationBuilder.DropTable(
                name: "RfDusun");
        }
    }
}
