using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChamsICSWebService.Model
{
    public class Wallet
    {
        public int WalletId { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
    }

    public class FindWalletRes : Response
    {
        public Wallet Wallet { get; set; }
    }

    public class FundWalletRes : Response
    {
        public string PaymentReference { get; set; }
    }
}