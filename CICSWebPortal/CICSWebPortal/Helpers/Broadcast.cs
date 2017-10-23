using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CICSWebPortal.Helpers
{
    internal class Broadcast
    {
        internal static bool SendEmail(string to, string subject, string body, string from, string fromname)
        {
            var objeto_mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential nc = new System.Net.NetworkCredential();
            nc.UserName = "services@chams.com";
            nc.Password = "welcome12@";
            client.Credentials = nc;

            objeto_mail.From = new MailAddress(from, fromname);

            // to = "afakokunde@chams.com,akinfaks@yahoo.com";

            if (to.Contains(","))
            {
                foreach (string add in to.Split(','))
                {
                    objeto_mail.To.Add(new MailAddress(add));
                }
            }
            else
            {
                objeto_mail.To.Add(new MailAddress(to));
            }

            objeto_mail.Subject = subject;
            objeto_mail.Body = body;
            objeto_mail.IsBodyHtml = true;
            client.Send(objeto_mail);
            return true;
        }

        internal static String WebPageToCode(string Url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            return result;
        }
    }
}