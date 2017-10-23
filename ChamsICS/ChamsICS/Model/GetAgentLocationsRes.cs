using System.Collections.Generic;

namespace ChamsICSWebService.Model
{
    public class TerminalLocation
    {
        public string LocationCode { get; set; }
        public string LocationDescription { get; set; }

    }

    public class GetAgentLocationsRes: Response
    {
        public IList<TerminalLocation> Locations { get; set; }
    }

    public class GetAgentLocationsReq
    {
        public string AgentCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}