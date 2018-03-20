using CICSWebPortal.Models;
using CICSWebPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace CICSWebPortal.Helpers
{
    public static class Utility
    {
        /// <summary>
        /// Extension method to convert IEnumerable to dataset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static DataSet ConvertToDataSet<T>(this IEnumerable<T> dataSource)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T   
            foreach (var propInfo in elementType.GetProperties())
            {
                t.Columns.Add(propInfo.Name, propInfo.PropertyType);
            }

            //go through each property on T and add each value to the table   
            foreach (T item in dataSource)
            {
                DataRow row = t.NewRow();
                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null);
                }
                t.Rows.Add(row);
            }
            return ds;
        }

        /// <summary>
        /// Helper method to data to excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="ctx"></param>
        /// <param name="Filename"></param>
        public static void ExportToExcel(DataSet ds, HttpContext ctx, String Filename)
        {
            ctx.Response.Clear();
            ctx.Response.Buffer = true;
            ctx.Response.AddHeader("content-disposition", "attachment;filename=" + Filename + ".xls");
            ctx.Response.Charset = "";
            ctx.Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                GridView gr = new GridView();
                gr.DataSource = ds;
                gr.DataBind();
                gr.AllowPaging = false;


                gr.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gr.HeaderRow.Cells)
                {
                    cell.BackColor = gr.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gr.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gr.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gr.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gr.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                ctx.Response.Write(style);
                ctx.Response.Output.Write(sw.ToString());
                ctx.Response.Flush();
                ctx.Response.End();
            }
        }

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
                int[] rangeValue = { 2, 3, 4, 8 };
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