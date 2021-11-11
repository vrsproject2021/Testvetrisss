namespace VETRISAccountsScheduler.UserControls
{
    partial class ucService
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucService));
            this.lblASProcess = new System.Windows.Forms.Label();
            this.btnASStop = new System.Windows.Forms.Button();
            this.btnASStart = new System.Windows.Forms.Button();
            this.lblASStatus = new System.Windows.Forms.Label();
            this.lblASDetails = new System.Windows.Forms.Label();
            this.lblASServices = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.timerBAUpdate = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblASProcess
            // 
            this.lblASProcess.AutoSize = true;
            this.lblASProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblASProcess.ForeColor = System.Drawing.Color.Gray;
            this.lblASProcess.Location = new System.Drawing.Point(4, 84);
            this.lblASProcess.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblASProcess.Name = "lblASProcess";
            this.lblASProcess.Size = new System.Drawing.Size(20, 17);
            this.lblASProcess.TabIndex = 101;
            this.lblASProcess.Text = "...";
            this.lblASProcess.Visible = false;
            // 
            // btnASStop
            // 
            this.btnASStop.BackColor = System.Drawing.Color.Red;
            this.btnASStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnASStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnASStop.FlatAppearance.BorderSize = 0;
            this.btnASStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnASStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnASStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnASStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnASStop.ForeColor = System.Drawing.Color.White;
            this.btnASStop.Image = ((System.Drawing.Image)(resources.GetObject("btnASStop.Image")));
            this.btnASStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnASStop.Location = new System.Drawing.Point(1112, 31);
            this.btnASStop.Margin = new System.Windows.Forms.Padding(5);
            this.btnASStop.Name = "btnASStop";
            this.btnASStop.Size = new System.Drawing.Size(100, 64);
            this.btnASStop.TabIndex = 100;
            this.btnASStop.Text = "Stop";
            this.btnASStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnASStop.UseVisualStyleBackColor = false;
            this.btnASStop.Visible = false;
            this.btnASStop.Click += new System.EventHandler(this.btnASStop_Click);
            // 
            // btnASStart
            // 
            this.btnASStart.BackColor = System.Drawing.Color.LimeGreen;
            this.btnASStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnASStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnASStart.FlatAppearance.BorderSize = 0;
            this.btnASStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.btnASStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnASStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnASStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnASStart.ForeColor = System.Drawing.Color.Black;
            this.btnASStart.Image = ((System.Drawing.Image)(resources.GetObject("btnASStart.Image")));
            this.btnASStart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnASStart.Location = new System.Drawing.Point(998, 31);
            this.btnASStart.Margin = new System.Windows.Forms.Padding(5);
            this.btnASStart.Name = "btnASStart";
            this.btnASStart.Size = new System.Drawing.Size(100, 64);
            this.btnASStart.TabIndex = 99;
            this.btnASStart.Text = "Start";
            this.btnASStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnASStart.UseVisualStyleBackColor = false;
            this.btnASStart.Click += new System.EventHandler(this.btnASStart_Click);
            // 
            // lblASStatus
            // 
            this.lblASStatus.AutoSize = true;
            this.lblASStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblASStatus.ForeColor = System.Drawing.Color.Red;
            this.lblASStatus.Location = new System.Drawing.Point(499, 26);
            this.lblASStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblASStatus.Name = "lblASStatus";
            this.lblASStatus.Size = new System.Drawing.Size(80, 17);
            this.lblASStatus.TabIndex = 98;
            this.lblASStatus.Text = "(Stopped)";
            // 
            // lblASDetails
            // 
            this.lblASDetails.AutoSize = true;
            this.lblASDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblASDetails.ForeColor = System.Drawing.Color.Gray;
            this.lblASDetails.Location = new System.Drawing.Point(4, 63);
            this.lblASDetails.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblASDetails.Name = "lblASDetails";
            this.lblASDetails.Size = new System.Drawing.Size(968, 17);
            this.lblASDetails.TabIndex = 97;
            this.lblASDetails.Text = "Updates/posts financial account receivable related data from VETRIS to QuickBooks" +
    ". If the application is stopped, the updates/posting of data is stopped";
            // 
            // lblASServices
            // 
            this.lblASServices.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblASServices.ForeColor = System.Drawing.Color.Gray;
            this.lblASServices.Image = ((System.Drawing.Image)(resources.GetObject("lblASServices.Image")));
            this.lblASServices.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblASServices.Location = new System.Drawing.Point(4, 16);
            this.lblASServices.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblASServices.Name = "lblASServices";
            this.lblASServices.Size = new System.Drawing.Size(317, 47);
            this.lblASServices.TabIndex = 96;
            this.lblASServices.Text = "Account Receivables";
            this.lblASServices.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Red;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnClose.Location = new System.Drawing.Point(744, 416);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 42);
            this.btnClose.TabIndex = 102;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // timerBAUpdate
            // 
            this.timerBAUpdate.Interval = 180000;
            this.timerBAUpdate.Tick += new System.EventHandler(this.timerBAUpdate_Tick);
            // 
            // ucService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblASProcess);
            this.Controls.Add(this.btnASStop);
            this.Controls.Add(this.btnASStart);
            this.Controls.Add(this.lblASStatus);
            this.Controls.Add(this.lblASDetails);
            this.Controls.Add(this.lblASServices);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucService";
            this.Size = new System.Drawing.Size(1844, 775);
            this.Load += new System.EventHandler(this.ucService_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblASProcess;
        private System.Windows.Forms.Button btnASStop;
        private System.Windows.Forms.Button btnASStart;
        private System.Windows.Forms.Label lblASStatus;
        private System.Windows.Forms.Label lblASDetails;
        private System.Windows.Forms.Label lblASServices;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Timer timerBAUpdate;
    }
}
