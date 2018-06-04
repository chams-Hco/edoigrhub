using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChamsICSWebService.Model
{

    public class Transaction
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public string LocationName { get; set; }
        public int TerminalId { get; set; }
        public string TerminalCode { get; set; }
        public string RevenueCode { get; set; }
        public string RevenueName { get; set; }
        public string Ministry { get; set; }
        public string RevenueHead { get; set; }
        public string TransactionCode { get; set; }
        public string ResidentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public Decimal Amount { get; set; }
        public string PaymentReference { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime UploadDate { get; set; }
        public int status { get; set; }
        public int? LocationId { get; internal set; }
        public decimal? Income { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? FoodAmount { get; set; }
        public decimal? DrinkAmount { get; set; }
        public decimal? RentalAmount { get; set; }
        public decimal? OtherAmount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Name { get; set; }
        public int Month { get; set; }
        public decimal? MonthlyIncome { get; set; } = 0;
        public decimal? MonthlyPension { get; set; } = 0;
        public decimal? MonthlyNHFund { get; set; } = 0;
        public decimal? MonthlyNHIS { get; set; } = 0;
        public string EmployeeName { get; set; }
        public string StaffPayerID { get; set; }
        public decimal? AnnualIncome { get; set; } = 0;
        public decimal? AnnualPension { get; set; } = 0;
        public decimal? AnnualNHFund { get; set; } = 0;
        public decimal? AnnualNHIS { get; set; } = 0;
        public decimal? ConsolidatedReliefs { get; set; } = 0;
        public decimal? TaxableIncome { get; set; } = 0;
        public decimal? ComputedAnnualTax { get; set; } = 0;
        public decimal? MinimumTax { get; set; } = 0;
        public decimal? AnnualTaxPayable { get; set; } = 0;
        public decimal? MonthlyTaxLiability { get; set; } = 0;
        //this section is for developmentlevyliabilty
        public int? Year { get; set; } = DateTime.Now.Year;
        public int? NoOfStaff { get; set; } = 0;
        public decimal? LiabilityPerStaff { get; set; } = 0;
        public decimal? DevelopmentLevyLiability { get; set; } = 0;
        //this section is for Withholding Tax
        public string WithholdingTaxRevenueName { get; set; }
        public decimal? WithholdingTaxRevenueDeductionPercentage { get; set; } = 0;
        public decimal? WithholdingTaxActualAmount { get; set; } = 0;
        public decimal? WithholdingTaxLiability { get; set; } = 0;
    }

    public class FindTransactionRes : Response
    {
        public Transaction Transaction { get; set; }
    }

    public class GetAllTransactionRes : Response
    {
        public IEnumerable<Transaction> Transactions { get; set; }
    }

    [DataContract]
    public class UploadTransactionReq
    {
        [DataMember]
        public string TerminalCode { get; set; }
        [DataMember]
        public string TransactionCode { get; set; }
        [DataMember]
        public string LocationCode { get; set; }
        [DataMember]
        public string ResidentId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string DateOfBirth { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string RevenueCode { get; set; }
        [DataMember]
        public string TransactionDate { get; set; }
        [DataMember]
        public string Amount { get; set; }
        [DataMember]
        public string PaymentReference { get; set; }
    }

    [DataContract]
    public class UploadTransactionRes : Response
    {
        [DataMember]
        public string TransactionCode { get; set; }
        [DataMember]
        public string ResidentId { get; set; }
        [DataMember]
        public string TempResidentId { get; set; }
    }

    [DataContract]
    public class WebTransactionReq
    {
        [DataMember]
        public int TerminalId { get; set; }
        [DataMember]
        public int RevenueItemId { get; set; }
        [DataMember]
        public DateTime FromDate { get; set; }
        [DataMember]
        public DateTime ToDate { get; set; }
        [DataMember]
        public decimal PercentageDeduction { get; set; } = 0;
        [DataMember]
        public decimal Income { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal FoodAmount { get; set; }
        [DataMember]
        public decimal DrinkAmount { get; set; }
        [DataMember]
        public decimal RentalAmount { get; set; }
        [DataMember]
        public decimal OtherAmount { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ResidentId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string DateOfBirth { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string RevenueCode { get; set; }
        [DataMember]
        public int Month { get; set; }
        [DataMember]
        public decimal? MonthlyIncome { get; set; } = 0;
        [DataMember]
        public decimal? MonthlyPension { get; set; } = 0;
        [DataMember]
        public decimal? MonthlyNHFund { get; set; } = 0;
        [DataMember]
        public decimal? MonthlyNHIS { get; set; } = 0;
        [DataMember]
        public string EmployeeName { get; set; }
        [DataMember]
        public string StaffPayerID { get; set; }
        [DataMember]
        public decimal? AnnualIncome { get; set; } = 0;
        [DataMember]
        public decimal? AnnualPension { get; set; } = 0;
        [DataMember]
        public decimal? AnnualNHFund { get; set; } = 0;
        [DataMember]
        public decimal? AnnualNHIS { get; set; } = 0;
        [DataMember]
        public decimal? ConsolidatedReliefs { get; set; } = 0;
        [DataMember]
        public decimal? TaxableIncome { get; set; } = 0;
        [DataMember]
        public decimal? ComputedAnnualTax { get; set; } = 0;
        [DataMember]
        public decimal? MinimumTax { get; set; } = 0;
        [DataMember]
        public decimal? AnnualTaxPayable { get; set; } = 0;
        [DataMember]
        public decimal? MonthlyTaxLiability { get; set; } = 0;
        //this section is for developmentlevyliabilty
        [DataMember]
        public int? Year { get; set; } = DateTime.Now.Year;
        [DataMember]
        public int? NoOfStaff { get; set; } = 0;
        [DataMember]
        public decimal? LiabilityPerStaff { get; set; } = 0;
        [DataMember]
        public decimal? DevelopmentLevyLiability { get; set; } = 0;
        [DataMember]
        //this section is for Withholding Tax
        public string WithholdingTaxRevenueName { get; set; }
        [DataMember]
        public decimal? WithholdingTaxRevenueDeductionPercentage { get; set; } = 0;
        [DataMember]
        public decimal? WithholdingTaxActualAmount { get; set; } = 0;
        [DataMember]
        public decimal? WithholdingTaxLiability { get; set; } = 0;

    }

    [DataContract]
    public class WebTransactionRes : Response
    {
        [DataMember]
        public string RemittanceCode { get; set; }
        [DataMember]
        public string TransactionCode { get; set; }
        [DataMember]
        public string TerminalCode { get; set; }
    }



   

    public class QueryTransactionReq
    {
        [DataMember]
        public string TerminalCode { get; set; }
        [DataMember]
        public string TransactionCode { get; set; }
    }

    public class QueryTransactionRes : Response
    {
        [DataMember]
        public string TerminalCode { get; set; }
        [DataMember]
        public string TransactionCode { get; set; }
        [DataMember]
        public string ResidentId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string DateOfBirth { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string RevenueCode { get; set; }
        [DataMember]
        public string Amount { get; set; }
        [DataMember]
        public string PaymentReference { get; set; }
        [DataMember]
        public string TransactionDate { get; set; }
        [DataMember]
        public string UploadDate { get; set; }
    }

}