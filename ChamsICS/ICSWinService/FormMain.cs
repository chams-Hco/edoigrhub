using ICSWinService.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICSWinService
{
    public partial class FormMain : Form
    {
        ServicesAdapter notificationService;
        ErrorUploadImporter errorUploadImporter;
        public bool clearConsoleAfterBGWorkerComplete = true;
        public string ServiceRefreshInterval = ConfigurationManager.AppSettings["ServiceRefreshInterval"];
        public bool autoImportUploadError = ConfigurationManager.AppSettings["AutoImportUploadError"] == "1" ? true : false;

        public FormMain()
        {
            InitializeComponent();
            notificationService = new NotificationService();
            errorUploadImporter = new ErrorUploadImporter();

        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void FormMain_Shown(object sender, EventArgs e)
        {

            timer.Interval = Convert.ToInt32(ServiceRefreshInterval);

            initLogTree();
            try
            {
                timer.Start();
                UpdateUIStatus();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void initLogTree()
        {
            try
            {
                //Get and list all Folders in App Log Dir
                String defaultDir = ConfigurationManager.AppSettings["LoggingPath"];

                groupBoxAppLogDirTree.Text = "Application Default Log Directory : " + defaultDir;

                //treeView1.ImageList = globalSettings.ImageList;

                List<string> defaultDirFolders = new List<string>();
                defaultDirFolders.AddRange(Directory.GetDirectories(defaultDir));

                treeView1.Nodes.Clear();
                foreach (String s in defaultDirFolders)
                {
                    treeView1.Nodes.Add(s);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void UpdateUIStatus()
        {
            try
            {
                TabPage activeTabPage = tabServiceMonitor.SelectedTab;
                BackgroundWorker bgWorker = getActiveTabBackgroudWorker(activeTabPage.Name);

                UpdateServerStatusControls(bgWorker, activeTabPage.Text, activeTabPage.Name);

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {

            //Notification
            notificationService.validateSettingsState();
            RunServiceOpperation(textBoxNotification, notificationService.bgWorkerLog, notificationService.bgWorker);

            //Error Upload Importer
            if (autoImportUploadError)
            {
                errorUploadImporter.validateSettingsState();
                RunServiceOpperation(textBoxErrorUploadImporter, errorUploadImporter.bgWorkerLog, errorUploadImporter.bgWorker);
            }
        }

        private void RunServiceOpperation(RichTextBox txtBox, StringBuilder sb, BackgroundWorker bgWorker)
        {
            if (sb != null)
            {
                if (clearConsoleAfterBGWorkerComplete)
                    txtBox.Text = sb.ToString();
                else
                    txtBox.Text += sb.ToString();
            }
            if (!bgWorker.IsBusy)
            {
                bgWorker.RunWorkerAsync();
            }

            tabServiceMonitor.SelectTab(tabServiceMonitor.SelectedIndex);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                String selectedNodeText = e.Node.Text;
                TreeNode selectedNode = e.Node;

                //Check if its a Folder..
                if (Directory.Exists(selectedNodeText))
                {
                    selectedNode.Nodes.Clear();

                    List<string> defaultDirFolders = Directory.GetDirectories(selectedNodeText).ToList();
                    List<string> defaultDirFiles = Directory.GetFiles(selectedNodeText).ToList();

                    foreach (String s in defaultDirFolders)
                    {
                        selectedNode.Nodes.Add(s);
                    }

                    foreach (String s in defaultDirFiles)
                    {
                        selectedNode.Nodes.Add(s);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                String selectedNodeText = e.Node.Text;
                TreeNode selectedNode = e.Node;

                //If its a File, Then Open it with Notepad
                if (File.Exists(selectedNodeText))
                {
                    if (MessageBox.Show(this,
                        String.Format("Are you sure you want to open {0}?", selectedNodeText),
                        "Open CashVault Service Monitor log File", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        //System.Diagnostics.Process.Start("notepad++.exe", selectedNodeText);
                        textBoxLogViewer.Text = File.ReadAllText(selectedNodeText);
                        tabServiceMonitor.SelectedTab = tabLogViewer;
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void refreshLogViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initLogTree();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CVServiceMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseMainForm(e);
        }

        private void CloseMainForm(FormClosingEventArgs e)
        {
            if (ApplicationIsBusy())
            {
                if (MessageBox.Show("Some Background services are currently running.\n" +
                    "Are you sure you want to close the Application?", "Close Service Monitor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    return;
                }
                else
                {
                    SafeCloseBackgroundServices();
                    e.Cancel = true;
                }
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to close the Application?", "Close Service Monitor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    return;
                }
                else
                {
                    SafeCloseBackgroundServices();
                    e.Cancel = true;
                }
            }
        }

        private bool ApplicationIsBusy()
        {
            if (notificationService.bgWorker.IsBusy) { return true; }
            if (errorUploadImporter.bgWorker.IsBusy) { return true; }
            return false;
        }

        void SafeCloseBackgroundServices()
        {
            if (notificationService.bgWorker.IsBusy) { notificationService.bgWorker.CancelAsync(); }
            if (errorUploadImporter.bgWorker.IsBusy) { errorUploadImporter.bgWorker.CancelAsync(); }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                String defaultDir = ConfigurationManager.AppSettings["LoggingPath"];
                //If its a File, Then Open it with Notepad
                if (Directory.Exists(defaultDir))
                {
                    System.Diagnostics.Process.Start(defaultDir);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage activeTabPage = tabServiceMonitor.SelectedTab;
            RichTextBox activeTextBox = activeTabPage.Controls.OfType<RichTextBox>().First();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = activeTabPage.Text + "_Log_" + DateTime.Now.ToString("yyyy-MM-dd");
            try
            {

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    activeTextBox.SaveFile(saveFileDialog1.FileName + ".txt", RichTextBoxStreamType.PlainText);
                    MessageBox.Show("Log save succesfully!");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(this, "Error Saving Log File!" + Environment.NewLine + exp.ToString(), this.Text);
            }
        }

        private void buttonRunService_Click(object sender, EventArgs e)
        {
            try
            {
                TabPage activeTabPage = tabServiceMonitor.SelectedTab;
                BackgroundWorker bgWorker = getActiveTabBackgroudWorker(activeTabPage.Name);

                if (bgWorker.IsBusy)
                {
                    if (MessageBox.Show(String.Format("Are you sure you want to STOP {0} ?", activeTabPage.Text),
                        String.Format("STOP {0}", activeTabPage.Text), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        bgWorker.CancelAsync();
                        UpdateUIStatus();
                        UpdateServerStatusFlag(activeTabPage, false);
                    }
                }
                else
                {
                    if (MessageBox.Show(String.Format("Are you sure you want to START {0} ?", activeTabPage.Text),
                        String.Format("START {0}", activeTabPage.Text), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        //QuickTellerDisbursement
                        RunServiceOpperation(activeTabPage);
                        UpdateUIStatus();
                        UpdateServerStatusFlag(activeTabPage, true);

                    }
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private bool UpdateServerStatusFlag(TabPage activeTabPage, bool status)
        {
            try
            {
                switch (activeTabPage.Name)
                {
                    //case "tabQueuedDisburse":
                    //    //QueuedDisbursement
                    //    textBoxQueuedDisburse.BackColor = status ? Color.Black : Color.Red;
                    //    return queuedDisburseFlag = status;
                    //case "tabVaultCodeGen":
                    //    //VaultCode Generation
                    //    textBoxVaultCodeGen.BackColor = status ? Color.Black : Color.Red;
                    //    return vaultCodeGenFlag = status;
                    //case "tabNotification":
                    //    //Notification
                    //    textBoxNotification.BackColor = status ? Color.Black : Color.Red;
                    //    return notificationServiceFlag = status;
                    //case "tabThrottle1":
                    //    //Throttle1
                    //    textBoxThrottle1.BackColor = status ? Color.Black : Color.Red;
                    //    return throttle1Flag = status;
                    //case "tabThrottle2":
                    //    //Throttle2
                    //    textBoxThrottle2.BackColor = status ? Color.Black : Color.Red;
                    //    return throttle2Flag = status;
                    //case "tabQTDisburse":
                    //    //QuickTellerDisbursement
                    //    textBoxQTDisbursement.BackColor = status ? Color.Black : Color.Red;
                    //    return qtDisburseFlag = status;
                    //case "tabPageNameEnquiry":
                    //    //Name INquiry
                    //    textBoxNameInquiry.BackColor = status ? Color.Black : Color.Red;
                    //    return nameEnqueryFlag = status;
                    //case "tabPageSelfService":
                    //    //SelfService
                    //    textBoxSelfService.BackColor = status ? Color.Black : Color.Red;
                    //    return selfServiceFlag = status;
                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void UpdateServerStatusControls(BackgroundWorker bgWorker, string Service, string activeTabName)
        {
            //try
            //{
            //    labelActiveServiceStatus.Text = bgWorker.IsBusy ? "Running" : "Stopped";
            //    labelActiveService.Text = Service;
            //    labelAutoStart.Text = getServiceAutoStartMode(activeTabName);
            //    switch (bgWorker.IsBusy)
            //    {
            //        case false:
            //            buttonRunService.Image = Image.FromFile(ConfigurationManager.AppSettings["StartImage"]);
            //            break;
            //        case true:
            //            buttonRunService.Image = Image.FromFile(ConfigurationManager.AppSettings["StopImage"]);
            //            break;
            //    }
            //}
            //catch (Exception exp)
            //{
            //    throw exp;
            //}
        }

        private string getServiceAutoStartMode(string activeTabName)
        {
            try
            {
                switch (activeTabName)
                {
                    //case "tabQueuedDisburse":
                    //    return queuedDisburseFlag == true ? "ON" : "OFF";
                    //case "tabVaultCodeGen":
                    //    return vaultCodeGenFlag == true ? "ON" : "OFF";
                    //case "tabNotification":
                    //    return notificationServiceFlag == true ? "ON" : "OFF";
                    //case "tabThrottle1":
                    //    return throttle1Flag == true ? "ON" : "OFF";
                    //case "tabThrottle2":
                    //    return throttle2Flag == true ? "ON" : "OFF";
                    //case "tabQTDisburse":
                    //    return qtDisburseFlag == true ? "ON" : "OFF";
                    //case "tabPageNameEnquiry":
                    //    return nameEnqueryFlag == true ? "ON" : "OFF";
                    //case "tabPageSelfService":
                    //    return selfServiceFlag == true ? "ON" : "OFF";
                    default:
                        return null;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private BackgroundWorker RunServiceOpperation(TabPage activeTabPage)
        {
            try
            {
                switch (activeTabPage.Name)
                {
                    case "tabNotification":
                        //Notification
                        RunServiceOpperation(textBoxNotification, notificationService.bgWorkerLog, notificationService.bgWorker);
                        return notificationService.bgWorker;
                    case "tabEUploadImporter":
                        //Notification
                        RunServiceOpperation(textBoxErrorUploadImporter, errorUploadImporter.bgWorkerLog, errorUploadImporter.bgWorker);
                        return notificationService.bgWorker;

                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private BackgroundWorker getActiveTabBackgroudWorker(String activeTabPage)
        {
            try
            {
                switch (activeTabPage)
                {
                    case "tabNotification":
                        return notificationService.bgWorker;
                    case "tabEUploadImporter":
                        return errorUploadImporter.bgWorker;

                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void tabServiceMonitor_Selected(object sender, TabControlEventArgs e)
        {
            UpdateUIStatus();
        }
    }
}
