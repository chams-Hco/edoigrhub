using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService.Model
{
    public class Agent
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public int status { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }

    public class CreateAgentRes: Response
    {
        public string AgentId { get; set; }
        public string AgentCode { get; set; }
    }

    public class FindAgentRes: Response
    {
        public Agent Agent {get;set;}
    }

    public class GetAllAgentRes: Response
    {
        public IEnumerable<Agent> Agents { get; set; }
    }
}
