using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Services
{
    public  class MainContainer
    {

        private MainContainer()
        {

        }

        private static IDataService _dataService;

        public static IDataService DataService()
        {
           
                if(_dataService==null)
                {
                    _dataService = new DataService();
                }
                return _dataService;          
        }
    }
}