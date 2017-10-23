using ChamsICSLib.Data;
using ChamsICSLib.Utilities;
using ICSWinService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSWinService.Classes
{
    public abstract class ServicesAdapter
    {
        public CICSEntities db;

        public string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
        public string emailPassword = ConfigurationManager.AppSettings["emailPassword"];
        public string mail_server = ConfigurationManager.AppSettings["mail_server"];
        public int mail_port = Convert.ToInt32(ConfigurationManager.AppSettings["mail_port"]);
        public string mail_from = ConfigurationManager.AppSettings["mail_from"];
        public string mail_sender = ConfigurationManager.AppSettings["mail_sender"];
        public string mail_name = ConfigurationManager.AppSettings["mail_name"];
        public string mail_pwd = ConfigurationManager.AppSettings["mail_pwd"];

        public string dataBatchSize = ConfigurationManager.AppSettings["BGServiceBatchSize"];
        public string UserSMSTemplete = ConfigurationManager.AppSettings["USER_SMS_TEMPLETE"];
        public string UserEmailTemplete = ConfigurationManager.AppSettings["USER_EMAIL_TEMPLETE"];
        public string UserEmail = ConfigurationManager.AppSettings["USER_EMAIL"];

        public string ResidentSMSTemplete = ConfigurationManager.AppSettings["RESIDENT_SMS_TEMPLETE"];
        public string ResidentEmailTemplete = ConfigurationManager.AppSettings["RESIDENT_EMAIL_TEMPLETE"];

        public string ResidentEmail = ConfigurationManager.AppSettings["RESIDENT_EMAIL"];

        public int? retryLimit = Convert.ToInt32(ConfigurationManager.AppSettings["RetryLimit"]);
        public string errorLogPath = ConfigurationManager.AppSettings["ErrorLoggingPath"];
        public string debuggingPath = ConfigurationManager.AppSettings["DebugLoggingPath"];
        public string failed_upload_path =  ConfigurationManager.AppSettings["DebugLoggingPath"]+"\\Failed_Upload";

        public int totalProcessed = 0;
        public double bgWorkerProgress = 0.0;
        public BackgroundWorker bgWorker;
        public StringBuilder bgWorkerLogBuffer;
        public StringBuilder bgWorkerLog;
        public Messaging Messaging;

        internal virtual void initializeSeviceSettings()
        {
            this.db = new CICSEntities();

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += new DoWorkEventHandler(bg_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bg_ProgressChanged);

            bgWorkerLog = new StringBuilder();
            bgWorkerLogBuffer = new StringBuilder();

            Messaging = new Messaging();
        }

        public virtual void validateSettingsState()
        {
            if (bgWorker == null) throw new Exception("bgWorker is not initialised");
            if (bgWorkerLog == null) throw new Exception("bgWorkerLog is not initialised");
            if (bgWorkerLogBuffer == null) throw new Exception("bgWorkerLogBuffer is not initialised");
        }

        internal virtual void UpdateProgressCompleted(int progressCount)
        {
            bgWorkerProgress = bgWorkerProgress += 1;
            var PercentageProgress = bgWorkerProgress / progressCount;
            var PercentageComplete = PercentageProgress * 100;
            bgWorker.ReportProgress(Convert.ToInt32(PercentageComplete), null);
        }

        internal virtual void UpdateProcessLog(string log)
        {
            bgWorkerLogBuffer.AppendLine(log + " : " + DateTime.Now.ToString());
        }

        internal virtual void UpdateProcessLog(Exception ex)
        {
            bgWorkerLogBuffer.AppendLine(ex.Message + " : " + DateTime.Now.ToString());
            Logger.logToFile(ex, errorLogPath);
        }

        internal virtual void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            //Get A Fresh Connection to the DBContect Instance for this Job.

            bgWorkerLogBuffer = new StringBuilder();
            UpdateProcessLog("Service Opperations Running");

            totalProcessed = RunServiceOpperation(dataBatchSize);

            UpdateProcessLog(totalProcessed + " Records Processed");
        }

        internal virtual void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                UpdateProcessLog(e.Error.Message);
            }
            if (e.Cancelled)
            {
                UpdateProcessLog("Service Opperations Was Cancelled");
            }
            UpdateProcessLog("Service Opperations End");
            bgWorkerLog = bgWorkerLogBuffer;
            bgWorkerProgress = 0.0;
        }

        internal virtual void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdateProcessLog(e.ProgressPercentage + " Completed..");
            bgWorkerLog = bgWorkerLogBuffer;
        }

        internal virtual int RunServiceOpperation(string dataBatchSize)
        {
            return totalProcessed;
        }

    }
}
