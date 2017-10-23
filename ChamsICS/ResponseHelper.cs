using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChamsICSWebService
{
    public static class ResponseHelper
    {
        public const string SUCCESS = "0000";
        public const string FAILED = "1001";
        public const string VALIDATION_ERROR = "1002";
        public const string APPLICATION_ERROR = "1003";
        public const string VERIFICATION_SERVICE_ERROR = "1004";
        public const string UNKNOWN_ERROR = "1005";
    }
}