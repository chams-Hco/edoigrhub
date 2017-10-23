using CICSWebPortal.Models;
using CICSWebPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace CICSWebPortal.Helpers
{
    public class Utility
    {

        public static IEnumerable<System.Web.Mvc.SelectListItem> GetClients(IDataService DataContext, int RoleId, int ClientId)
        {
            if (RoleId > 2)
            {
                var types = DataContext.GetAllClients().Where(x=> x.ClientId==ClientId).Select(x =>
                                    new System.Web.Mvc.SelectListItem
                                    {
                                        Value = x.ClientId.ToString(),
                                        Text = x.ClientName
                                    });

                return new SelectList(types, "Value", "Text");
            }
            else
            {
                var types = DataContext.GetAllClients().Select(x =>
                                    new System.Web.Mvc.SelectListItem
                                    {
                                        Value = x.ClientId.ToString(),
                                        Text = x.ClientName
                                    });

                return new SelectList(types, "Value", "Text");
            }
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> GetRoles(IDataService DataContext, int roleId)
        {

            if (roleId == 1)
            {
                int[] rangeValue = { 2, 3, 4 };
                var types = DataContext.GetAllRoles().Where(e => rangeValue.Contains(e.RoleId)).Select(x =>
                                  new System.Web.Mvc.SelectListItem
                                  {
                                      Value = x.RoleId.ToString(),
                                      Text = x.RoleName
                                  });

                return new SelectList(types, "Value", "Text");
            }
            else if (roleId == 3)
            {
                int[] rangeValue = { 4, 5, 6 };
                var types = DataContext.GetAllRoles().Where(e => rangeValue.Contains(e.RoleId)).Select(x =>
                                new System.Web.Mvc.SelectListItem
                                {
                                    Value = x.RoleId.ToString(),
                                    Text = x.RoleName
                                });

                return new SelectList(types, "Value", "Text");
            }
            else if (roleId == 5)
            {
                int[] rangeValue = { 6 };
                var types = DataContext.GetAllRoles().Where(e => rangeValue.Contains(e.RoleId)).Select(x =>
                                new System.Web.Mvc.SelectListItem
                                {
                                    Value = x.RoleId.ToString(),
                                    Text = x.RoleName
                                });

                return new SelectList(types, "Value", "Text");
            }

            return new List<System.Web.Mvc.SelectListItem>();

        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> GetAgents(IDataService DataContext, int RoleId, int UserTypeParentId)
        {
            if (RoleId <= 2)
            {
                var types = DataContext.GetAllAgents().Select(x =>
                                    new System.Web.Mvc.SelectListItem
                                    {
                                        Value = x.AgentId.ToString(),
                                        Text = x.Name
                                    });

                return new SelectList(types, "Value", "Text");
            }
            else if(RoleId <=4){
                var types = DataContext.GetAllAgentsByClientId(UserTypeParentId).Select(x =>
                                    new System.Web.Mvc.SelectListItem
                                    {
                                        Value = x.AgentId.ToString(),
                                        Text = x.Name
                                    });

                return new SelectList(types, "Value", "Text");
            }
            else {
                var types = DataContext.GetAllAgentsByClientId(UserTypeParentId).Where(x=> x.AgentId==UserTypeParentId).Select(x =>
                                    new System.Web.Mvc.SelectListItem
                                    {
                                        Value = x.AgentId.ToString(),
                                        Text = x.Name
                                    });

                return new SelectList(types, "Value", "Text");
            }
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> GetTerminals(IDataService DataContext, int RoleId, int UserTypeParentId)
        {
            if (RoleId <= 2)
            {
                var types = DataContext.GetAllTerminals().Select(x =>
                                    new System.Web.Mvc.SelectListItem
                                    {
                                        Value = x.AgentId.ToString(),
                                        Text = x.Name
                                    });

                return new SelectList(types, "Value", "Text");
            }
            else if (RoleId <= 4)
            {
                var types = DataContext.GetAllTerminalsByClientId(UserTypeParentId).Select(x =>
                    new System.Web.Mvc.SelectListItem
                    {
                        Value = x.AgentId.ToString(),
                        Text = x.Name
                    });

                return new SelectList(types, "Value", "Text");
            }

            else {
                var types = DataContext.GetAllTerminalsByAgentId(UserTypeParentId).Select(x =>
                    new System.Web.Mvc.SelectListItem
                    {
                        Value = x.AgentId.ToString(),
                        Text = x.Name
                    });

                return new SelectList(types, "Value", "Text");
            }
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> GetRevenues(IDataService DataContext, int RoleId, int ClientId)
        {

            var types = DataContext.GetAllRevenueHead().Select(x =>
                                new System.Web.Mvc.SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Name
                                });

            return new SelectList(types, "Value", "Text");
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> GetMDAs(IDataService DataContext, int RoleId, int ClientId)
        {
            if (RoleId > 2)
            {

                var types = DataContext.GetAllMinistry().Select(x =>
                                new System.Web.Mvc.SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Name
                                });
                return new SelectList(types, "Value", "Text");

            }
            else
            {
                var types = DataContext.GetAllMinistryByClientId(ClientId).Select(x =>
                                new System.Web.Mvc.SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Name
                                });
                return new SelectList(types, "Value", "Text");

            }
        }

        public static int GetParentId(int roleId, int selectedClientId, int selectedAgentId)
        {
            if (roleId == 2)
            {
                return 0;
            }
            else if (roleId == 3 || roleId == 4)
            {
                return selectedClientId;
            }
            else if (roleId == 5 || roleId == 6)
            {
                return selectedAgentId;
            }

            return -1;
        }
    }
}