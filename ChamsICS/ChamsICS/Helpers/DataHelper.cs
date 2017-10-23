using ChamsICSLib;
using ChamsICSLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChamsICSWebService
{
    public class DataHelper
    {
        public static CICSEntities db = DbInstance.Db();

        //constant that holds the payment target wallet
        public const string WALLET_PAYMENT = "wallet";

        //constant that holds the payment target wallet
        public const string TRANSACTION_PAYMENT = "transaction";
    }
}