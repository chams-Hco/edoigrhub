using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService.Model
{
    [DataContract]
    public class UserLoginRes: ResponseModel
    {
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string SessionId { get; set; }
    }

    [DataContract]
    public class UserLoginReq
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string UserRole { get; set; }
        [DataMember]
        public string UserPassword { get; set; }
    }
}
