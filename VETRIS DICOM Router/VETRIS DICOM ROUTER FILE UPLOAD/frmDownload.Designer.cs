namespace VETRIS_DICOM_ROUTER_FILE_UPLOAD
{
    partial class frmDownload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDownload));
            this.lblZipResult = new System.Windows.Forms.Label();
            this.lblPer = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblDL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblZipResult
            // 
            this.lblZipResult.AutoSize = true;
            this.lblZipResult.Location = new System.Drawing.Point(16, 68);
            this.lblZipResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblZipResult.Name = "lblZipResult";
            this.lblZipResult.Size = new System.Drawing.Size(20, 17);
            this.lblZipResult.TabIndex = 7;
            this.lblZipResult.Text = "...";
            // 
            // lblPer
            // 
            this.lblPer.AutoSize = true;
            this.lblPer.Location = new System.Drawing.Point(809, 28);
            this.lblPer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPer.Name = "lblPer";
            this.lblPer.Size = new System.Drawing.Size(12, 17);
            this.lblPer.TabIndex = 6;
            this.lblPer.Text = ".";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(16, 94);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(821, 28);
            this.progressBar.TabIndex = 4;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(16, 52);
            this.lblResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(20, 17);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "...";
            // 
            // lblDL
            // 
            this.lblDL.AutoSize = true;
            this.lblDL.Location = new System.Drawing.Point(16, 28);
            this.lblDL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDL.Name = "lblDL";
            this.lblDL.Size = new System.Drawing.Size(20, 17);
            this.lblDL.TabIndex = 8;
            this.lblDL.Text = "...";
            // 
            // frmDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(853, 138);
            this.Controls.Add(this.lblDL);
            this.Controls.Add(this.lblZipResult);
            this.Controls.Add(this.lblPer);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.progressBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDownload";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DICOM Router Setup";
            this.Load += new System.EventHandler(this.frmDownload_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblZipResult;
        private System.Windows.Forms.Label lblPer;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblDL;
    }
}