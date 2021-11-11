namespace VETRIS_DICOM_ROUTER_ADMIN.UserControls
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucService));
            this.lblDRProcess = new System.Windows.Forms.Label();
            this.btnDRStop = new System.Windows.Forms.Button();
            this.btnDRStart = new System.Windows.Forms.Button();
            this.lblDRStatus = new System.Windows.Forms.Label();
            this.lblDRDetails = new System.Windows.Forms.Label();
            this.lblDRServices = new System.Windows.Forms.Label();
            this.btnDSStop = new System.Windows.Forms.Button();
            this.btnDSStart = new System.Windows.Forms.Button();
            this.lblDSProcess = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblDSStatus = new System.Windows.Forms.Label();
            this.lblDSDetails = new System.Windows.Forms.Label();
            this.lblDSServices = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDRProcess
            // 
            this.lblDRProcess.AutoSize = true;
            this.lblDRProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDRProcess.ForeColor = System.Drawing.Color.Gray;
            this.lblDRProcess.Location = new System.Drawing.Point(33, 101);
            this.lblDRProcess.Name = "lblDRProcess";
            this.lblDRProcess.Size = new System.Drawing.Size(16, 13);
            this.lblDRProcess.TabIndex = 121;
            this.lblDRProcess.Text = "...";
            this.lblDRProcess.Visible = false;
            // 
            // btnDRStop
            // 
            this.btnDRStop.BackColor = System.Drawing.Color.Red;
            this.btnDRStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDRStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDRStop.FlatAppearance.BorderSize = 0;
            this.btnDRStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnDRStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnDRStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDRStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDRStop.ForeColor = System.Drawing.Color.White;
            this.btnDRStop.Image = ((System.Drawing.Image)(resources.GetObject("btnDRStop.Image")));
            this.btnDRStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDRStop.Location = new System.Drawing.Point(645, 40);
            this.btnDRStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnDRStop.Name = "btnDRStop";
            this.btnDRStop.Size = new System.Drawing.Size(75, 62);
            this.btnDRStop.TabIndex = 120;
            this.btnDRStop.Text = "Stop";
            this.btnDRStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDRStop.UseVisualStyleBackColor = false;
            this.btnDRStop.Click += new System.EventHandler(this.btnDRStop_Click);
            // 
            // btnDRStart
            // 
            this.btnDRStart.BackColor = System.Drawing.Color.LimeGreen;
            this.btnDRStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDRStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDRStart.FlatAppearance.BorderSize = 0;
            this.btnDRStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.btnDRStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnDRStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDRStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDRStart.ForeColor = System.Drawing.Color.Black;
            this.btnDRStart.Image = ((System.Drawing.Image)(resources.GetObject("btnDRStart.Image")));
            this.btnDRStart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDRStart.Location = new System.Drawing.Point(562, 40);
            this.btnDRStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnDRStart.Name = "btnDRStart";
            this.btnDRStart.Size = new System.Drawing.Size(75, 62);
            this.btnDRStart.TabIndex = 119;
            this.btnDRStart.Text = "Start";
            this.btnDRStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDRStart.UseVisualStyleBackColor = false;
            this.btnDRStart.Click += new System.EventHandler(this.btnDRStart_Click);
            // 
            // lblDRStatus
            // 
            this.lblDRStatus.AutoSize = true;
            this.lblDRStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDRStatus.ForeColor = System.Drawing.Color.Blue;
            this.lblDRStatus.Location = new System.Drawing.Point(397, 48);
            this.lblDRStatus.Name = "lblDRStatus";
            this.lblDRStatus.Size = new System.Drawing.Size(62, 13);
            this.lblDRStatus.TabIndex = 118;
            this.lblDRStatus.Text = "(Stopped)";
            // 
            // lblDRDetails
            // 
            this.lblDRDetails.AutoSize = true;
            this.lblDRDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDRDetails.ForeColor = System.Drawing.Color.Gray;
            this.lblDRDetails.Location = new System.Drawing.Point(27, 80);
            this.lblDRDetails.Name = "lblDRDetails";
            this.lblDRDetails.Size = new System.Drawing.Size(524, 13);
            this.lblDRDetails.TabIndex = 117;
            this.lblDRDetails.Text = "Receives Dicom Images from remote system. If this service is stopped, listening f" +
    "or incoming images also stops";
            // 
            // lblDRServices
            // 
            this.lblDRServices.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDRServices.ForeColor = System.Drawing.Color.Gray;
            this.lblDRServices.Image = ((System.Drawing.Image)(resources.GetObject("lblDRServices.Image")));
            this.lblDRServices.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDRServices.Location = new System.Drawing.Point(26, 34);
            this.lblDRServices.Name = "lblDRServices";
            this.lblDRServices.Size = new System.Drawing.Size(244, 38);
            this.lblDRServices.TabIndex = 116;
            this.lblDRServices.Text = "Dicom Receiving Service";
            this.lblDRServices.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDSStop
            // 
            this.btnDSStop.BackColor = System.Drawing.Color.Red;
            this.btnDSStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDSStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDSStop.FlatAppearance.BorderSize = 0;
            this.btnDSStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnDSStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnDSStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDSStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDSStop.ForeColor = System.Drawing.Color.White;
            this.btnDSStop.Image = ((System.Drawing.Image)(resources.GetObject("btnDSStop.Image")));
            this.btnDSStop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDSStop.Location = new System.Drawing.Point(645, 139);
            this.btnDSStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnDSStop.Name = "btnDSStop";
            this.btnDSStop.Size = new System.Drawing.Size(75, 62);
            this.btnDSStop.TabIndex = 115;
            this.btnDSStop.Text = "Stop";
            this.btnDSStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDSStop.UseVisualStyleBackColor = false;
            this.btnDSStop.Click += new System.EventHandler(this.btnDSStop_Click);
            // 
            // btnDSStart
            // 
            this.btnDSStart.BackColor = System.Drawing.Color.LimeGreen;
            this.btnDSStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDSStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDSStart.FlatAppearance.BorderSize = 0;
            this.btnDSStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.btnDSStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnDSStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDSStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDSStart.ForeColor = System.Drawing.Color.Black;
            this.btnDSStart.Image = ((System.Drawing.Image)(resources.GetObject("btnDSStart.Image")));
            this.btnDSStart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDSStart.Location = new System.Drawing.Point(562, 139);
            this.btnDSStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnDSStart.Name = "btnDSStart";
            this.btnDSStart.Size = new System.Drawing.Size(75, 62);
            this.btnDSStart.TabIndex = 114;
            this.btnDSStart.Text = "Start";
            this.btnDSStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDSStart.UseVisualStyleBackColor = false;
            this.btnDSStart.Click += new System.EventHandler(this.btnDSStart_Click);
            // 
            // lblDSProcess
            // 
            this.lblDSProcess.AutoSize = true;
            this.lblDSProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDSProcess.ForeColor = System.Drawing.Color.Gray;
            this.lblDSProcess.Location = new System.Drawing.Point(32, 197);
            this.lblDSProcess.Name = "lblDSProcess";
            this.lblDSProcess.Size = new System.Drawing.Size(16, 13);
            this.lblDSProcess.TabIndex = 113;
            this.lblDSProcess.Text = "...";
            this.lblDSProcess.Visible = false;
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
            this.btnClose.Location = new System.Drawing.Point(359, 476);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 34);
            this.btnClose.TabIndex = 112;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblDSStatus
            // 
            this.lblDSStatus.AutoSize = true;
            this.lblDSStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDSStatus.ForeColor = System.Drawing.Color.Blue;
            this.lblDSStatus.Location = new System.Drawing.Point(397, 153);
            this.lblDSStatus.Name = "lblDSStatus";
            this.lblDSStatus.Size = new System.Drawing.Size(62, 13);
            this.lblDSStatus.TabIndex = 111;
            this.lblDSStatus.Text = "(Stopped)";
            // 
            // lblDSDetails
            // 
            this.lblDSDetails.AutoSize = true;
            this.lblDSDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDSDetails.ForeColor = System.Drawing.Color.Gray;
            this.lblDSDetails.Location = new System.Drawing.Point(26, 183);
            this.lblDSDetails.Name = "lblDSDetails";
            this.lblDSDetails.Size = new System.Drawing.Size(512, 13);
            this.lblDSDetails.TabIndex = 110;
            this.lblDSDetails.Text = "Send Dicom Images to PACS. If this service is stopped, image transfer from local " +
    "system to PACS also stops";
            // 
            // lblDSServices
            // 
            this.lblDSServices.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDSServices.ForeColor = System.Drawing.Color.Gray;
            this.lblDSServices.Image = ((System.Drawing.Image)(resources.GetObject("lblDSServices.Image")));
            this.lblDSServices.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDSServices.Location = new System.Drawing.Point(26, 139);
            this.lblDSServices.Name = "lblDSServices";
            this.lblDSServices.Size = new System.Drawing.Size(232, 38);
            this.lblDSServices.TabIndex = 109;
            this.lblDSServices.Text = "Dicom Sending Service";
            this.lblDSServices.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblDRProcess);
            this.Controls.Add(this.btnDRStop);
            this.Controls.Add(this.btnDRStart);
            this.Controls.Add(this.lblDRStatus);
            this.Controls.Add(this.lblDRDetails);
            this.Controls.Add(this.lblDRServices);
            this.Controls.Add(this.btnDSStop);
            this.Controls.Add(this.btnDSStart);
            this.Controls.Add(this.lblDSProcess);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblDSStatus);
            this.Controls.Add(this.lblDSDetails);
            this.Controls.Add(this.lblDSServices);
            this.Name = "ucService";
            this.Size = new System.Drawing.Size(921, 555);
            this.Load += new System.EventHandler(this.ucService_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDRProcess;
        private System.Windows.Forms.Button btnDRStop;
        private System.Windows.Forms.Button btnDRStart;
        private System.Windows.Forms.Label lblDRStatus;
        private System.Windows.Forms.Label lblDRDetails;
        private System.Windows.Forms.Label lblDRServices;
        private System.Windows.Forms.Button btnDSStop;
        private System.Windows.Forms.Button btnDSStart;
        private System.Windows.Forms.Label lblDSProcess;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblDSStatus;
        private System.Windows.Forms.Label lblDSDetails;
        private System.Windows.Forms.Label lblDSServices;
    }
}
