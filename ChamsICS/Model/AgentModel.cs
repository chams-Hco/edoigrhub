using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService.Model
{
    public class Agent
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string status { get; set; }
    }

    public class CreateAgentRes: ResponseModel
    {
        public string AgentId { get; set; }
        public string AgentCode { get; set; }
    }

    public class FindAgentRes: ResponseModel
    {
        public Agent Agent {get;set;}
    }

    public class GetAllAgentRes: ResponseModel
    {
        public IEnumerable<Agent> Agents { get; set; }
    }

}
