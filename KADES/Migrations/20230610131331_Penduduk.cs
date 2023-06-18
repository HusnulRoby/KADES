using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace KADES.Migrations
{
    public partial class Penduduk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RFUsers",
                table: "RFUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RFJabatan",
                table: "RFJabatan");

            migrationBuilder.DropColumn(
                name: "DURASI",
                table: "KegiatanTaruna");

            migrationBuilder.DropColumn(
                name: "DURASI",
                table: "KegiatanPKK");

            migrationBuilder.DropColumn(
                name: "DURASI",
                table: "KegiatanBPD");

            migrationBuilder.AlterColumn<string>(
                name: "USERID",
                table: "RFUsers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "RFUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "KODE_JABATAN",
                table: "RFJabatan",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "RFJabatan",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RFUsers",
                table: "RFUsers",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RFJabatan",
                table: "RFJabatan",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RFUsers",
                table: "RFUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RFJabatan",
                table: "RFJabatan");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "RFUsers");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "RFJabatan");

            migrationBuilder.AlterColumn<string>(
                name: "USERID",
                table: "RFUsers",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "KODE_JABATAN",
                table: "RFJabatan",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<int>(
                name: "DURASI",
                table: "KegiatanTaruna",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DURASI",
                table: "KegiatanPKK",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DURASI",
                table: "KegiatanBPD",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RFUsers",
                table: "RFUsers",
                column: "USERID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RFJabatan",
                table: "RFJabatan",
                column: "KODE_JABATAN");
        }
    }
}
