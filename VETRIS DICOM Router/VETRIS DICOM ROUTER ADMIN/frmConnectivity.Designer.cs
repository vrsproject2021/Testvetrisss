namespace VETRIS_DICOM_ROUTER_ADMIN
{
    partial class frmConnectivity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnectivity));
            this.pbxOffline = new System.Windows.Forms.PictureBox();
            this.pbxOnline = new System.Windows.Forms.PictureBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.pbxCheck = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbxOffline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOnline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxOffline
            // 
            this.pbxOffline.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbxOffline.BackgroundImage")));
            this.pbxOffline.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxOffline.InitialImage = null;
            this.pbxOffline.Location = new System.Drawing.Point(172, 3);
            this.pbxOffline.Name = "pbxOffline";
            this.pbxOffline.Size = new System.Drawing.Size(52, 37);
            this.pbxOffline.TabIndex = 8;
            this.pbxOffline.TabStop = false;
            this.pbxOffline.Visible = false;
            // 
            // pbxOnline
            // 
            this.pbxOnline.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbxOnline.BackgroundImage")));
            this.pbxOnline.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbxOnline.InitialImage = null;
            this.pbxOnline.Location = new System.Drawing.Point(114, 3);
            this.pbxOnline.Name = "pbxOnline";
            this.pbxOnline.Size = new System.Drawing.Size(52, 37);
            this.pbxOnline.TabIndex = 7;
            this.pbxOnline.TabStop = false;
            this.pbxOnline.Visible = false;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.Location = new System.Drawing.Point(70, 43);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(219, 16);
            this.lblResult.TabIndex = 6;
            this.lblResult.Text = "Checking connection...Please wait...";
            // 
            // pbxCheck
            // 
            this.pbxCheck.Image = ((System.Drawing.Image)(resources.GetObject("pbxCheck.Image")));
            this.pbxCheck.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbxCheck.InitialImage")));
            this.pbxCheck.Location = new System.Drawing.Point(12, 31);
            this.pbxCheck.Name = "pbxCheck";
            this.pbxCheck.Size = new System.Drawing.Size(52, 37);
            this.pbxCheck.TabIndex = 5;
            this.pbxCheck.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmConnectivity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(310, 92);
            this.Controls.Add(this.pbxOffline);
            this.Controls.Add(this.pbxOnline);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.pbxCheck);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConnectivity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Internet Connectivity";
            this.Load += new System.EventHandler(this.frmConnectivity_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxOffline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOnline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxCheck)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxOffline;
        private System.Windows.Forms.PictureBox pbxOnline;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.PictureBox pbxCheck;
        private System.Windows.Forms.Timer timer1;
    }
}