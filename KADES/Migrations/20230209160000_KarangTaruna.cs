using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace KADES.Migrations
{
    public partial class KarangTaruna : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KarangTaruna",
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
                    table.PrimaryKey("PK_KarangTaruna", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KarangTaruna");
        }
    }
}
