namespace VETRISAccountsScheduler.UserControls
{
    partial class ucViewLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucViewLog));
            this.lvw_log_view = new System.Windows.Forms.ListView();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbToMin = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbToHr = new System.Windows.Forms.ComboBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnPurge = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmbFromMin = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFromHr = new System.Windows.Forms.ComboBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.pnlHdr = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.pnlHdr.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvw_log_view
            // 
            this.lvw_log_view.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvw_log_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvw_log_view.ForeColor = System.Drawing.Color.DimGray;
            this.lvw_log_view.FullRowSelect = true;
            this.lvw_log_view.Location = new System.Drawing.Point(0, 77);
            this.lvw_log_view.MultiSelect = false;
            this.lvw_log_view.Name = "lvw_log_view";
            this.lvw_log_view.Size = new System.Drawing.Size(1095, 169);
            this.lvw_log_view.TabIndex = 20;
            this.lvw_log_view.UseCompatibleStateImageBehavior = false;
            this.lvw_log_view.View = System.Windows.Forms.View.Details;
            this.lvw_log_view.Visible = false;
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(394, 34);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(128, 26);
            this.cmbType.TabIndex = 11;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.BackColor = System.Drawing.Color.Transparent;
            this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblType.Location = new System.Drawing.Point(391, 9);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(40, 18);
            this.lblType.TabIndex = 10;
            this.lblType.Text = "Type";
            // 
            // cmbToMin
            // 
            this.cmbToMin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbToMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbToMin.FormattingEnabled = true;
            this.cmbToMin.Location = new System.Drawing.Point(313, 39);
            this.cmbToMin.Name = "cmbToMin";
            this.cmbToMin.Size = new System.Drawing.Size(45, 26);
            this.cmbToMin.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(295, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = ":";
            // 
            // cmbToHr
            // 
            this.cmbToHr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToHr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbToHr.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbToHr.FormattingEnabled = true;
            this.cmbToHr.Location = new System.Drawing.Point(244, 40);
            this.cmbToHr.Name = "cmbToHr";
            this.cmbToHr.Size = new System.Drawing.Size(45, 26);
            this.cmbToHr.TabIndex = 7;
            // 
            // dtpTo
            // 
            this.dtpTo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTo.CustomFormat = "";
            this.dtpTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(123, 42);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(114, 24);
            this.dtpTo.TabIndex = 6;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.BackColor = System.Drawing.Color.Transparent;
            this.lblTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTo.Location = new System.Drawing.Point(3, 47);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(98, 18);
            this.lblTo.TabIndex = 5;
            this.lblTo.Text = "To Date/Time";
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = System.Drawing.Color.LimeGreen;
            this.btnFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFilter.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.btnFilter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.ForeColor = System.Drawing.Color.Black;
            this.btnFilter.Location = new System.Drawing.Point(151, 4);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(75, 48);
            this.btnFilter.TabIndex = 15;
            this.btnFilter.Text = "&Filter";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Gold;
            this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReset.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gold;
            this.btnReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.Black;
            this.btnReset.Location = new System.Drawing.Point(232, 5);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 48);
            this.btnReset.TabIndex = 14;
            this.btnReset.Text = "&Reset Filter";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnPurge
            // 
            this.btnPurge.BackColor = System.Drawing.Color.Maroon;
            this.btnPurge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPurge.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnPurge.FlatAppearance.BorderSize = 0;
            this.btnPurge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            this.btnPurge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.btnPurge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPurge.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPurge.ForeColor = System.Drawing.Color.White;
            this.btnPurge.Location = new System.Drawing.Point(313, 4);
            this.btnPurge.Name = "btnPurge";
            this.btnPurge.Size = new System.Drawing.Size(75, 48);
            this.btnPurge.TabIndex = 13;
            this.btnPurge.Text = "&Purge Log";
            this.btnPurge.UseVisualStyleBackColor = false;
            this.btnPurge.Click += new System.EventHandler(this.btnPurge_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.btnFilter);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.btnPurge);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 246);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1095, 60);
            this.panel1.TabIndex = 21;
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
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(394, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 48);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmbFromMin
            // 
            this.cmbFromMin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFromMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFromMin.FormattingEnabled = true;
            this.cmbFromMin.Location = new System.Drawing.Point(313, 9);
            this.cmbFromMin.Name = "cmbFromMin";
            this.cmbFromMin.Size = new System.Drawing.Size(45, 26);
            this.cmbFromMin.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(295, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = ":";
            // 
            // cmbFromHr
            // 
            this.cmbFromHr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromHr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFromHr.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFromHr.FormattingEnabled = true;
            this.cmbFromHr.Location = new System.Drawing.Point(244, 8);
            this.cmbFromHr.Name = "cmbFromHr";
            this.cmbFromHr.Size = new System.Drawing.Size(45, 26);
            this.cmbFromHr.TabIndex = 2;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrom.CustomFormat = "";
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(123, 9);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(114, 24);
            this.dtpFrom.TabIndex = 1;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.BackColor = System.Drawing.Color.Transparent;
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblFrom.Location = new System.Drawing.Point(3, 12);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(116, 18);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From Date/Time";
            // 
            // pnlHdr
            // 
            this.pnlHdr.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlHdr.BackgroundImage")));
            this.pnlHdr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlHdr.Controls.Add(this.cmbType);
            this.pnlHdr.Controls.Add(this.lblType);
            this.pnlHdr.Controls.Add(this.cmbToMin);
            this.pnlHdr.Controls.Add(this.label2);
            this.pnlHdr.Controls.Add(this.cmbToHr);
            this.pnlHdr.Controls.Add(this.dtpTo);
            this.pnlHdr.Controls.Add(this.lblTo);
            this.pnlHdr.Controls.Add(this.cmbFromMin);
            this.pnlHdr.Controls.Add(this.label1);
            this.pnlHdr.Controls.Add(this.cmbFromHr);
            this.pnlHdr.Controls.Add(this.dtpFrom);
            this.pnlHdr.Controls.Add(this.lblFrom);
            this.pnlHdr.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHdr.Location = new System.Drawing.Point(0, 0);
            this.pnlHdr.Name = "pnlHdr";
            this.pnlHdr.Size = new System.Drawing.Size(1095, 77);
            this.pnlHdr.TabIndex = 19;
            // 
            // ucViewLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lvw_log_view);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlHdr);
            this.Name = "ucViewLog";
            this.Size = new System.Drawing.Size(1095, 306);
            this.Load += new System.EventHandler(this.ucViewLog_Load);
            this.panel1.ResumeLayout(false);
            this.pnlHdr.ResumeLayout(false);
            this.pnlHdr.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvw_log_view;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbToMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbToHr;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnPurge;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cmbFromMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbFromHr;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Panel pnlHdr;
    }
}
