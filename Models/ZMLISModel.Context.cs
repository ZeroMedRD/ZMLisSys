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
    
    public partial class ZMLISEntities : DbContext
    {
        public ZMLISEntities()
            : base("name=ZMLISEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<LISQueue> LISQueue { get; set; }
        public virtual DbSet<LISSchedule> LISSchedule { get; set; }
        public virtual DbSet<lisLaboratoryClass> lisLaboratoryClass { get; set; }
        public virtual DbSet<lisLaboratoryItem> lisLaboratoryItem { get; set; }
        public virtual DbSet<lisLaboratoryMaster> lisLaboratoryMaster { get; set; }
        public virtual DbSet<lisDataMapping> lisDataMapping { get; set; }
        public virtual DbSet<lisLaboratoryDetail> lisLaboratoryDetail { get; set; }
    }
}