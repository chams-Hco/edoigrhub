using ChamsICSLib.Data;
using ChamsICSLib.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSWinService.Classes
{
    class NotificationService: ServicesAdapter
    {
        public NotificationService()
        {
            initializeSeviceSettings();
        }

        internal override int RunServiceOpperation(string dataBatch)
        {
            int result = 0;//Number of Funds Sources Processed;

            int batchSize = 0;
            Int32.TryParse(dataBatch, out batchSize);
            IList<Notification> notifications = LoadNotifications(batchSize);

            foreach (var item in notifications)
            {
                UpdateProgressCompleted(notifications.Count());
                switch (item.TypeId)
                {
                    case ChamsICSLib.Model.NotificationType.USER_SMS:
                        result += SendUserSMS(item);
                        break;
                    case ChamsICSLib.Model.NotificationType.USER_EMAIL:
                        result += SendUserEmail2(item);
                        break;
                    case ChamsICSLib.Model.NotificationType.RESIDENT_SMS:
                        result += SendResidentSMS(item);
                        break;
                    case ChamsICSLib.Model.NotificationType.RESIDENT_EMAIL:
                        result += SendResidentEmail2(item);
                        break;
                    default:
                        break;
                }
                db.SaveChanges();
            }
            return result;
        }

        //Templete: Your Profile has been setup on IGRHub. Login to www.igrhub.com to activate your account. Profile Details: {0}
        private int SendUserSMS(Notification item)
        {
            int processed = 0;
            string messageVariables = item.Message;
            string receiver = item.Recipient;
            string[] messageParts = messageVariables.Split(new string[]{ "||" },StringSplitOptions.RemoveEmptyEntries);

            string message = string.Format(UserSMSTemplete, messageVariables);
            //Send the SMS
            string response = string.Empty;
            Messaging.SendSMS(receiver, message, "IGRHub", out response);

            //Update Notification Service in Database
            item.Status = response.StartsWith("0") ? 1 : 0;
            item.StatusMessage = response;
            item.RetryCount = item.RetryCount!=null? item.RetryCount+1 : 1;

            processed = 1;
            return processed;
        }

        //Templete: Dear {0} Your Profile has been setup on IGRHub. Login to www.igrhub.com to activate your account. Profile Details: {1}
        private int SendUserEmail(Notification item)
        {
            int processed = 0;
            string messageVariables = item.Message;
            string receiver = item.Recipient;
            string[] messageParts = messageVariables.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            string messageSignature = Environment.NewLine + "Regards," + Environment.NewLine + "IGRHub Support Team" + Environment.NewLine + "http://www.igrhub.com/";
            string message = string.Format(UserEmailTemplete, item.Recipient+Environment.NewLine, messageVariables+messageSignature);
            //Send the SMS
            string response = string.Empty;
            Messaging.SendEmail(mail_from, new string[] {item.Recipient },new string[] { },new string[] { },message, "IGRHub System Notification", out response);

            //Update Notification Service in Database
            item.Status = response.StartsWith("0") ? 1 : 0;
            item.StatusMessage = response;
            item.RetryCount = item.RetryCount != null ? item.RetryCount + 1 : 1;

            processed = 1;
            return processed;
        }

        private int SendUserEmail2(Notification item)
        {
            int processed = 0;
            string messageVariables = item.Message;
            string receiver = item.Recipient;
            string[] messageParts = messageVariables.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            string body = string.Empty;
            using (StreamReader sr = new StreamReader(UserEmail))
            {
                body = sr.ReadToEnd();
            }
            body = body.Replace("{name}", receiver);
            body = body.Replace("{profile}", messageVariables);

            string message = body;
            
            //Send the SMS
            string response = string.Empty;
            Messaging.SendEmail(mail_from, new string[] {item.Recipient },new string[] { },new string[] { },message, "IGRHub System Notification", out response);

            //Update Notification Service in Database
            item.Status = response.StartsWith("0") ? 1 : 0;
            item.StatusMessage = response;
            item.RetryCount = item.RetryCount != null ? item.RetryCount + 1 : 1;

            processed = 1;
            return processed;
        }

        //Template: Your Payment of {0} for {1} was processed successfully on {2}. Your Transaction Code: {3}.
        private int SendResidentSMS(Notification item)
        {
            int processed = 0;
            string messageVariables = item.Message;
            string receiver = item.Recipient;
            string[] messageParts = messageVariables.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            string message = string.Format(ResidentSMSTemplete, 
                messageParts[0], 
                messageParts[1],
                messageParts[2], 
                messageParts[3]);
            //Send the SMS
            string response = string.Empty;
            Messaging.SendSMS(receiver, message, "IGRHub", out response);

            //Update Notification Service in Database
            item.Status = response.StartsWith("0") ? 1 : 0;
            item.StatusMessage = response;
            item.RetryCount = item.RetryCount != null ? item.RetryCount + 1 : 1;

            processed = 1;
            return processed;
        }

        //Templete: Dear {0}, Your Payment of {1} for {2} was processed successfully on {3}. Your Transaction Code is: {4}.
        private int SendResidentEmail(Notification item)
        {
            int processed = 0;
            string messageVariables = item.Message;
            string receiver = item.Recipient;
            string[] messageParts = messageVariables.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            string messageSignature = Environment.NewLine + "Regards," + Environment.NewLine + "IGRHub Support Team" + Environment.NewLine + "http://www.igrhub.com/";

            string message = string.Format(ResidentSMSTemplete,
                item.Recipient+
                Environment.NewLine, 
                messageParts[0], 
                messageParts[1], 
                messageParts[2], 
                messageParts[3]+
                messageSignature);
            //Send the SMS
            string response = string.Empty;
            Messaging.SendEmail(mail_from, new string[] {item.Recipient}, new string[] { }, new string[] { }, message, "IGRHub System Notification", out response);

            //Update Notification Service in Database
            item.Status = response.StartsWith("0") ? 1 : 0;
            item.StatusMessage = response;
            item.RetryCount = item.RetryCount != null ? item.RetryCount + 1 : 1;

            processed = 1;
            return processed;
        }

        //1200||FLAT (2 - 4 BEDROOM) / SANITATION LEVY / MINISTRY OF ENVIRONMENT||2/1/2016 12:00:00 AM||1001Z000024451230321
        private int SendResidentEmail2(Notification item)
        {
            int processed = 0;
            string messageVariables = item.Message;
            string receiver = item.Recipient;
            string[] messageParts = messageVariables.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);


            string body = string.Empty;
            using (StreamReader sr = new StreamReader(ResidentEmail))
            {
                body = sr.ReadToEnd();
            }

            body = body.Replace("{name}", receiver);
            body = body.Replace("{amount}", messageParts[0]);
            body = body.Replace("{service}", messageParts[1]);
            body = body.Replace("{date}", messageParts[2]);
            body = body.Replace("{code}", messageParts[3]);
            body = body.Replace("{receipt}", messageVariables);

            string message = body;

            //Send the SMS
            string response = string.Empty;
            Messaging.SendEmail(mail_from, new string[] {item.Recipient}, new string[] { }, new string[] { }, message, "IGRHub System Notification", out response);

            //Update Notification Service in Database
            item.Status = response.StartsWith("0") ? 1 : 0;
            item.StatusMessage = response;
            item.RetryCount = item.RetryCount != null ? item.RetryCount + 1 : 1;

            processed = 1;
            return processed;
        }

       internal IList<Notification> LoadNotifications(int batchSize)
        {
            //Load records that has not been flagged as treated...into Memory
            var notifications = db.Notifications.Where(n => n.Status==0 
            && (n.RetryCount==null || n.RetryCount < retryLimit))
                .Take(batchSize).ToList();
            UpdateProcessLog("Loaded Notifications " + notifications.Count().ToString());
            return notifications;
        }
    }
}
