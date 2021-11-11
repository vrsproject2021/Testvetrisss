namespace VETRISScheduler
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnViewLog = new System.Windows.Forms.Button();
            this.pnlAction = new System.Windows.Forms.Panel();
            this.pnlFns = new System.Windows.Forms.Panel();
            this.btnStartStopSvc = new System.Windows.Forms.Button();
            this.lblHdr = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlFns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSettings
            // 
            this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.ForeColor = System.Drawing.Color.MediumBlue;
            this.btnSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnSettings.Image")));
            this.btnSettings.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSettings.Location = new System.Drawing.Point(3, 167);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(4);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(128, 62);
            this.btnSettings.TabIndex = 8;
            this.btnSettings.Text = "Scheduler Settings";
            this.btnSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.MediumBlue;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(2, 246);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(128, 62);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnViewLog
            // 
            this.btnViewLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnViewLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnViewLog.FlatAppearance.BorderSize = 0;
            this.btnViewLog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnViewLog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnViewLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewLog.ForeColor = System.Drawing.Color.MediumBlue;
            this.btnViewLog.Image = ((System.Drawing.Image)(resources.GetObject("btnViewLog.Image")));
            this.btnViewLog.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnViewLog.Location = new System.Drawing.Point(3, 88);
            this.btnViewLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnViewLog.Name = "btnViewLog";
            this.btnViewLog.Size = new System.Drawing.Size(128, 62);
            this.btnViewLog.TabIndex = 6;
            this.btnViewLog.Text = "View Log";
            this.btnViewLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnViewLog.UseVisualStyleBackColor = true;
            this.btnViewLog.Click += new System.EventHandler(this.btnViewLog_Click);
            // 
            // pnlAction
            // 
            this.pnlAction.BackColor = System.Drawing.Color.White;
            this.pnlAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAction.Location = new System.Drawing.Point(151, 97);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Size = new System.Drawing.Size(639, 401);
            this.pnlAction.TabIndex = 14;
            // 
            // pnlFns
            // 
            this.pnlFns.BackColor = System.Drawing.Color.Transparent;
            this.pnlFns.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFns.BackgroundImage")));
            this.pnlFns.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlFns.Controls.Add(this.btnSettings);
            this.pnlFns.Controls.Add(this.btnExit);
            this.pnlFns.Controls.Add(this.btnViewLog);
            this.pnlFns.Controls.Add(this.btnStartStopSvc);
            this.pnlFns.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFns.Location = new System.Drawing.Point(0, 97);
            this.pnlFns.Margin = new System.Windows.Forms.Padding(4);
            this.pnlFns.Name = "pnlFns";
            this.pnlFns.Size = new System.Drawing.Size(151, 401);
            this.pnlFns.TabIndex = 13;
            // 
            // btnStartStopSvc
            // 
            this.btnStartStopSvc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStartStopSvc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartStopSvc.FlatAppearance.BorderSize = 0;
            this.btnStartStopSvc.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStartStopSvc.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStartStopSvc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStopSvc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartStopSvc.ForeColor = System.Drawing.Color.MediumBlue;
            this.btnStartStopSvc.Image = ((System.Drawing.Image)(resources.GetObject("btnStartStopSvc.Image")));
            this.btnStartStopSvc.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStartStopSvc.Location = new System.Drawing.Point(4, 9);
            this.btnStartStopSvc.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartStopSvc.Name = "btnStartStopSvc";
            this.btnStartStopSvc.Size = new System.Drawing.Size(128, 62);
            this.btnStartStopSvc.TabIndex = 0;
            this.btnStartStopSvc.Text = "Start/Stop Services";
            this.btnStartStopSvc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnStartStopSvc.UseVisualStyleBackColor = true;
            this.btnStartStopSvc.Click += new System.EventHandler(this.btnStartStopSvc_Click);
            // 
            // lblHdr
            // 
            this.lblHdr.AutoSize = true;
            this.lblHdr.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHdr.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblHdr.Location = new System.Drawing.Point(202, 34);
            this.lblHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHdr.Name = "lblHdr";
            this.lblHdr.Size = new System.Drawing.Size(332, 20);
            this.lblHdr.TabIndex = 0;
            this.lblHdr.Text = "VETRIS : Data Synch Scheduler Service";
            // 
            // pbLogo
            // 
            this.pbLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbLogo.BackgroundImage")));
            this.pbLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbLogo.InitialImage = null;
            this.pbLogo.Location = new System.Drawing.Point(9, 16);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(161, 57);
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlHeader.BackgroundImage")));
            this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlHeader.Controls.Add(this.pbLogo);
            this.pnlHeader.Controls.Add(this.lblHdr);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(790, 97);
            this.pnlHeader.TabIndex = 12;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(790, 498);
            this.Controls.Add(this.pnlAction);
            this.Controls.Add(this.pnlFns);
            this.Controls.Add(this.pnlHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "VETRIS : Data Synch Scheduler";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlFns.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnViewLog;
        private System.Windows.Forms.Panel pnlAction;
        private System.Windows.Forms.Panel pnlFns;
        private System.Windows.Forms.Button btnStartStopSvc;
        private System.Windows.Forms.Label lblHdr;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Panel pnlHeader;
    }
}

