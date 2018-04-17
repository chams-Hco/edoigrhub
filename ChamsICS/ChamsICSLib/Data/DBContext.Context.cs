﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChamsICSLib.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CICSEntities : DbContext
    {
        public CICSEntities()
            : base("name=CICSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<AgentManager> AgentManagers { get; set; }
        public virtual DbSet<AuditTrail> AuditTrails { get; set; }
        public virtual DbSet<IdentityService> IdentityServices { get; set; }
        public virtual DbSet<Ministry> Ministries { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<Revenue> Revenues { get; set; }
        public virtual DbSet<RevenueCategory> RevenueCategories { get; set; }
        public virtual DbSet<RevenueHead> RevenueHeads { get; set; }
        public virtual DbSet<RevenueItem> RevenueItems { get; set; }
        public virtual DbSet<Terminal> Terminals { get; set; }
        public virtual DbSet<TerminalLocation> TerminalLocations { get; set; }
        public virtual DbSet<TerminalUpdateLog> TerminalUpdateLogs { get; set; }
        public virtual DbSet<TransactionLogException> TransactionLogExceptions { get; set; }
        public virtual DbSet<TransactionStatu> TransactionStatus { get; set; }
        public virtual DbSet<TransactionUploadError> TransactionUploadErrors { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAgent> UserAgents { get; set; }
        public virtual DbSet<EOD> EODs { get; set; }
        public virtual DbSet<EODPaymentNotificationLog> EODPaymentNotificationLogs { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserClient> UserClients { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }
        public virtual DbSet<TransactionLog> TransactionLogs { get; set; }
        public virtual DbSet<ClientSetting> ClientSettings { get; set; }
    }
}
