using System.Collections.Generic;

namespace ChamsICSWebService.Model
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Recipient { get; set; }
        public int? ReferenceId { get; set; }
        public int? Retry { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public string Type { get; set; }
    }

    public class GetNotificationsRes: Response
    {
        public List<Notification> Notifications { get; set; }
    }
}