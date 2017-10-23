using System.Collections.Generic;
using System.Linq;

namespace ChamsICSWebService.Model
{
    public class GetAllErrorTransactionRes:Response
    {
        public IEnumerable<ErrorTransaction> Transactions { get; set; }
    }
}