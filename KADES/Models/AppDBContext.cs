﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using KADES.Models.Account;
using KADES.Models.Administrasi;
using KADES.Models.Pelayanan;
using Microsoft.EntityFrameworkCore;

namespace KADES.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<LogSurat>? LogSurat { get; set; }
        public virtual DbSet<TemplateSurat>? TemplateSurat { get; set; }
        public virtual DbSet<RFGroup> RFGroup { get; set; }
        public virtual DbSet<RFUsers> RFUsers { get; set; }
        public virtual DbSet<AparaturDesa>? AparaturDesa { get; set; }
        public virtual DbSet<RFJabatan>? RFJabatan { get; set; }
        public virtual DbSet<RAB_DESA>? RAB_Desa { get; set; }
        public virtual DbSet<BPD>? BPD { get; set; }
        public virtual DbSet<KegiatanBPD>? KegiatanBPD { get; set; }
        public virtual DbSet<KarangTaruna>? KarangTaruna { get; set; }
        public virtual DbSet<KegiatanTaruna>? KegiatanTaruna { get; set; }


    }
}
