using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ChamsICSLib.Data;

namespace ChamsICSWebService.Model
{
    public class QueryTransactionReq
    {
        [DataMember]
        public string TerminalCode { get; set; }
        [DataMember]
        public string TransactionCode { get; set; }
    }

    public class QueryTransactionRes: ResponseModel
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

    public class GetTerminalsReq
    {
        [DataMember]
        public string AgentCode { get; set; }
        [DataMember]
        public string USerName { get; set; }
        [DataMember]
        public string Password { get; set; }
    }

    public class GetTerminalsRes: ResponseModel
    {
        public IList<Terminal> terminal { get; set; }
    }

    public class GetTerminalDetailsReq
    {
        [DataMember]
        public string AgentCode { get; set; }
        [DataMember]
        public string TerminalCode { get; set; }
        [DataMember]
        public string USerName { get; set; }
        [DataMember]
        public string Password { get; set; }
    }

    public class GetTerminalDetailsRes: ResponseModel
    {
        public Terminal terminal { get; set; }
    }

}
