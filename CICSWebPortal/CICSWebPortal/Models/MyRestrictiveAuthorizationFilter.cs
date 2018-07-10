using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class MyRestrictiveAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize([NotNull] DashboardContext context)
        {


            try
            {
                var user = (ViewModels.UserDashBoardViewModel)HttpContext.Current.Session["LoggedInUser"];

                if (user.RoleCode == "SA")
                    return true;

                return false;
            }
            catch (Exception)
            {

                return false;
            }


        }
    }
}