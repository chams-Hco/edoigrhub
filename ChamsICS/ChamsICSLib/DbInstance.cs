using ChamsICSLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSLib
{
   public class DbInstance
    {
       private static CICSEntities _db = null;
       private DbInstance()
       {

       }

       public static CICSEntities Db()
       {
              
              if(_db==null)
              {
                  _db = new CICSEntities();
              }
               return _db;       
       }
    }
}
