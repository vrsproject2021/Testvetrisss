namespace DICOMRouterInstaller.UserControls
{
    partial class ucInstallWiz3
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
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpRec = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRCVPORTNO = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRCVAETITLE = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkCompFile = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkAdminSC = new System.Windows.Forms.CheckBox();
            this.chkFUSC = new System.Windows.Forms.CheckBox();
            this.chkArch = new System.Windows.Forms.CheckBox();
            this.grpRec.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(511, 2);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 17);
            this.label8.TabIndex = 109;
            this.label8.Text = "(* Mandatory)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 24);
            this.label1.TabIndex = 108;
            this.label1.Text = "Specify Server Details";
            // 
            // grpRec
            // 
            this.grpRec.Controls.Add(this.label4);
            this.grpRec.Controls.Add(this.label6);
            this.grpRec.Controls.Add(this.txtRCVPORTNO);
            this.grpRec.Controls.Add(this.label3);
            this.grpRec.Controls.Add(this.txtRCVAETITLE);
            this.grpRec.Controls.Add(this.label2);
            this.grpRec.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRec.Location = new System.Drawing.Point(35, 42);
            this.grpRec.Margin = new System.Windows.Forms.Padding(4);
            this.grpRec.Name = "grpRec";
            this.grpRec.Padding = new System.Windows.Forms.Padding(4);
            this.grpRec.Size = new System.Drawing.Size(907, 118);
            this.grpRec.TabIndex = 1;
            this.grpRec.TabStop = false;
            this.grpRec.Text = "Receiver (PACS)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(405, 37);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 20);
            this.label4.TabIndex = 106;
            this.label4.Text = "*";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(107, 37);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 20);
            this.label6.TabIndex = 105;
            this.label6.Text = "*";
            // 
            // txtRCVPORTNO
            // 
            this.txtRCVPORTNO.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRCVPORTNO.ForeColor = System.Drawing.Color.Black;
            this.txtRCVPORTNO.Location = new System.Drawing.Point(291, 68);
            this.txtRCVPORTNO.Margin = new System.Windows.Forms.Padding(4);
            this.txtRCVPORTNO.MaxLength = 50;
            this.txtRCVPORTNO.Name = "txtRCVPORTNO";
            this.txtRCVPORTNO.Size = new System.Drawing.Size(195, 26);
            this.txtRCVPORTNO.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(287, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 20);
            this.label3.TabIndex = 86;
            this.label3.Text = "Port Number";
            // 
            // txtRCVAETITLE
            // 
            this.txtRCVAETITLE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRCVAETITLE.ForeColor = System.Drawing.Color.Black;
            this.txtRCVAETITLE.Location = new System.Drawing.Point(23, 68);
            this.txtRCVAETITLE.Margin = new System.Windows.Forms.Padding(4);
            this.txtRCVAETITLE.MaxLength = 50;
            this.txtRCVAETITLE.Name = "txtRCVAETITLE";
            this.txtRCVAETITLE.Size = new System.Drawing.Size(195, 26);
            this.txtRCVAETITLE.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(25, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 20);
            this.label2.TabIndex = 84;
            this.label2.Text = "AE Title";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 525);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1029, 62);
            this.panel1.TabIndex = 3;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.Gainsboro;
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.Color.Black;
            this.btnPrev.Location = new System.Drawing.Point(696, 18);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(100, 28);
            this.btnPrev.TabIndex = 3;
            this.btnPrev.Text = "&Previous";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Gainsboro;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(912, 18);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Gainsboro;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.Black;
            this.btnNext.Location = new System.Drawing.Point(804, 18);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(100, 28);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "&Next";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkArch);
            this.groupBox1.Controls.Add(this.chkCompFile);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtURL);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(35, 176);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(907, 179);
            this.groupBox1.TabIndex = 110;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "VETRIS";
            // 
            // chkCompFile
            // 
            this.chkCompFile.AutoSize = true;
            this.chkCompFile.Checked = true;
            this.chkCompFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCompFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCompFile.Location = new System.Drawing.Point(23, 112);
            this.chkCompFile.Margin = new System.Windows.Forms.Padding(4);
            this.chkCompFile.Name = "chkCompFile";
            this.chkCompFile.Size = new System.Drawing.Size(319, 24);
            this.chkCompFile.TabIndex = 117;
            this.chkCompFile.Text = "Compress files while sending to PACS";
            this.chkCompFile.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(71, 37);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 20);
            this.label7.TabIndex = 105;
            this.label7.Text = "*";
            // 
            // txtURL
            // 
            this.txtURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtURL.ForeColor = System.Drawing.Color.Black;
            this.txtURL.Location = new System.Drawing.Point(23, 68);
            this.txtURL.Margin = new System.Windows.Forms.Padding(4);
            this.txtURL.MaxLength = 200;
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(845, 26);
            this.txtURL.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(25, 37);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 20);
            this.label10.TabIndex = 84;
            this.label10.Text = "URL";
            // 
            // chkAdminSC
            // 
            this.chkAdminSC.AutoSize = true;
            this.chkAdminSC.Checked = true;
            this.chkAdminSC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdminSC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAdminSC.Location = new System.Drawing.Point(55, 399);
            this.chkAdminSC.Margin = new System.Windows.Forms.Padding(4);
            this.chkAdminSC.Name = "chkAdminSC";
            this.chkAdminSC.Size = new System.Drawing.Size(295, 24);
            this.chkAdminSC.TabIndex = 116;
            this.chkAdminSC.Text = "Create Admin Shortcut On Desktop";
            this.chkAdminSC.UseVisualStyleBackColor = true;
            // 
            // chkFUSC
            // 
            this.chkFUSC.AutoSize = true;
            this.chkFUSC.Checked = true;
            this.chkFUSC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFUSC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFUSC.Location = new System.Drawing.Point(55, 442);
            this.chkFUSC.Margin = new System.Windows.Forms.Padding(4);
            this.chkFUSC.Name = "chkFUSC";
            this.chkFUSC.Size = new System.Drawing.Size(332, 24);
            this.chkFUSC.TabIndex = 117;
            this.chkFUSC.Text = "Create File Upload Shortcut On Desktop";
            this.chkFUSC.UseVisualStyleBackColor = true;
            // 
            // chkArch
            // 
            this.chkArch.AutoSize = true;
            this.chkArch.Checked = true;
            this.chkArch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkArch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkArch.Location = new System.Drawing.Point(23, 144);
            this.chkArch.Margin = new System.Windows.Forms.Padding(4);
            this.chkArch.Name = "chkArch";
            this.chkArch.Size = new System.Drawing.Size(298, 24);
            this.chkArch.TabIndex = 118;
            this.chkArch.Text = "Archive files while sending to PACS";
            this.chkArch.UseVisualStyleBackColor = true;
            // 
            // ucInstallWiz3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkFUSC);
            this.Controls.Add(this.chkAdminSC);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpRec);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucInstallWiz3";
            this.Size = new System.Drawing.Size(1029, 587);
            this.Load += new System.EventHandler(this.ucInstallWiz3_Load);
            this.grpRec.ResumeLayout(false);
            this.grpRec.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpRec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRCVPORTNO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRCVAETITLE;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkAdminSC;
        private System.Windows.Forms.CheckBox chkFUSC;
        private System.Windows.Forms.CheckBox chkCompFile;
        private System.Windows.Forms.CheckBox chkArch;
    }
}
