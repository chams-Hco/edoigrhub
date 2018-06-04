using System;
using System.Collections.Generic;
using System.Text;

namespace ChamsICSWebService.Model
{
    public class MemberRequest
    {
        public int? Id { get; set; }
        public string OtherSearch { get; set; }
        public SearchType SearchType { get; set; } = SearchType.Id;
    }

    public enum SearchType
    {
        Id = 0, OtherSearch = 1,
    }
}
