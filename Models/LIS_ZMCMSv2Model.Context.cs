﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZMLISSys.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LIS_ZMCMSv2Entities : DbContext
    {
        public LIS_ZMCMSv2Entities()
            : base("name=LIS_ZMCMSv2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<ComboDetail> ComboDetail { get; set; }
        public virtual DbSet<ComboMaster> ComboMaster { get; set; }
        public virtual DbSet<SysHospital> SysHospital { get; set; }
        public virtual DbSet<SysMedicalGroup> SysMedicalGroup { get; set; }
        public virtual DbSet<SysMedicalGroupHospital> SysMedicalGroupHospital { get; set; }
        public virtual DbSet<SysRole> SysRole { get; set; }
        public virtual DbSet<SysRoleProgram> SysRoleProgram { get; set; }
        public virtual DbSet<SysUploadServer> SysUploadServer { get; set; }
    }
}