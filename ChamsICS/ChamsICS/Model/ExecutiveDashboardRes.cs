using System;
using System.Collections.Generic;

namespace ChamsICSWebService.Model
{
    public class ExecutiveDashboardRes:Response
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalAgent { get; set; }
        public int TotalClient { get; set; }
        public int TotalNotification { get; set; }
        public int TotalTerminal { get; set; }

        public int TotalTransaction { get; set; }
        public decimal TotalTransctionValue { get; set; }

        public AgentLeaderStats AgentLeaderStats { get; set; }
        public RevenueLeaderStats RevenueLeaderStats { get; set; }
        public IEnumerable<AgentStats> AgentStats { get; set; }
    }

    public class AgentLeaderStats
    {
        public string LeadingAgent { get; set; }
        public string TrailingAgent { get; set; }
        public decimal? LeadingAgentVal { get; set; }
        public decimal? TrailingAgentVal { get; set; }
    }

    public class RevenueLeaderStats
    {
        public string LeadingRevenue { get; set; }
        public string TrailingRevenue { get; set; }
        public decimal? LeadingRevenueVal { get; set; }
        public decimal? TrailingRevenueVal { get; set; }
    }

    public class AgentStats
    {
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentCode { get; set; }

        public AgentTerminalStats TerminalStats { get; set; }
        public IEnumerable<RevenueStats> RevenueStats { get; set; }
        public IEnumerable<RevenueStats> PeriodicRevenueStats { get; set; }
    }

    public class AgentTerminalStats
    {
        public int TotalTerminals { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalTransactionVal { get; set; }
        public int TotalActiveTerminals { get; set; }
        public int TotalInActiveTerminals { get; set; }

        public int TotalPeriodicTransactions { get; set; }
        public decimal TotalPeriodicTransactionVal { get; set; }
        public int TotalPeriodicActiveTerminals { get; set; }
        public int TotalPeriodicInActiveTerminals { get; set; }

        public int Total6MonthsTransactions { get; set; }
        public int Total3MonthsTransactions { get; set; }
        public int Total30DaysTransactions { get; set; }
        public int Total7DaysTransactions { get; set; }
        public int TotalTodayTransactions { get; set; }

        public decimal Total6MonthsTransactionVal { get; set; }
        public decimal Total3MonthsTransactionVal { get; set; }
        public decimal Total30DaysTransactionVal { get; set; }
        public decimal Total7DaysTransactionVal { get; set; }
        public decimal TotalTodayTransactionVal { get; set; }

        public int Total6MonthsActiveTerminals { get; set; }
        public int Total3MonthsActiveTerminals { get; set; }
        public int Total30DaysActiveTerminals { get; set; }
        public int Total7DaysActiveTerminals { get; set; }
        public int TotalTodayActiveTerminals { get; set; }

        public int Total6MonthsInActiveTerminals { get; set; }
        public int Total3MonthsInActiveTerminals { get; set; }
        public int Total30DaysInActiveTerminals { get; set; }
        public int Total7DaysInActiveTerminals { get; set; }
        public int TotalTodayInActiveTerminals { get; set; }
    }

    public class TerminalStats
    {
        public int TerminalId { get; set; }
        public string TerminalCode { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalTransactionVal { get; set; }
        public int TotalPeriodicTransactions { get; set; }
        public decimal TotalPeriodicTransactionVal { get; set; }    

        public int Total6MonthsTransactions { get; set; }
        public int Total3MonthsTransactions { get; set; }
        public int Total30DaysTransactions { get; set; }
        public int Total7DaysTransactions { get; set; }
        public int TotalTodayTransactions { get; set; }

        public decimal Total6MonthsTransactionVal { get; set; }
        public decimal Total3MonthsTransactionVal { get; set; }
        public decimal Total30DaysTransactionVal { get; set; }
        public decimal Total7DaysTransactionVal { get; set; }
        public decimal TotalTodayTransactionVal { get; set; }
    }

    public class RevenueStats
    {
        public string RevenueName { get; set; }
        public int TotalTransactionVol { get; set; }
        public decimal? TotalTransactionVal { get; set; }
    }
}