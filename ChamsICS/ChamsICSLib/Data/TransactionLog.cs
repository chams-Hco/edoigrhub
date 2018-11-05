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
    
    public partial class TransactionLog
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
        public string LocationCode { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Income { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<decimal> FoodAmount { get; set; }
        public Nullable<decimal> DrinkAmount { get; set; }
        public Nullable<decimal> RentalAmount { get; set; }
        public Nullable<decimal> OtherAmount { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<decimal> MonthlyIncome { get; set; }
        public Nullable<decimal> MonthlyPension { get; set; }
        public Nullable<decimal> MonthlyNHFund { get; set; }
        public Nullable<decimal> MonthlyNHIS { get; set; }
        public string EmployeeName { get; set; }
        public string StaffPayerID { get; set; }
        public Nullable<decimal> AnnualIncome { get; set; }
        public Nullable<decimal> AnnualPension { get; set; }
        public Nullable<decimal> AnnualNHFund { get; set; }
        public Nullable<decimal> AnnualNHIS { get; set; }
        public Nullable<decimal> ConsolidatedReliefs { get; set; }
        public Nullable<decimal> TaxableIncome { get; set; }
        public Nullable<decimal> ComputedAnnualTax { get; set; }
        public Nullable<decimal> MinimumTax { get; set; }
        public Nullable<decimal> AnnualTaxPayable { get; set; }
        public Nullable<decimal> MonthlyTaxLiability { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> NoOfStaff { get; set; }
        public Nullable<decimal> LiabilityPerStaff { get; set; }
        public Nullable<decimal> DevelopmentLevyLiability { get; set; }
        public string WithholdingTaxRevenueName { get; set; }
        public Nullable<decimal> WithholdingTaxRevenueDeductionPercentage { get; set; }
        public Nullable<decimal> WithholdingTaxActualAmount { get; set; }
        public Nullable<decimal> WithholdingTaxLiability { get; set; }
        public string BatchCode { get; set; }   
        public virtual Agent Agent { get; set; }
        public virtual Client Client { get; set; }
        public virtual Terminal Terminal { get; set; }
    }
}
