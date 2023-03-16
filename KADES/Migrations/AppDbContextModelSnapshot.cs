﻿// <auto-generated />
using System;
using KADES.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KADES.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("KADES.Models.Account.RFGroup", b =>
                {
                    b.Property<string>("GROUPID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("GROUP_NAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("GROUPID");

                    b.ToTable("RFGroup");
                });

            modelBuilder.Entity("KADES.Models.Account.RFUsers", b =>
                {
                    b.Property<string>("USERID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("EMAIL")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("GROUPID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PASSWORD")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("USERNAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("USERID");

                    b.ToTable("RFUsers");
                });

            modelBuilder.Entity("KADES.Models.Administrasi.AparaturDesa", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("ACTIVE")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ALAMAT")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CREATED_BY")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CREATED_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("JENIS_KELAMIN")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("KODE_JABATAN")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NAMA")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NIK")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NO_TELP")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("SK")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("TGL_BERHENTI")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("TGL_MASUK")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("AparaturDesa");
                });

            modelBuilder.Entity("KADES.Models.Administrasi.BPD", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("ACTIVE")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ALAMAT")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CREATED_BY")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CREATED_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("JENIS_KELAMIN")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("KODE_JABATAN")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NAMA")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NIK")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NO_TELP")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("TGL_PEMBERHENTIAN")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("TGL_PENGANGKATAN")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("BPD");
                });

            modelBuilder.Entity("KADES.Models.Administrasi.KarangTaruna", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("ACTIVE")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ALAMAT")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CREATED_BY")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CREATED_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("JENIS_KELAMIN")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("KODE_JABATAN")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NAMA")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NIK")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NO_TELP")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("TGL_PEMBERHENTIAN")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("TGL_PENGANGKATAN")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("KarangTaruna");
                });

            modelBuilder.Entity("KADES.Models.Administrasi.KegiatanBPD", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DURASI")
                        .HasColumnType("int");

                    b.Property<string>("KEGIATAN")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("KOORDINATOR")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("TGL_BERAKHIR")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("TGL_MULAI")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("KegiatanBPD");
                });

            modelBuilder.Entity("KADES.Models.Administrasi.KegiatanTaruna", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DURASI")
                        .HasColumnType("int");

                    b.Property<string>("KEGIATAN")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("KOORDINATOR")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("TGL_BERAKHIR")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("TGL_MULAI")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("KegiatanTaruna");
                });

            modelBuilder.Entity("KADES.Models.Administrasi.RAB_DESA", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CREATED_BY")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CREATED_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FILENAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("JENIS_RAB")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("KETERANGAN")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PATH_FILE")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("TGL_RAB")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("RAB_Desa");
                });

            modelBuilder.Entity("KADES.Models.Administrasi.RFJabatan", b =>
                {
                    b.Property<string>("KODE_JABATAN")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("ACTIVE")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("JABATAN")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("KODE_TYPE")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("KODE_JABATAN");

                    b.ToTable("RFJabatan");
                });

            modelBuilder.Entity("KADES.Models.Pelayanan.LogSurat", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("GENERATE_BY")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("GENERATE_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ID_SURAT")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("LogSurat");
                });

            modelBuilder.Entity("KADES.Models.Pelayanan.TemplateSurat", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("ACTIVE")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("CREATED_BY")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CREATED_DATE")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FILE_NAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NAMA_SURAT")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NO_SURAT")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PATH_FILE")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UPDATED_BY")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UPDATED_DATE")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("TemplateSurat");
                });
#pragma warning restore 612, 618
        }
    }
}
