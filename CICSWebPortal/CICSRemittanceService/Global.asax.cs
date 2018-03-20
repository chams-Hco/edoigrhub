using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.SessionState;

namespace CICSRemittanceService
{
    public class Global : System.Web.HttpApplication
    {
        //private static bool __initialized;
        //public Guid AppId = Guid.NewGuid();
        //public String TestMessage;

        //protected DefaultProfile Profile
        //{
        //    get
        //    {
        //        return (DefaultProfile)base.Context.Profile;
        //    }
        //}
        //// events go here
        //[DebuggerNonUserCode]
        //public Global()
        //{
        //    if (!__initialized)
        //    {
        //        __initialized = true;
        //    }
        //}

        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var soapAction = Request.Headers["SOAPAction"];

            if (string.IsNullOrEmpty(soapAction) || soapAction == "\"\"")
                Request.Headers.Remove("SOAPAction");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}