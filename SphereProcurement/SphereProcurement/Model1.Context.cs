﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SphereProcurement
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProcurementDBEntities1 : DbContext
    {
        public ProcurementDBEntities1()
            : base("name=ProcurementDBEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<supplier> suppliers { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }
        public virtual DbSet<payment> payments { get; set; }
        public virtual DbSet<purchaseRequest> purchaseRequests { get; set; }
        public virtual DbSet<siteManager> siteManagers { get; set; }
        public virtual DbSet<supplier_items> supplier_items { get; set; }
        public virtual DbSet<inventory> inventories { get; set; }
        public virtual DbSet<order> orders { get; set; }
    }
}
