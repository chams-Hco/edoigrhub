using CICSWebPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.ViewModels
{
    public class WalletViewModel
    {
        public Wallet Wallet { get; set; }
        public Payment Payment { get; set; }
    }
}