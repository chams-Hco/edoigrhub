using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSWinService.Models
{

    public class UploadTransactionReq
    {
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
        public string TransactionDate { get; set; }
        public string Amount { get; set; }
        public string PaymentReference { get; set; }
    }
}
