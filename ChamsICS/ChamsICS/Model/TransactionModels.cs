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
        public int ClientId { get; set; }
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