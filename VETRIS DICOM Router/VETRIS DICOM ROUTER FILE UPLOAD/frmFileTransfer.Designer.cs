namespace VETRIS_DICOM_ROUTER_FILE_UPLOAD
{
    partial class frmFileTransfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFileTransfer));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlConfirm = new System.Windows.Forms.Panel();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlMsg = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblWait = new System.Windows.Forms.Label();
            this.pnlProc = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblProgDtls = new System.Windows.Forms.Label();
            this.lblProg = new System.Windows.Forms.Label();
            this.pbProc = new System.Windows.Forms.ProgressBar();
            this.pnlConfirm.SuspendLayout();
            this.pnlMsg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlProc.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlConfirm
            // 
            this.pnlConfirm.BackColor = System.Drawing.Color.Transparent;
            this.pnlConfirm.Controls.Add(this.btnNo);
            this.pnlConfirm.Controls.Add(this.btnYes);
            this.pnlConfirm.Controls.Add(this.label3);
            this.pnlConfirm.Location = new System.Drawing.Point(345, 9);
            this.pnlConfirm.Name = "pnlConfirm";
            this.pnlConfirm.Size = new System.Drawing.Size(340, 94);
            this.pnlConfirm.TabIndex = 6;
            this.pnlConfirm.Visible = false;
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.Red;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.Location = new System.Drawing.Point(162, 43);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 34);
            this.btnNo.TabIndex = 8;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.Green;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.Location = new System.Drawing.Point(81, 43);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 34);
            this.btnYes.TabIndex = 7;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(31, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(283, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Do you want to view the study in VETRIS now ?";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlMsg
            // 
            this.pnlMsg.BackColor = System.Drawing.Color.Transparent;
            this.pnlMsg.Controls.Add(this.pictureBox1);
            this.pnlMsg.Controls.Add(this.lblMsg);
            this.pnlMsg.Location = new System.Drawing.Point(348, 106);
            this.pnlMsg.Name = "pnlMsg";
            this.pnlMsg.Size = new System.Drawing.Size(340, 94);
            this.pnlMsg.TabIndex = 9;
            this.pnlMsg.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(26, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(27, 34);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(59, 18);
            this.lblMsg.MaximumSize = new System.Drawing.Size(90, 200);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(17, 16);
            this.lblMsg.TabIndex = 6;
            this.lblMsg.Text = "...";
            // 
            // lblWait
            // 
            this.lblWait.AutoSize = true;
            this.lblWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWait.Location = new System.Drawing.Point(23, 18);
            this.lblWait.Name = "lblWait";
            this.lblWait.Size = new System.Drawing.Size(86, 16);
            this.lblWait.TabIndex = 12;
            this.lblWait.Text = "Please wait...";
            // 
            // pnlProc
            // 
            this.pnlProc.Controls.Add(this.label1);
            this.pnlProc.Controls.Add(this.lblProgDtls);
            this.pnlProc.Controls.Add(this.lblProg);
            this.pnlProc.Controls.Add(this.pbProc);
            this.pnlProc.Controls.Add(this.lblWait);
            this.pnlProc.Location = new System.Drawing.Point(6, 6);
            this.pnlProc.Name = "pnlProc";
            this.pnlProc.Size = new System.Drawing.Size(340, 141);
            this.pnlProc.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(56, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "completed";
            // 
            // lblProgDtls
            // 
            this.lblProgDtls.AutoSize = true;
            this.lblProgDtls.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgDtls.Location = new System.Drawing.Point(23, 69);
            this.lblProgDtls.Name = "lblProgDtls";
            this.lblProgDtls.Size = new System.Drawing.Size(17, 16);
            this.lblProgDtls.TabIndex = 15;
            this.lblProgDtls.Text = "...";
            // 
            // lblProg
            // 
            this.lblProg.AutoSize = true;
            this.lblProg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProg.Location = new System.Drawing.Point(23, 45);
            this.lblProg.Name = "lblProg";
            this.lblProg.Size = new System.Drawing.Size(27, 16);
            this.lblProg.TabIndex = 14;
            this.lblProg.Text = "0%";
            // 
            // pbProc
            // 
            this.pbProc.Location = new System.Drawing.Point(23, 100);
            this.pbProc.Name = "pbProc";
            this.pbProc.Size = new System.Drawing.Size(302, 23);
            this.pbProc.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbProc.TabIndex = 13;
            // 
            // frmFileTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(350, 148);
            this.Controls.Add(this.pnlProc);
            this.Controls.Add(this.pnlConfirm);
            this.Controls.Add(this.pnlMsg);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFileTransfer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Status - Processing Of Study Images";
            this.LoadCompleted += new VETRIS_DICOM_ROUTER_FILE_UPLOAD.BaseForm.LoadCompletedEventHandler(this.frmFileTransfer_LoadCompleted);
            this.Load += new System.EventHandler(this.frmFileTransfer_Load);
            this.pnlConfirm.ResumeLayout(false);
            this.pnlConfirm.PerformLayout();
            this.pnlMsg.ResumeLayout(false);
            this.pnlMsg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlProc.ResumeLayout(false);
            this.pnlProc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel pnlConfirm;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Panel pnlMsg;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblWait;
        private System.Windows.Forms.Panel pnlProc;
        private System.Windows.Forms.Label lblProg;
        private System.Windows.Forms.ProgressBar pbProc;
        private System.Windows.Forms.Label lblProgDtls;
        private System.Windows.Forms.Label label1;

    }
}