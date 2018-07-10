using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class WebPayment
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string DatePeriod { get; set; }
        public int UserId { get; set; }
        public int TerminalId { get; set; }
        public decimal? FoodAmount { get; set; } = 0;
        public decimal? DrinkAmount { get; set; } = 0;
        public decimal? RentalAmount { get; set; } = 0;
        public decimal? OtherAmount { get; set; } = 0;
        public decimal? GrossIncome { get; set; } = 0;
        public decimal? PercentageDeduction { get; set; } = 0;
        public decimal Amount { get; set; } = 0;
        public int RevenueItemId { get; set; }
        //this section is for payeeAOC
        public Month? Month { get; set; }
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
        public decimal? ComputedAnnualTax{ get; set; } = 0;
        public decimal? MinimumTax{ get; set; } = 0;
        public decimal? AnnualTaxPayable{ get; set; } = 0;
        public decimal? MonthlyTaxLiability{ get; set; } = 0;
        //this section is for developmentlevyliabilty
        public int? Year { get; set; } = DateTime.Now.Year;
        public int? NoOfStaff { get; set; } = 0;
        public decimal? LiabilityPerStaff { get; set; } = 100;
        public decimal? DevelopmentLevyLiability { get; set; } = 0;
        //this section is for Withholding Tax
        public string WithholdingTaxRevenueName{ get; set; } 
        public decimal? WithholdingTaxRevenueDeductionPercentage { get; set; } = 0;
        public decimal? WithholdingTaxActualAmount { get; set; } = 0;
        public decimal? WithholdingTaxLiability { get; set; } = 0;

        //withholdingTaxCalculators and rates
        public decimal? ContractAndSupplyRate { get; set; } = 5;
        public decimal? ConsultancyOrProffesionalFeeRate { get; set; } = 5;
        public decimal? RentRate { get; set; } = 10;
        public decimal? DividendRate { get; set; } = 10;
        public decimal? DirectorFeeRate{ get; set; } = 10;
        public decimal? ServicesRate { get; set; } = 5;
        public decimal? CommissionRate { get; set; } = 5;
        //this section is for anambraresidency
        public string Name { get; set; }
        public string ResidentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string RevenueCode { get; set; }
    }

    public enum Month
    {
        January,
        February,
        March,April, May,June,july,August,September,October,November,December
    }
    

}