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
    
    public partial class LIS_HISEntities : DbContext
    {
        public LIS_HISEntities()
            : base("name=LIS_HISEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<lisHospitalLaboratoryItem> lisHospitalLaboratoryItem { get; set; }
        public virtual DbSet<lisPatientLaboratoryDetail> lisPatientLaboratoryDetail { get; set; }
        public virtual DbSet<lisPatientLaboratoryMaster> lisPatientLaboratoryMaster { get; set; }
    }
}