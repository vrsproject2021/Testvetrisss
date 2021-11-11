namespace VETRIS_DICOM_ROUTER_FILE_UPLOAD
{
    partial class frmUpload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpload));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCheckConn = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.lblVer = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pnlAction = new System.Windows.Forms.Panel();
            this.gbManual = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.btnFiles = new System.Windows.Forms.Button();
            this.gbAuto = new System.Windows.Forms.GroupBox();
            this.dgvImg = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblFileType = new System.Windows.Forms.Label();
            this.pnlScan = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmbFileType = new System.Windows.Forms.ComboBox();
            this.dgvPatient = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rdoAllFiles = new System.Windows.Forms.RadioButton();
            this.rdoPatient = new System.Windows.Forms.RadioButton();
            this.btnUploadUSB = new System.Windows.Forms.Button();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.sel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.patient_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.file_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.lstDrives = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkShowProg = new System.Windows.Forms.CheckBox();
            this.btnManual = new System.Windows.Forms.Button();
            this.btnAuto = new System.Windows.Forms.Button();
            this.lblpbInfo = new System.Windows.Forms.Label();
            this.pb1 = new System.Windows.Forms.ProgressBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.pnlHeader.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlAction.SuspendLayout();
            this.gbManual.SuspendLayout();
            this.gbAuto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImg)).BeginInit();
            this.pnlScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.AllowDrop = true;
            this.pnlHeader.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlHeader.Controls.Add(this.panel2);
            this.pnlHeader.Controls.Add(this.lblVer);
            this.pnlHeader.Controls.Add(this.pbLogo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(939, 75);
            this.pnlHeader.TabIndex = 22;
            this.pnlHeader.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlHeader_DragDrop);
            this.pnlHeader.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlHeader_DragEnter);
            // 
            // panel2
            // 
            this.panel2.AllowDrop = true;
            this.panel2.Controls.Add(this.btnCheckConn);
            this.panel2.Controls.Add(this.btnDownload);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(245, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(694, 75);
            this.panel2.TabIndex = 3;
            this.panel2.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel2_DragDrop);
            this.panel2.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel2_DragEnter);
            // 
            // btnCheckConn
            // 
            this.btnCheckConn.BackColor = System.Drawing.Color.Honeydew;
            this.btnCheckConn.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckConn.Image")));
            this.btnCheckConn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCheckConn.Location = new System.Drawing.Point(491, 11);
            this.btnCheckConn.Name = "btnCheckConn";
            this.btnCheckConn.Size = new System.Drawing.Size(178, 55);
            this.btnCheckConn.TabIndex = 1;
            this.btnCheckConn.Text = "Check Internet Connectivity";
            this.btnCheckConn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCheckConn.UseVisualStyleBackColor = false;
            this.btnCheckConn.Click += new System.EventHandler(this.btnCheckConn_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.Gold;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload.Location = new System.Drawing.Point(223, 12);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(246, 55);
            this.btnDownload.TabIndex = 0;
            this.btnDownload.Text = "Download New Version";
            this.btnDownload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Visible = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // lblVer
            // 
            this.lblVer.AutoSize = true;
            this.lblVer.Location = new System.Drawing.Point(7, 53);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(20, 17);
            this.lblVer.TabIndex = 2;
            this.lblVer.Text = "...";
            // 
            // pbLogo
            // 
            this.pbLogo.BackColor = System.Drawing.Color.Transparent;
            this.pbLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbLogo.BackgroundImage")));
            this.pbLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbLogo.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbLogo.InitialImage")));
            this.pbLogo.Location = new System.Drawing.Point(9, 7);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(191, 43);
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // pnlAction
            // 
            this.pnlAction.AllowDrop = true;
            this.pnlAction.BackColor = System.Drawing.Color.Transparent;
            this.pnlAction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlAction.Controls.Add(this.gbManual);
            this.pnlAction.Controls.Add(this.gbAuto);
            this.pnlAction.Controls.Add(this.groupBox1);
            this.pnlAction.Controls.Add(this.lblpbInfo);
            this.pnlAction.Controls.Add(this.pb1);
            this.pnlAction.Controls.Add(this.btnClose);
            this.pnlAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAction.Location = new System.Drawing.Point(0, 75);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Size = new System.Drawing.Size(939, 554);
            this.pnlAction.TabIndex = 24;
            this.pnlAction.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlAction_DragDrop);
            this.pnlAction.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlAction_DragEnter);
            // 
            // gbManual
            // 
            this.gbManual.Controls.Add(this.btnRemove);
            this.gbManual.Controls.Add(this.btnUpload);
            this.gbManual.Controls.Add(this.lstFiles);
            this.gbManual.Controls.Add(this.btnFiles);
            this.gbManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbManual.ForeColor = System.Drawing.Color.DimGray;
            this.gbManual.Location = new System.Drawing.Point(3, 317);
            this.gbManual.Name = "gbManual";
            this.gbManual.Size = new System.Drawing.Size(913, 226);
            this.gbManual.TabIndex = 109;
            this.gbManual.TabStop = false;
            this.gbManual.Text = "Manally select folder/file(s) and upload";
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnRemove.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRemove.BackgroundImage")));
            this.btnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.btnRemove.ForeColor = System.Drawing.Color.Black;
            this.btnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemove.Location = new System.Drawing.Point(781, 103);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(115, 37);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "     Remove";
            this.btnRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnUpload.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpload.BackgroundImage")));
            this.btnUpload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnUpload.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnUpload.FlatAppearance.BorderSize = 0;
            this.btnUpload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.btnUpload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.ForeColor = System.Drawing.Color.Black;
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpload.Location = new System.Drawing.Point(781, 146);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(115, 37);
            this.btnUpload.TabIndex = 7;
            this.btnUpload.Text = "    Upload";
            this.btnUpload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.AllowDrop = true;
            this.lstFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstFiles.ForeColor = System.Drawing.Color.DimGray;
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.HorizontalScrollbar = true;
            this.lstFiles.ItemHeight = 17;
            this.lstFiles.Location = new System.Drawing.Point(8, 27);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstFiles.Size = new System.Drawing.Size(732, 174);
            this.lstFiles.TabIndex = 6;
            this.lstFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstFiles_DragDrop);
            this.lstFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstFiles_DragEnter);
            // 
            // btnFiles
            // 
            this.btnFiles.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFiles.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFiles.BackgroundImage")));
            this.btnFiles.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.btnFiles.ForeColor = System.Drawing.Color.Black;
            this.btnFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFiles.Location = new System.Drawing.Point(781, 60);
            this.btnFiles.Name = "btnFiles";
            this.btnFiles.Size = new System.Drawing.Size(115, 37);
            this.btnFiles.TabIndex = 5;
            this.btnFiles.Text = "     Browse";
            this.btnFiles.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFiles.UseVisualStyleBackColor = false;
            this.btnFiles.Click += new System.EventHandler(this.btnFiles_Click);
            // 
            // gbAuto
            // 
            this.gbAuto.Controls.Add(this.dgvImg);
            this.gbAuto.Controls.Add(this.lblFileType);
            this.gbAuto.Controls.Add(this.pnlScan);
            this.gbAuto.Controls.Add(this.cmbFileType);
            this.gbAuto.Controls.Add(this.dgvPatient);
            this.gbAuto.Controls.Add(this.rdoAllFiles);
            this.gbAuto.Controls.Add(this.rdoPatient);
            this.gbAuto.Controls.Add(this.btnUploadUSB);
            this.gbAuto.Controls.Add(this.dgvFiles);
            this.gbAuto.Controls.Add(this.label1);
            this.gbAuto.Controls.Add(this.lstDrives);
            this.gbAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAuto.ForeColor = System.Drawing.Color.DimGray;
            this.gbAuto.Location = new System.Drawing.Point(3, 112);
            this.gbAuto.Name = "gbAuto";
            this.gbAuto.Size = new System.Drawing.Size(913, 242);
            this.gbAuto.TabIndex = 108;
            this.gbAuto.TabStop = false;
            this.gbAuto.Text = "Auto Detect USB drive(s) and select file(s)";
            this.gbAuto.Visible = false;
            // 
            // dgvImg
            // 
            this.dgvImg.AllowUserToAddRows = false;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvImg.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvImg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvImg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dgvImg.Location = new System.Drawing.Point(533, 70);
            this.dgvImg.Name = "dgvImg";
            this.dgvImg.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvImg.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvImg.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvImg.Size = new System.Drawing.Size(353, 129);
            this.dgvImg.TabIndex = 9;
            this.dgvImg.Visible = false;
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.NullValue = false;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn2.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewCheckBoxColumn2.FalseValue = "N";
            this.dataGridViewCheckBoxColumn2.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn2.TrueValue = "Y";
            this.dataGridViewCheckBoxColumn2.Width = 70;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewTextBoxColumn3.HeaderText = "File Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // lblFileType
            // 
            this.lblFileType.AutoSize = true;
            this.lblFileType.Location = new System.Drawing.Point(17, 25);
            this.lblFileType.Name = "lblFileType";
            this.lblFileType.Size = new System.Drawing.Size(156, 24);
            this.lblFileType.TabIndex = 1;
            this.lblFileType.Text = "Select File Type :";
            // 
            // pnlScan
            // 
            this.pnlScan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlScan.Controls.Add(this.label2);
            this.pnlScan.Controls.Add(this.pictureBox1);
            this.pnlScan.Location = new System.Drawing.Point(401, 113);
            this.pnlScan.Name = "pnlScan";
            this.pnlScan.Size = new System.Drawing.Size(276, 59);
            this.pnlScan.TabIndex = 109;
            this.pnlScan.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(57, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(259, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Scanning USB Drive(s) ... Please Wait";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(39, 34);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // cmbFileType
            // 
            this.cmbFileType.BackColor = System.Drawing.Color.White;
            this.cmbFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFileType.ForeColor = System.Drawing.Color.DimGray;
            this.cmbFileType.FormattingEnabled = true;
            this.cmbFileType.Location = new System.Drawing.Point(137, 23);
            this.cmbFileType.Name = "cmbFileType";
            this.cmbFileType.Size = new System.Drawing.Size(154, 32);
            this.cmbFileType.TabIndex = 2;
            this.cmbFileType.SelectedIndexChanged += new System.EventHandler(this.cmbFileType_SelectedIndexChanged);
            // 
            // dgvPatient
            // 
            this.dgvPatient.AllowUserToAddRows = false;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPatient.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvPatient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPatient.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgvPatient.Location = new System.Drawing.Point(137, 59);
            this.dgvPatient.Name = "dgvPatient";
            this.dgvPatient.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPatient.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvPatient.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvPatient.Size = new System.Drawing.Size(389, 165);
            this.dgvPatient.TabIndex = 8;
            this.dgvPatient.Visible = false;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle14.NullValue = false;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewCheckBoxColumn1.FalseValue = "N";
            this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.TrueValue = "Y";
            this.dataGridViewCheckBoxColumn1.Width = 70;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Patient Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 300;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewTextBoxColumn2.HeaderText = "File Count";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // rdoAllFiles
            // 
            this.rdoAllFiles.AutoSize = true;
            this.rdoAllFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoAllFiles.Location = new System.Drawing.Point(533, 23);
            this.rdoAllFiles.Name = "rdoAllFiles";
            this.rdoAllFiles.Size = new System.Drawing.Size(123, 24);
            this.rdoAllFiles.TabIndex = 4;
            this.rdoAllFiles.TabStop = true;
            this.rdoAllFiles.Text = "All Files List";
            this.rdoAllFiles.UseVisualStyleBackColor = true;
            this.rdoAllFiles.Visible = false;
            this.rdoAllFiles.Click += new System.EventHandler(this.rdoAllFiles_Click);
            // 
            // rdoPatient
            // 
            this.rdoPatient.AutoSize = true;
            this.rdoPatient.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoPatient.Location = new System.Drawing.Point(371, 23);
            this.rdoPatient.Name = "rdoPatient";
            this.rdoPatient.Size = new System.Drawing.Size(158, 24);
            this.rdoPatient.TabIndex = 3;
            this.rdoPatient.TabStop = true;
            this.rdoPatient.Text = "Patient Wise List";
            this.rdoPatient.UseVisualStyleBackColor = true;
            this.rdoPatient.Visible = false;
            this.rdoPatient.Click += new System.EventHandler(this.rdoPatient_Click);
            // 
            // btnUploadUSB
            // 
            this.btnUploadUSB.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnUploadUSB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUploadUSB.BackgroundImage")));
            this.btnUploadUSB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnUploadUSB.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnUploadUSB.FlatAppearance.BorderSize = 0;
            this.btnUploadUSB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.btnUploadUSB.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnUploadUSB.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadUSB.ForeColor = System.Drawing.Color.Black;
            this.btnUploadUSB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUploadUSB.Location = new System.Drawing.Point(781, 16);
            this.btnUploadUSB.Name = "btnUploadUSB";
            this.btnUploadUSB.Size = new System.Drawing.Size(115, 37);
            this.btnUploadUSB.TabIndex = 5;
            this.btnUploadUSB.Text = "    Upload";
            this.btnUploadUSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUploadUSB.UseVisualStyleBackColor = false;
            this.btnUploadUSB.Click += new System.EventHandler(this.btnUploadUSB_Click);
            // 
            // dgvFiles
            // 
            this.dgvFiles.AllowUserToAddRows = false;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sel,
            this.patient_name,
            this.file_name});
            this.dgvFiles.Location = new System.Drawing.Point(117, 59);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvFiles.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvFiles.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvFiles.Size = new System.Drawing.Size(779, 165);
            this.dgvFiles.TabIndex = 7;
            this.dgvFiles.Visible = false;
            // 
            // sel
            // 
            this.sel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle17.NullValue = false;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.sel.DefaultCellStyle = dataGridViewCellStyle17;
            this.sel.FalseValue = "N";
            this.sel.HeaderText = "Select";
            this.sel.Name = "sel";
            this.sel.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sel.TrueValue = "Y";
            this.sel.Width = 70;
            // 
            // patient_name
            // 
            this.patient_name.HeaderText = "Patient Name";
            this.patient_name.Name = "patient_name";
            this.patient_name.ReadOnly = true;
            this.patient_name.Width = 300;
            // 
            // file_name
            // 
            this.file_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black;
            this.file_name.DefaultCellStyle = dataGridViewCellStyle18;
            this.file_name.HeaderText = "File Name";
            this.file_name.Name = "file_name";
            this.file_name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 18);
            this.label1.TabIndex = 105;
            this.label1.Text = "USB Drive(s)";
            // 
            // lstDrives
            // 
            this.lstDrives.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstDrives.ForeColor = System.Drawing.Color.Gray;
            this.lstDrives.FormattingEnabled = true;
            this.lstDrives.HorizontalScrollbar = true;
            this.lstDrives.ItemHeight = 18;
            this.lstDrives.Location = new System.Drawing.Point(6, 70);
            this.lstDrives.Name = "lstDrives";
            this.lstDrives.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstDrives.Size = new System.Drawing.Size(89, 148);
            this.lstDrives.Sorted = true;
            this.lstDrives.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox1.Controls.Add(this.chkShowProg);
            this.groupBox1.Controls.Add(this.btnManual);
            this.groupBox1.Controls.Add(this.btnAuto);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Gray;
            this.groupBox1.Location = new System.Drawing.Point(3, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(913, 99);
            this.groupBox1.TabIndex = 107;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose your method to upload :";
            // 
            // chkShowProg
            // 
            this.chkShowProg.AutoSize = true;
            this.chkShowProg.Location = new System.Drawing.Point(503, 50);
            this.chkShowProg.Name = "chkShowProg";
            this.chkShowProg.Size = new System.Drawing.Size(491, 28);
            this.chkShowProg.TabIndex = 3;
            this.chkShowProg.Text = "View file processing progess when file(s) are uploaded";
            this.chkShowProg.UseVisualStyleBackColor = true;
            // 
            // btnManual
            // 
            this.btnManual.BackColor = System.Drawing.Color.Gold;
            this.btnManual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnManual.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnManual.FlatAppearance.BorderSize = 0;
            this.btnManual.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.btnManual.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManual.ForeColor = System.Drawing.Color.Black;
            this.btnManual.Image = ((System.Drawing.Image)(resources.GetObject("btnManual.Image")));
            this.btnManual.Location = new System.Drawing.Point(9, 21);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(115, 72);
            this.btnManual.TabIndex = 1;
            this.btnManual.Text = "Manually select folder/file(s)";
            this.btnManual.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnManual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnManual.UseVisualStyleBackColor = false;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // btnAuto
            // 
            this.btnAuto.BackColor = System.Drawing.Color.Azure;
            this.btnAuto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAuto.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnAuto.FlatAppearance.BorderSize = 0;
            this.btnAuto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.btnAuto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuto.ForeColor = System.Drawing.Color.Black;
            this.btnAuto.Image = ((System.Drawing.Image)(resources.GetObject("btnAuto.Image")));
            this.btnAuto.Location = new System.Drawing.Point(130, 21);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(115, 72);
            this.btnAuto.TabIndex = 2;
            this.btnAuto.Text = "Auto detect from USB drive(s)";
            this.btnAuto.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAuto.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAuto.UseVisualStyleBackColor = false;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // lblpbInfo
            // 
            this.lblpbInfo.AutoSize = true;
            this.lblpbInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.lblpbInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblpbInfo.Location = new System.Drawing.Point(6, 542);
            this.lblpbInfo.Name = "lblpbInfo";
            this.lblpbInfo.Size = new System.Drawing.Size(15, 24);
            this.lblpbInfo.TabIndex = 112;
            this.lblpbInfo.Text = ".";
            this.lblpbInfo.Visible = false;
            // 
            // pb1
            // 
            this.pb1.ForeColor = System.Drawing.Color.LawnGreen;
            this.pb1.Location = new System.Drawing.Point(3, 571);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(796, 23);
            this.pb1.TabIndex = 111;
            this.pb1.Visible = false;
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
            this.btnClose.Location = new System.Drawing.Point(824, 561);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 35);
            this.btnClose.TabIndex = 110;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer3
            // 
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // frmUpload
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(939, 629);
            this.Controls.Add(this.pnlAction);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Gray;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUpload";
            this.Text = "VETRIS DICOM ROUTER : Upload File";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmUpload_FormClosed);
            this.Load += new System.EventHandler(this.frmUpload_Load);
            this.Shown += new System.EventHandler(this.frmUpload_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmUpload_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmUpload_DragEnter);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pnlAction.ResumeLayout(false);
            this.pnlAction.PerformLayout();
            this.gbManual.ResumeLayout(false);
            this.gbAuto.ResumeLayout(false);
            this.gbAuto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImg)).EndInit();
            this.pnlScan.ResumeLayout(false);
            this.pnlScan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCheckConn;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label lblVer;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Panel pnlAction;
        private System.Windows.Forms.GroupBox gbManual;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Button btnFiles;
        private System.Windows.Forms.GroupBox gbAuto;
        private System.Windows.Forms.DataGridView dgvImg;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label lblFileType;
        private System.Windows.Forms.Panel pnlScan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox cmbFileType;
        private System.Windows.Forms.DataGridView dgvPatient;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.RadioButton rdoAllFiles;
        private System.Windows.Forms.RadioButton rdoPatient;
        private System.Windows.Forms.Button btnUploadUSB;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sel;
        private System.Windows.Forms.DataGridViewTextBoxColumn patient_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn file_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstDrives;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkShowProg;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.Label lblpbInfo;
        private System.Windows.Forms.ProgressBar pb1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Button btnRemove;
    }
}

