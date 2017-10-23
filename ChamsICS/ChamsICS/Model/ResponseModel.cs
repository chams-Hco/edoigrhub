using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService.Model
{
    [DataContract]
    public class Response
    {
        [DataMember(Order = 0)]
        public string ResponseCode { get; set; }
        [DataMember(Order = 1)]
        public string ResponseDescription { get; set; }
    }
}
