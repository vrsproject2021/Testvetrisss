namespace DICOMRouterInstaller.UserControls
{
    partial class ucInstallWiz5
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
            this.lblInstallResult = new System.Windows.Forms.Label();
            this.txtError = new System.Windows.Forms.TextBox();
            this.lblErr = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFinish = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInstallResult
            // 
            this.lblInstallResult.AutoSize = true;
            this.lblInstallResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstallResult.ForeColor = System.Drawing.Color.Black;
            this.lblInstallResult.Location = new System.Drawing.Point(52, 45);
            this.lblInstallResult.Name = "lblInstallResult";
            this.lblInstallResult.Size = new System.Drawing.Size(153, 18);
            this.lblInstallResult.TabIndex = 110;
            this.lblInstallResult.Text = "Installation Completed";
            // 
            // txtError
            // 
            this.txtError.BackColor = System.Drawing.Color.White;
            this.txtError.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtError.ForeColor = System.Drawing.Color.Red;
            this.txtError.Location = new System.Drawing.Point(55, 120);
            this.txtError.Multiline = true;
            this.txtError.Name = "txtError";
            this.txtError.ReadOnly = true;
            this.txtError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtError.Size = new System.Drawing.Size(684, 217);
            this.txtError.TabIndex = 111;
            this.txtError.Visible = false;
            // 
            // lblErr
            // 
            this.lblErr.AutoSize = true;
            this.lblErr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErr.ForeColor = System.Drawing.Color.Black;
            this.lblErr.Location = new System.Drawing.Point(52, 88);
            this.lblErr.Name = "lblErr";
            this.lblErr.Size = new System.Drawing.Size(82, 16);
            this.lblErr.TabIndex = 112;
            this.lblErr.Text = "Error Details";
            this.lblErr.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.btnFinish);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 427);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(772, 50);
            this.panel1.TabIndex = 113;
            // 
            // btnFinish
            // 
            this.btnFinish.BackColor = System.Drawing.Color.Gainsboro;
            this.btnFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinish.ForeColor = System.Drawing.Color.Black;
            this.btnFinish.Location = new System.Drawing.Point(684, 15);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 2;
            this.btnFinish.Text = "&Finish";
            this.btnFinish.UseVisualStyleBackColor = false;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // ucInstallWiz5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblErr);
            this.Controls.Add(this.txtError);
            this.Controls.Add(this.lblInstallResult);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.Name = "ucInstallWiz5";
            this.Size = new System.Drawing.Size(772, 477);
            this.Load += new System.EventHandler(this.ucInstallWiz5_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInstallResult;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.Label lblErr;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFinish;
    }
}
