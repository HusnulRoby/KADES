using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace KADES.Migrations
{
    public partial class APARATURDESA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AparaturDesa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    KODE_JABATAN = table.Column<string>(type: "longtext", nullable: false),
                    SK = table.Column<string>(type: "longtext", nullable: false),
                    NAMA = table.Column<string>(type: "longtext", nullable: false),
                    JENIS_KELAMIN = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NIK = table.Column<string>(type: "longtext", nullable: false),
                    NO_TELP = table.Column<string>(type: "longtext", nullable: false),
                    ALAMAT = table.Column<string>(type: "longtext", nullable: false),
                    TGL_MASUK = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TGL_BERHENTI = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "longtext", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AparaturDesa", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPD",
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
                    table.PrimaryKey("PK_BPD", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LogSurat",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ID_SURAT = table.Column<string>(type: "longtext", nullable: false),
                    GENERATE_BY = table.Column<string>(type: "longtext", nullable: false),
                    GENERATE_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSurat", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RAB_Desa",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(255)", nullable: false),
                    JENIS_RAB = table.Column<string>(type: "longtext", nullable: false),
                    TGL_RAB = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FILENAME = table.Column<string>(type: "longtext", nullable: false),
                    PATH_FILE = table.Column<string>(type: "longtext", nullable: false),
                    KETERANGAN = table.Column<string>(type: "longtext", nullable: false),
                    CREATED_BY = table.Column<string>(type: "longtext", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAB_Desa", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RFGroup",
                columns: table => new
                {
                    GROUPID = table.Column<string>(type: "varchar(255)", nullable: false),
                    GROUP_NAME = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFGroup", x => x.GROUPID);
                });

            migrationBuilder.CreateTable(
                name: "RFJabatan",
                columns: table => new
                {
                    KODE_JABATAN = table.Column<string>(type: "varchar(255)", nullable: false),
                    JABATAN = table.Column<string>(type: "longtext", nullable: false),
                    KODE_TYPE = table.Column<string>(type: "longtext", nullable: false),
                    ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFJabatan", x => x.KODE_JABATAN);
                });

            migrationBuilder.CreateTable(
                name: "RFUsers",
                columns: table => new
                {
                    USERID = table.Column<string>(type: "varchar(255)", nullable: false),
                    USERNAME = table.Column<string>(type: "longtext", nullable: false),
                    PASSWORD = table.Column<string>(type: "longtext", nullable: false),
                    GROUPID = table.Column<string>(type: "longtext", nullable: false),
                    EMAIL = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFUsers", x => x.USERID);
                });

            migrationBuilder.CreateTable(
                name: "TemplateSurat",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(255)", nullable: false),
                    NO_SURAT = table.Column<string>(type: "longtext", nullable: false),
                    NAMA_SURAT = table.Column<string>(type: "longtext", nullable: false),
                    FILE_NAME = table.Column<string>(type: "longtext", nullable: false),
                    PATH_FILE = table.Column<string>(type: "longtext", nullable: false),
                    ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "longtext", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UPDATED_BY = table.Column<string>(type: "longtext", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateSurat", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AparaturDesa");

            migrationBuilder.DropTable(
                name: "BPD");

            migrationBuilder.DropTable(
                name: "LogSurat");

            migrationBuilder.DropTable(
                name: "RAB_Desa");

            migrationBuilder.DropTable(
                name: "RFGroup");

            migrationBuilder.DropTable(
                name: "RFJabatan");

            migrationBuilder.DropTable(
                name: "RFUsers");

            migrationBuilder.DropTable(
                name: "TemplateSurat");
        }
    }
}
