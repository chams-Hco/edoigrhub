using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService.Model
{

    public class AuthoriseTerminalReq
    {
        [DataMember]
        public string AgentCode { get; set; }

        [DataMember]
        public string TerminalName { get; set; }

        [DataMember]
        public string TerminalSerialNumber { get; set; }
    }

    [DataContract]
    public class AuthoriseTerminalRes: ResponseModel
    {
        [DataMember]
        public string TerminalCode { get; set; }

        [DataMember]
        public string TerminalSerialNumber { get; set; }
    }

    [DataContract]
    public class UploadTransactionReq
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
        public string TransactionDate { get; set; }
        [DataMember]
        public string Amount { get; set; }
        [DataMember]
        public string PaymentReference { get; set; }
    }

    [DataContract]
    public class UploadTransactionRes: ResponseModel
    {
        [DataMember]
        public string TransactionCode { get; set; }
        [DataMember]
        public string ResidentId { get; set; }
        [DataMember]
        public string TempResidentId { get; set; }
    }

    public class Terminal
    {
        public string Id { get; set; }
        public string AgentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
    }

    public class FindTerminalRes : ResponseModel
    {
        public Terminal Terminal { get; set; }
    }

    public class GetAllTerminalRes : ResponseModel
    {
        public IEnumerable<Terminal> Terminals { get; set; }
    }

    public class Transaction
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string AgentId { get; set; }
        public string TerminalId { get; set; }
        public string TerminalCode { get; set; }
        public string TransactionCode { get; set; }
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
        public string Amount { get; set; }
        public string PaymentReference { get; set; }
        public string TransactionDate { get; set; }
        public string UploadDate { get; set; }
        public string status { get; set; }
    }

    public class FindTransactionRes: ResponseModel
    {
        public Transaction Transaction { get; set; }
    }

    public class GetAllTransactionRes: ResponseModel
    {
        public IEnumerable<Transaction> Transactions { get; set; }
    }

}
