//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class TransactionLogBackup_02Aug2017
    {
        public int Id { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> AgentId { get; set; }
        public Nullable<int> TerminalId { get; set; }
        public string Code { get; set; }
        public string ResidentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string RevenueCode { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string PaymentReference { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<System.DateTime> UploadDate { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> LocationId { get; set; }
    }
}
