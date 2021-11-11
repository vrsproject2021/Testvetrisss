namespace VETRISSchedulerInstaller
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
            this.pnlUC = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblVer = new System.Windows.Forms.Label();
            this.lblSetupDesc = new System.Windows.Forms.Label();
            this.lblSetup = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUC
            // 
            this.pnlUC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUC.Location = new System.Drawing.Point(0, 75);
            this.pnlUC.Name = "pnlUC";
            this.pnlUC.Size = new System.Drawing.Size(722, 292);
            this.pnlUC.TabIndex = 25;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlHeader.Controls.Add(this.lblVer);
            this.pnlHeader.Controls.Add(this.lblSetupDesc);
            this.pnlHeader.Controls.Add(this.lblSetup);
            this.pnlHeader.Controls.Add(this.pbLogo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(722, 75);
            this.pnlHeader.TabIndex = 24;
            // 
            // lblVer
            // 
            this.lblVer.AutoSize = true;
            this.lblVer.Location = new System.Drawing.Point(557, 23);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(42, 13);
            this.lblVer.TabIndex = 4;
            this.lblVer.Text = "Version";
            // 
            // lblSetupDesc
            // 
            this.lblSetupDesc.AutoSize = true;
            this.lblSetupDesc.Location = new System.Drawing.Point(260, 46);
            this.lblSetupDesc.Name = "lblSetupDesc";
            this.lblSetupDesc.Size = new System.Drawing.Size(362, 13);
            this.lblSetupDesc.TabIndex = 3;
            this.lblSetupDesc.Text = "Sets up a new instance/Updates the new patches/Uninstall the application";
            // 
            // lblSetup
            // 
            this.lblSetup.AutoSize = true;
            this.lblSetup.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSetup.Location = new System.Drawing.Point(259, 13);
            this.lblSetup.Name = "lblSetup";
            this.lblSetup.Size = new System.Drawing.Size(292, 23);
            this.lblSetup.TabIndex = 2;
            this.lblSetup.Text = "VETRIS Scheduler Services Setup";
            // 
            // pbLogo
            // 
            this.pbLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbLogo.BackgroundImage")));
            this.pbLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbLogo.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbLogo.InitialImage")));
            this.pbLogo.Location = new System.Drawing.Point(8, 10);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(231, 49);
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(722, 367);
            this.Controls.Add(this.pnlUC);
            this.Controls.Add(this.pnlHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VETRIS Scheduler Services Installer";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlUC;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblVer;
        private System.Windows.Forms.Label lblSetupDesc;
        private System.Windows.Forms.Label lblSetup;
        private System.Windows.Forms.PictureBox pbLogo;
    }
}

