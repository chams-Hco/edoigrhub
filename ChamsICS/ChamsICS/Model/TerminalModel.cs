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

        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
    }

    [DataContract]
    public class AuthoriseTerminalRes: Response
    {
        [DataMember]
        public string TerminalCode { get; set; }

        [DataMember]
        public string TerminalSerialNumber { get; set; }
    }

    public class Terminal
    {
        public int TerminalId { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public int status { get; set; }
    }

    public class FindTerminalRes : Response
    {
        public Terminal Terminal { get; set; }
    }

    public class GetAllTerminalRes : Response
    {
        public IEnumerable<Terminal> Terminals { get; set; }
        
    }

    public class GetTerminalsReq
    {
        [DataMember]
        public string AgentCode { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
    }

    public class GetTerminalsRes : Response
    {
        public IList<ServiceTerminal> Terminals { get; set; }
    }

    public class FindTerminalReq
    {
        [DataMember]
        public string AgentCode { get; set; }
        [DataMember]
        public string TerminalCode { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
    }

    public class GetTerminalDetailsRes : Response
    {
        public Terminal terminal { get; set; }
    }

    public class ServiceTerminal
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string status { get; set; }
    }
}
