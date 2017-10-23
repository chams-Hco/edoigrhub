using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService.Model
{
    [DataContract(Namespace ="")]
    public class VerifyIdResponse
    {
        [DataMember]
        public string ResponseCode { get; set; }
        [DataMember]
        public string ResponseDescription { get; set; }
        [DataMember]
        public string TITLEPREFIX { get; set; }
        [DataMember]
        public string SURNAME { get; set; }
        [DataMember]
        public string FIRSTNAME { get; set; }
        [DataMember]
        public string MIDDLENAME { get; set; }
        [DataMember]
        public string DOB { get; set; }
        [DataMember]
        public string GENDER { get; set; }
        [DataMember]
        public string EMAIL { get; set; }
        [DataMember]
        public string MOBILENUMBER { get; set; }
        [DataMember]
        public string RESIDENTIAL_ADDRESS { get; set; }
        [DataMember]
        public string CONTACT_ADDRESS { get; set; }
        [DataMember]
        public string OFFICE_ADDRESS { get; set; }
        [DataMember]
        public string STATE { get; set; }
        [DataMember]
        public string LGA { get; set; }
        [DataMember]
        public string NOK_ADDRESS { get; set; }
        [DataMember]
        public string NOK_NAME { get; set; }
        [DataMember]
        public string NOK_RELATIONSHIP { get; set; }
    }

    [DataContract]
    public class VerifyResidentIdReq
    {
        [DataMember]
        public string AgentCode { get; set; }
        [DataMember]
        public string TerminalCode { get; set; }
        [DataMember]
        public string ResidentId { get; set; }
    }

    [DataContract]
    public class VerifyResidentIdRes : ResponseModel
    {
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
    }
}
