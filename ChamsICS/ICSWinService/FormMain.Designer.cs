namespace ICSWinService
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxAppLogDirTree = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabServiceMonitor = new System.Windows.Forms.TabControl();
            this.tabNotification = new System.Windows.Forms.TabPage();
            this.textBoxNotification = new System.Windows.Forms.RichTextBox();
            this.tabLogViewer = new System.Windows.Forms.TabPage();
            this.textBoxLogViewer = new System.Windows.Forms.RichTextBox();
            this.tabEUploadImporter = new System.Windows.Forms.TabPage();
            this.tabEUploadResolver = new System.Windows.Forms.TabPage();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.textBoxErrorUploadImporter = new System.Windows.Forms.RichTextBox();
            this.textBoxErrorUploadResolver = new System.Windows.Forms.RichTextBox();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxAppLogDirTree.SuspendLayout();
            this.tabServiceMonitor.SuspendLayout();
            this.tabNotification.SuspendLayout();
            this.tabLogViewer.SuspendLayout();
            this.tabEUploadImporter.SuspendLayout();
            this.tabEUploadResolver.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 5;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(171, 22);
            this.toolStripMenuItem1.Text = "Open Log Folder...";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(168, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxAppLogDirTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabServiceMonitor);
            this.splitContainer1.Size = new System.Drawing.Size(784, 509);
            this.splitContainer1.SplitterDistance = 302;
            this.splitContainer1.TabIndex = 7;
            // 
            // groupBoxAppLogDirTree
            // 
            this.groupBoxAppLogDirTree.Controls.Add(this.treeView1);
            this.groupBoxAppLogDirTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxAppLogDirTree.Location = new System.Drawing.Point(0, 0);
            this.groupBoxAppLogDirTree.Name = "groupBoxAppLogDirTree";
            this.groupBoxAppLogDirTree.Size = new System.Drawing.Size(302, 509);
            this.groupBoxAppLogDirTree.TabIndex = 0;
            this.groupBoxAppLogDirTree.TabStop = false;
            this.groupBoxAppLogDirTree.Text = "Logs";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 16);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(296, 490);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // tabServiceMonitor
            // 
            this.tabServiceMonitor.Controls.Add(this.tabNotification);
            this.tabServiceMonitor.Controls.Add(this.tabLogViewer);
            this.tabServiceMonitor.Controls.Add(this.tabEUploadImporter);
            this.tabServiceMonitor.Controls.Add(this.tabEUploadResolver);
            this.tabServiceMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabServiceMonitor.Location = new System.Drawing.Point(0, 0);
            this.tabServiceMonitor.Multiline = true;
            this.tabServiceMonitor.Name = "tabServiceMonitor";
            this.tabServiceMonitor.SelectedIndex = 0;
            this.tabServiceMonitor.Size = new System.Drawing.Size(478, 509);
            this.tabServiceMonitor.TabIndex = 1;
            // 
            // tabNotification
            // 
            this.tabNotification.Controls.Add(this.textBoxNotification);
            this.tabNotification.Location = new System.Drawing.Point(4, 22);
            this.tabNotification.Name = "tabNotification";
            this.tabNotification.Padding = new System.Windows.Forms.Padding(3);
            this.tabNotification.Size = new System.Drawing.Size(470, 483);
            this.tabNotification.TabIndex = 9;
            this.tabNotification.Text = "Service Console";
            this.tabNotification.UseVisualStyleBackColor = true;
            // 
            // textBoxNotification
            // 
            this.textBoxNotification.BackColor = System.Drawing.Color.Black;
            this.textBoxNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNotification.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNotification.ForeColor = System.Drawing.Color.White;
            this.textBoxNotification.Location = new System.Drawing.Point(3, 3);
            this.textBoxNotification.Name = "textBoxNotification";
            this.textBoxNotification.Size = new System.Drawing.Size(464, 477);
            this.textBoxNotification.TabIndex = 1;
            this.textBoxNotification.Text = "";
            // 
            // tabLogViewer
            // 
            this.tabLogViewer.Controls.Add(this.textBoxLogViewer);
            this.tabLogViewer.Location = new System.Drawing.Point(4, 22);
            this.tabLogViewer.Name = "tabLogViewer";
            this.tabLogViewer.Size = new System.Drawing.Size(470, 483);
            this.tabLogViewer.TabIndex = 10;
            this.tabLogViewer.Text = "Log Viewer";
            this.tabLogViewer.UseVisualStyleBackColor = true;
            // 
            // textBoxLogViewer
            // 
            this.textBoxLogViewer.BackColor = System.Drawing.Color.Black;
            this.textBoxLogViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogViewer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLogViewer.ForeColor = System.Drawing.Color.White;
            this.textBoxLogViewer.Location = new System.Drawing.Point(0, 0);
            this.textBoxLogViewer.Name = "textBoxLogViewer";
            this.textBoxLogViewer.Size = new System.Drawing.Size(470, 483);
            this.textBoxLogViewer.TabIndex = 2;
            this.textBoxLogViewer.Text = "";
            // 
            // tabEUploadImporter
            // 
            this.tabEUploadImporter.Controls.Add(this.textBoxErrorUploadImporter);
            this.tabEUploadImporter.Location = new System.Drawing.Point(4, 22);
            this.tabEUploadImporter.Name = "tabEUploadImporter";
            this.tabEUploadImporter.Padding = new System.Windows.Forms.Padding(3);
            this.tabEUploadImporter.Size = new System.Drawing.Size(470, 483);
            this.tabEUploadImporter.TabIndex = 11;
            this.tabEUploadImporter.Text = "Error Upload Importer";
            this.tabEUploadImporter.UseVisualStyleBackColor = true;
            // 
            // tabEUploadResolver
            // 
            this.tabEUploadResolver.Controls.Add(this.textBoxErrorUploadResolver);
            this.tabEUploadResolver.Location = new System.Drawing.Point(4, 22);
            this.tabEUploadResolver.Name = "tabEUploadResolver";
            this.tabEUploadResolver.Padding = new System.Windows.Forms.Padding(3);
            this.tabEUploadResolver.Size = new System.Drawing.Size(470, 483);
            this.tabEUploadResolver.TabIndex = 12;
            this.tabEUploadResolver.Text = "Error Upload Resolver";
            this.tabEUploadResolver.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 540);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 6;
            this.statusStrip.Text = "statusStrip1";
            // 
            // textBoxErrorUploadImporter
            // 
            this.textBoxErrorUploadImporter.BackColor = System.Drawing.Color.Black;
            this.textBoxErrorUploadImporter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxErrorUploadImporter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxErrorUploadImporter.ForeColor = System.Drawing.Color.White;
            this.textBoxErrorUploadImporter.Location = new System.Drawing.Point(3, 3);
            this.textBoxErrorUploadImporter.Name = "textBoxErrorUploadImporter";
            this.textBoxErrorUploadImporter.Size = new System.Drawing.Size(464, 477);
            this.textBoxErrorUploadImporter.TabIndex = 2;
            this.textBoxErrorUploadImporter.Text = "";
            // 
            // textBoxErrorUploadResolver
            // 
            this.textBoxErrorUploadResolver.BackColor = System.Drawing.Color.Black;
            this.textBoxErrorUploadResolver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxErrorUploadResolver.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxErrorUploadResolver.ForeColor = System.Drawing.Color.White;
            this.textBoxErrorUploadResolver.Location = new System.Drawing.Point(3, 3);
            this.textBoxErrorUploadResolver.Name = "textBoxErrorUploadResolver";
            this.textBoxErrorUploadResolver.Size = new System.Drawing.Size(464, 477);
            this.textBoxErrorUploadResolver.TabIndex = 3;
            this.textBoxErrorUploadResolver.Text = "";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Name = "FormMain";
            this.Text = "IGR Hub Logs & Notification Monitor";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxAppLogDirTree.ResumeLayout(false);
            this.tabServiceMonitor.ResumeLayout(false);
            this.tabNotification.ResumeLayout(false);
            this.tabLogViewer.ResumeLayout(false);
            this.tabEUploadImporter.ResumeLayout(false);
            this.tabEUploadResolver.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxAppLogDirTree;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabControl tabServiceMonitor;
        private System.Windows.Forms.TabPage tabNotification;
        private System.Windows.Forms.TabPage tabLogViewer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.RichTextBox textBoxNotification;
        private System.Windows.Forms.RichTextBox textBoxLogViewer;
        private System.Windows.Forms.TabPage tabEUploadImporter;
        private System.Windows.Forms.TabPage tabEUploadResolver;
        private System.Windows.Forms.RichTextBox textBoxErrorUploadImporter;
        private System.Windows.Forms.RichTextBox textBoxErrorUploadResolver;
    }
}

