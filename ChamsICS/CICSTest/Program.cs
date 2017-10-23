using ChamsICSLib.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICSTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Utils.IsValidEmail("otunfikayo@gmail."));
            Console.WriteLine(Utils.IsValidEmail("otunfikayo@gmail.c"));
            Console.WriteLine(Utils.IsValidEmail("o@gmail.co.cc.ng"));
            Console.WriteLine(Utils.IsValidEmail("o@.ng"));
            Console.WriteLine(Utils.IsValidEmail("ogmail.co.cc.ng"));
            Console.WriteLine(Utils.IsValidEmail("o@gmail.co"));
            Console.WriteLine(Utils.IsValidEmail("o@g.ng"));

            //TestSendEmail();
            Console.Read();
        }

        private static void TestSendEmail()
        {
            Messaging Messaging = new Messaging();
            string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            string emailPassword = ConfigurationManager.AppSettings["emailPassword"];
            string mail_server = ConfigurationManager.AppSettings["mail_server"];
            int mail_port = Convert.ToInt32(ConfigurationManager.AppSettings["mail_port"]);
            string mail_from = ConfigurationManager.AppSettings["mail_from"];
            string mail_sender = ConfigurationManager.AppSettings["mail_sender"];
            string mail_name = ConfigurationManager.AppSettings["mail_name"];
            string mail_pwd = ConfigurationManager.AppSettings["mail_pwd"];

            //SendSMSTest();
            string status = string.Empty;
            var result = Messaging.SendEmail(mail_from, new string[] { "otunfikayo@gmail.com" }, new string[] { }, new string[] { },
               "Testing Again", "IGR Hub Notification", out status);

            Console.WriteLine(status);
        }

        private static void SendSMSTest()
        {
            Messaging Messaging = new Messaging();
            string status = string.Empty;
            bool result = Messaging.SendSMS("2348177778513", "Testing", "IGRHub", out status);
            Console.WriteLine(status);
        }
    }
}
