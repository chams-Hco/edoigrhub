using System.Collections.Generic;

namespace ChamsICSWebService.Model
{
    public class Location
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public int? AgentId { get; set; }
        public string LocationCode { get; set; }
        public string LocationDescription { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }

    public class CreateLocationRes : Response
    {
        public int LocationId { get; set; }
    }

    public class FindLocationRes : Response
    {
        public Location Location { get; set; }
    }

    public class GetAllLocationRes : Response
    {
        public IEnumerable<Location> Locations { get; set; }
    }
}