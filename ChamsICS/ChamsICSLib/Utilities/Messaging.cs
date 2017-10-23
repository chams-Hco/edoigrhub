using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace ChamsICSLib.Utilities
{
    public class Messaging
    {
        string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
        string emailPassword = ConfigurationManager.AppSettings["emailPassword"];
        string mail_server = ConfigurationManager.AppSettings["mail_server"];
        int mail_port = Convert.ToInt32(ConfigurationManager.AppSettings["mail_port"]);
        string mail_from = ConfigurationManager.AppSettings["mail_from"];
        string mail_sender = ConfigurationManager.AppSettings["mail_sender"];
        string mail_name = ConfigurationManager.AppSettings["mail_name"];
        string mail_pwd = ConfigurationManager.AppSettings["mail_pwd"];

        public  bool SendSMS(string receiver, string message, string sender, out string status)
        {             
            bool result = false;

            status = "-1";
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                receiver = receiver.StartsWith("234") ? receiver : "234" + receiver.TrimStart(new char[] { '0' });

                string urlArgs = string.Format("to={0}&message={1}&sender={2}", receiver, message, sender);

                string sms_url = ConfigurationManager.AppSettings["sms_url"];
                string url = sms_url + urlArgs;

                string fullURl = url;
                WebRequest request = WebRequest.Create(fullURl);

                response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                status = responseFromServer;
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();

                result = true;
            }
            catch (Exception e)
            {
                status = e.Message;
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
                status = e.Message;

                return false;
            }

            return result;
        }

        public  bool SendEmail(string from, string[] to, string[] cc, string[] bcc, string message, string subject, out string status)
        {
            bool result = false;
            status = "-1";
            try
            {
                MailMessage objeto_mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = mail_port;
                client.Host = mail_server;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(mail_sender, mail_pwd);
                
                objeto_mail.From = new MailAddress(from,"IGRHub");
                objeto_mail.ReplyToList.Add(new MailAddress("itsupport@chams.com","Chams IT Support"));
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Priority = MailPriority.High;

                if (to.Length < 1)
                {
                    status = "Invalid To Address";
                    return false;
                }
                foreach (string s in to)
                {
                    objeto_mail.To.Add(s);
                }
                if (cc.Length > 0)
                {
                    foreach (string s in cc)
                    {
                        objeto_mail.To.Add(s);
                    }
                }
                if (bcc.Length > 0)
                {
                    foreach (string s in bcc)
                    {
                        objeto_mail.Bcc.Add(s);
                    }
                }
                objeto_mail.Subject = subject;
                objeto_mail.Body = message;

                client.Send(objeto_mail);

                status = "00";
                result = true;            
            }
            catch (Exception e)
            {
                status = e.Message;
                result = false;
            }

            return result;
        }

        public  bool SendEmail(string from, string[] to, string[] cc, string[] bcc, string message, ListDictionary replacements, string subject, out string status)
        {
            bool result = false;
            status = "-1";
            try
            {
                MailMessage objeto_mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = mail_port;
                client.Host = mail_server;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(mail_sender, mail_pwd);
                
                objeto_mail.From = new MailAddress(from,"IGRHub");
                objeto_mail.ReplyToList.Add(new MailAddress("itsupport@chams.com","Chams IT Support"));
                objeto_mail.IsBodyHtml = true;
                objeto_mail.Priority = MailPriority.High;

                if (to.Length < 1)
                {
                    status = "Invalid To Address";
                    return false;
                }
                foreach (string s in to)
                {
                    objeto_mail.To.Add(s);
                }

                if (cc.Length > 0)
                {
                    foreach (string s in cc)
                    {
                        objeto_mail.To.Add(s);
                    }
                }
                if (bcc.Length > 0)
                {
                    foreach (string s in bcc)
                    {
                        objeto_mail.Bcc.Add(s);
                    }
                }
                objeto_mail.Subject = subject;
                objeto_mail.Body = message;

                client.Send(objeto_mail);

                status = "00";
                result = true;
            }
            catch (Exception e)
            {
                status = e.Message;
                result = false;
            }

            return result;
        }

        public  void sendUserMessage(string from_email,string from_name, string subject, string to_email, 
            string to_name, string server, int server_port, string server_user, string server_password)
        {
            StringBuilder Msg_ = new StringBuilder();
            string ty = "Thank you " + to_name + "! Your request is received. We will get back to you shortly to follow up with your request";
            Msg_.Append("<br/>");

            Msg_.Append("<p>Message: &nbsp;" + ty + "</p>");
            Msg_.Append("<br/>");
            Msg_.Append("<br/>");

            Msg_.Append("<p>Best Regards,</p>");
            Msg_.Append("<br/>");
            Msg_.Append("<p><b>SM TOTAL</b></p>");

            //MailMessage m = new MailMessage(fromEmail, toEmail, subject, Msg_.ToString());
           
            MailMessage m = new MailMessage();

            m.Priority = MailPriority.High;
            m.To.Add(new MailAddress(to_email));
            MailAddress fromAddress = new MailAddress(from_email, from_name);
            m.From = fromAddress;
            m.Subject = subject;

            // Specify an HTMLmessagebody
            m.Body = Msg_.ToString();
            m.IsBodyHtml = true;

            // Send themessage
            SmtpClient client = new SmtpClient(server);
            client.Port = server_port;
            client.Credentials = new System.Net.NetworkCredential(server_user, server_password);
            client.Send(m);
        }
    }
}
