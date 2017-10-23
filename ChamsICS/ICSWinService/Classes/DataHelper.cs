using ChamsICSLib.Data;
using ICSWinService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSWinService.Classes
{
    public class DataHelper
    {
        CICSEntities db;
        public DataHelper()
        {
            db = new CICSEntities();
        }

    }
}
