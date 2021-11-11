namespace VETRIS_DICOM_ROUTER_ADMIN.UserControls
{
    partial class ucConfig
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucConfig));
            this.tcSettings = new System.Windows.Forms.TabControl();
            this.tpVS = new System.Windows.Forms.TabPage();
            this.txtVETURL = new System.Windows.Forms.TextBox();
            this.lblVSAddress = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblAddr2 = new System.Windows.Forms.Label();
            this.txtVETLOGIN = new System.Windows.Forms.TextBox();
            this.lblZip = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblVSSitecode = new System.Windows.Forms.Label();
            this.lblAddr1 = new System.Windows.Forms.Label();
            this.lblVSInsName = new System.Windows.Forms.Label();
            this.lblInstName = new System.Windows.Forms.Label();
            this.lblSiteCode = new System.Windows.Forms.Label();
            this.tpRS = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvDevice = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aetitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.port_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.del = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnAddDevice = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkImgAutoDetect = new System.Windows.Forms.CheckBox();
            this.txtRcvFolder = new System.Windows.Forms.TextBox();
            this.chkAutoDetect = new System.Windows.Forms.CheckBox();
            this.txtRCVIMGDIR = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtRCVDIRMANUAL = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.tpSS = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnFTPAbsPath = new System.Windows.Forms.Button();
            this.txtFTPAbsPath = new System.Windows.Forms.TextBox();
            this.rdoUpload = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.rdoCopy = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkArch = new System.Windows.Forms.CheckBox();
            this.chkCompFile = new System.Windows.Forms.CheckBox();
            this.txtArchFolder = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnArchFolder = new System.Windows.Forms.Button();
            this.txtSendFolder = new System.Windows.Forms.TextBox();
            this.lblSndFolder = new System.Windows.Forms.Label();
            this.btnSndFolder = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tcSettings.SuspendLayout();
            this.tpVS.SuspendLayout();
            this.tpRS.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tpSS.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcSettings
            // 
            this.tcSettings.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tcSettings.Controls.Add(this.tpVS);
            this.tcSettings.Controls.Add(this.tpRS);
            this.tcSettings.Controls.Add(this.tpSS);
            this.tcSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcSettings.Location = new System.Drawing.Point(21, 42);
            this.tcSettings.Name = "tcSettings";
            this.tcSettings.SelectedIndex = 0;
            this.tcSettings.Size = new System.Drawing.Size(1257, 501);
            this.tcSettings.TabIndex = 96;
            // 
            // tpVS
            // 
            this.tpVS.BackColor = System.Drawing.Color.White;
            this.tpVS.Controls.Add(this.txtVETURL);
            this.tpVS.Controls.Add(this.lblVSAddress);
            this.tpVS.Controls.Add(this.label6);
            this.tpVS.Controls.Add(this.lblAddr2);
            this.tpVS.Controls.Add(this.txtVETLOGIN);
            this.tpVS.Controls.Add(this.lblZip);
            this.tpVS.Controls.Add(this.label7);
            this.tpVS.Controls.Add(this.lblVSSitecode);
            this.tpVS.Controls.Add(this.lblAddr1);
            this.tpVS.Controls.Add(this.lblVSInsName);
            this.tpVS.Controls.Add(this.lblInstName);
            this.tpVS.Controls.Add(this.lblSiteCode);
            this.tpVS.Location = new System.Drawing.Point(4, 32);
            this.tpVS.Name = "tpVS";
            this.tpVS.Size = new System.Drawing.Size(1249, 465);
            this.tpVS.TabIndex = 2;
            this.tpVS.Text = "VETRIS";
            // 
            // txtVETURL
            // 
            this.txtVETURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVETURL.ForeColor = System.Drawing.Color.Gray;
            this.txtVETURL.Location = new System.Drawing.Point(186, 207);
            this.txtVETURL.Margin = new System.Windows.Forms.Padding(4);
            this.txtVETURL.MaxLength = 200;
            this.txtVETURL.Name = "txtVETURL";
            this.txtVETURL.Size = new System.Drawing.Size(612, 27);
            this.txtVETURL.TabIndex = 2;
            // 
            // lblVSAddress
            // 
            this.lblVSAddress.AutoSize = true;
            this.lblVSAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVSAddress.ForeColor = System.Drawing.Color.Gray;
            this.lblVSAddress.Location = new System.Drawing.Point(28, 88);
            this.lblVSAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVSAddress.Name = "lblVSAddress";
            this.lblVSAddress.Size = new System.Drawing.Size(71, 20);
            this.lblVSAddress.TabIndex = 94;
            this.lblVSAddress.Text = "Address";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Gray;
            this.label6.Location = new System.Drawing.Point(28, 211);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 20);
            this.label6.TabIndex = 82;
            this.label6.Text = "URL";
            // 
            // lblAddr2
            // 
            this.lblAddr2.AutoSize = true;
            this.lblAddr2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddr2.ForeColor = System.Drawing.Color.Gray;
            this.lblAddr2.Location = new System.Drawing.Point(184, 112);
            this.lblAddr2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddr2.Name = "lblAddr2";
            this.lblAddr2.Size = new System.Drawing.Size(25, 20);
            this.lblAddr2.TabIndex = 91;
            this.lblAddr2.Text = "....";
            // 
            // txtVETLOGIN
            // 
            this.txtVETLOGIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVETLOGIN.ForeColor = System.Drawing.Color.Gray;
            this.txtVETLOGIN.Location = new System.Drawing.Point(186, 168);
            this.txtVETLOGIN.Margin = new System.Windows.Forms.Padding(4);
            this.txtVETLOGIN.MaxLength = 20;
            this.txtVETLOGIN.Name = "txtVETLOGIN";
            this.txtVETLOGIN.Size = new System.Drawing.Size(195, 27);
            this.txtVETLOGIN.TabIndex = 1;
            // 
            // lblZip
            // 
            this.lblZip.AutoSize = true;
            this.lblZip.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZip.ForeColor = System.Drawing.Color.Gray;
            this.lblZip.Location = new System.Drawing.Point(185, 134);
            this.lblZip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(25, 20);
            this.lblZip.TabIndex = 92;
            this.lblZip.Text = "....";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(28, 171);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 20);
            this.label7.TabIndex = 80;
            this.label7.Text = "Login ID";
            // 
            // lblVSSitecode
            // 
            this.lblVSSitecode.AutoSize = true;
            this.lblVSSitecode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVSSitecode.ForeColor = System.Drawing.Color.Gray;
            this.lblVSSitecode.Location = new System.Drawing.Point(28, 60);
            this.lblVSSitecode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVSSitecode.Name = "lblVSSitecode";
            this.lblVSSitecode.Size = new System.Drawing.Size(82, 20);
            this.lblVSSitecode.TabIndex = 91;
            this.lblVSSitecode.Text = "Site Code";
            // 
            // lblAddr1
            // 
            this.lblAddr1.AutoSize = true;
            this.lblAddr1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddr1.ForeColor = System.Drawing.Color.Gray;
            this.lblAddr1.Location = new System.Drawing.Point(183, 88);
            this.lblAddr1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddr1.Name = "lblAddr1";
            this.lblAddr1.Size = new System.Drawing.Size(25, 20);
            this.lblAddr1.TabIndex = 90;
            this.lblAddr1.Text = "....";
            // 
            // lblVSInsName
            // 
            this.lblVSInsName.AutoSize = true;
            this.lblVSInsName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVSInsName.ForeColor = System.Drawing.Color.Gray;
            this.lblVSInsName.Location = new System.Drawing.Point(28, 33);
            this.lblVSInsName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVSInsName.Name = "lblVSInsName";
            this.lblVSInsName.Size = new System.Drawing.Size(130, 20);
            this.lblVSInsName.TabIndex = 90;
            this.lblVSInsName.Text = "Institution Name";
            // 
            // lblInstName
            // 
            this.lblInstName.AutoSize = true;
            this.lblInstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstName.ForeColor = System.Drawing.Color.Gray;
            this.lblInstName.Location = new System.Drawing.Point(183, 33);
            this.lblInstName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstName.Name = "lblInstName";
            this.lblInstName.Size = new System.Drawing.Size(25, 20);
            this.lblInstName.TabIndex = 89;
            this.lblInstName.Text = "....";
            // 
            // lblSiteCode
            // 
            this.lblSiteCode.AutoSize = true;
            this.lblSiteCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteCode.ForeColor = System.Drawing.Color.Gray;
            this.lblSiteCode.Location = new System.Drawing.Point(183, 60);
            this.lblSiteCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSiteCode.Name = "lblSiteCode";
            this.lblSiteCode.Size = new System.Drawing.Size(25, 20);
            this.lblSiteCode.TabIndex = 93;
            this.lblSiteCode.Text = "....";
            // 
            // tpRS
            // 
            this.tpRS.BackColor = System.Drawing.Color.White;
            this.tpRS.Controls.Add(this.groupBox1);
            this.tpRS.Controls.Add(this.groupBox4);
            this.tpRS.Location = new System.Drawing.Point(4, 32);
            this.tpRS.Name = "tpRS";
            this.tpRS.Padding = new System.Windows.Forms.Padding(3);
            this.tpRS.Size = new System.Drawing.Size(1249, 465);
            this.tpRS.TabIndex = 0;
            this.tpRS.Text = "Receiver Service";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.dgvDevice);
            this.groupBox1.Controls.Add(this.btnAddDevice);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Gray;
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1208, 281);
            this.groupBox1.TabIndex = 87;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Devices";
            // 
            // dgvDevice
            // 
            this.dgvDevice.AllowUserToAddRows = false;
            this.dgvDevice.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.NullValue = null;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDevice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvDevice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.aetitle,
            this.port_no,
            this.del});
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDevice.DefaultCellStyle = dataGridViewCellStyle13;
            this.dgvDevice.Location = new System.Drawing.Point(16, 84);
            this.dgvDevice.Name = "dgvDevice";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDevice.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvDevice.RowTemplate.Height = 24;
            this.dgvDevice.Size = new System.Drawing.Size(1072, 171);
            this.dgvDevice.TabIndex = 20;
            this.dgvDevice.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDevice_CellClick);
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.id.DefaultCellStyle = dataGridViewCellStyle9;
            this.id.HeaderText = "Sl. #";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // aetitle
            // 
            this.aetitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.aetitle.DefaultCellStyle = dataGridViewCellStyle10;
            this.aetitle.HeaderText = "AE Title";
            this.aetitle.Name = "aetitle";
            this.aetitle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // port_no
            // 
            this.port_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.port_no.DefaultCellStyle = dataGridViewCellStyle11;
            this.port_no.HeaderText = "Port No.";
            this.port_no.Name = "port_no";
            this.port_no.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // del
            // 
            this.del.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle12.NullValue")));
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.del.DefaultCellStyle = dataGridViewCellStyle12;
            this.del.HeaderText = "Delete";
            this.del.Image = ((System.Drawing.Image)(resources.GetObject("del.Image")));
            this.del.Name = "del";
            this.del.ReadOnly = true;
            this.del.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // btnAddDevice
            // 
            this.btnAddDevice.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddDevice.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddDevice.BackgroundImage")));
            this.btnAddDevice.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAddDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddDevice.ForeColor = System.Drawing.Color.Black;
            this.btnAddDevice.Location = new System.Drawing.Point(14, 34);
            this.btnAddDevice.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddDevice.Name = "btnAddDevice";
            this.btnAddDevice.Size = new System.Drawing.Size(190, 34);
            this.btnAddDevice.TabIndex = 19;
            this.btnAddDevice.Text = "   Add a device";
            this.btnAddDevice.UseVisualStyleBackColor = false;
            this.btnAddDevice.Click += new System.EventHandler(this.btnAddDevice_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkImgAutoDetect);
            this.groupBox4.Controls.Add(this.txtRcvFolder);
            this.groupBox4.Controls.Add(this.chkAutoDetect);
            this.groupBox4.Controls.Add(this.txtRCVIMGDIR);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.txtRCVDIRMANUAL);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.Gray;
            this.groupBox4.Location = new System.Drawing.Point(7, 296);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(1208, 157);
            this.groupBox4.TabIndex = 86;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Path Settings";
            // 
            // chkImgAutoDetect
            // 
            this.chkImgAutoDetect.AutoSize = true;
            this.chkImgAutoDetect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkImgAutoDetect.Location = new System.Drawing.Point(943, 132);
            this.chkImgAutoDetect.Margin = new System.Windows.Forms.Padding(4);
            this.chkImgAutoDetect.Name = "chkImgAutoDetect";
            this.chkImgAutoDetect.Size = new System.Drawing.Size(164, 22);
            this.chkImgAutoDetect.TabIndex = 91;
            this.chkImgAutoDetect.Text = "Detect Automatically";
            this.chkImgAutoDetect.UseVisualStyleBackColor = true;
            this.chkImgAutoDetect.Visible = false;
            // 
            // txtRcvFolder
            // 
            this.txtRcvFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRcvFolder.ForeColor = System.Drawing.Color.Gray;
            this.txtRcvFolder.Location = new System.Drawing.Point(311, 24);
            this.txtRcvFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtRcvFolder.MaxLength = 5;
            this.txtRcvFolder.Name = "txtRcvFolder";
            this.txtRcvFolder.ReadOnly = true;
            this.txtRcvFolder.Size = new System.Drawing.Size(574, 27);
            this.txtRcvFolder.TabIndex = 78;
            // 
            // chkAutoDetect
            // 
            this.chkAutoDetect.AutoSize = true;
            this.chkAutoDetect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoDetect.Location = new System.Drawing.Point(699, 69);
            this.chkAutoDetect.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoDetect.Name = "chkAutoDetect";
            this.chkAutoDetect.Size = new System.Drawing.Size(186, 24);
            this.chkAutoDetect.TabIndex = 90;
            this.chkAutoDetect.Text = "Detect Automatically";
            this.chkAutoDetect.UseVisualStyleBackColor = true;
            // 
            // txtRCVIMGDIR
            // 
            this.txtRCVIMGDIR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRCVIMGDIR.ForeColor = System.Drawing.Color.Gray;
            this.txtRCVIMGDIR.Location = new System.Drawing.Point(311, 162);
            this.txtRCVIMGDIR.Margin = new System.Windows.Forms.Padding(4);
            this.txtRCVIMGDIR.MaxLength = 5;
            this.txtRCVIMGDIR.Name = "txtRCVIMGDIR";
            this.txtRCVIMGDIR.ReadOnly = true;
            this.txtRCVIMGDIR.Size = new System.Drawing.Size(801, 24);
            this.txtRCVIMGDIR.TabIndex = 89;
            this.txtRCVIMGDIR.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Gray;
            this.label11.Location = new System.Drawing.Point(13, 162);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(268, 18);
            this.label11.TabIndex = 88;
            this.label11.Text = "IMAGE (NON DICOM) Received Folder";
            this.label11.Visible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(1120, 156);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "   Browse";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            // 
            // txtRCVDIRMANUAL
            // 
            this.txtRCVDIRMANUAL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRCVDIRMANUAL.ForeColor = System.Drawing.Color.Gray;
            this.txtRCVDIRMANUAL.Location = new System.Drawing.Point(311, 101);
            this.txtRCVDIRMANUAL.Margin = new System.Windows.Forms.Padding(4);
            this.txtRCVDIRMANUAL.MaxLength = 5;
            this.txtRCVDIRMANUAL.Name = "txtRCVDIRMANUAL";
            this.txtRCVDIRMANUAL.ReadOnly = true;
            this.txtRCVDIRMANUAL.Size = new System.Drawing.Size(574, 27);
            this.txtRCVDIRMANUAL.TabIndex = 86;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Gray;
            this.label12.Location = new System.Drawing.Point(13, 101);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(191, 20);
            this.label12.TabIndex = 85;
            this.label12.Text = "DICOM Received Folder";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(893, 100);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 29);
            this.button2.TabIndex = 4;
            this.button2.Text = "   Browse";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Gray;
            this.label16.Location = new System.Drawing.Point(13, 30);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(206, 20);
            this.label16.TabIndex = 77;
            this.label16.Text = "DICOM Processing Folder";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Location = new System.Drawing.Point(893, 23);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 30);
            this.button3.TabIndex = 3;
            this.button3.Text = "   Browse";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Visible = false;
            // 
            // tpSS
            // 
            this.tpSS.BackColor = System.Drawing.Color.White;
            this.tpSS.Controls.Add(this.groupBox3);
            this.tpSS.Controls.Add(this.groupBox2);
            this.tpSS.Location = new System.Drawing.Point(4, 32);
            this.tpSS.Name = "tpSS";
            this.tpSS.Padding = new System.Windows.Forms.Padding(3);
            this.tpSS.Size = new System.Drawing.Size(1249, 465);
            this.tpSS.TabIndex = 1;
            this.tpSS.Text = "Sender Service";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btnFTPAbsPath);
            this.groupBox3.Controls.Add(this.txtFTPAbsPath);
            this.groupBox3.Controls.Add(this.rdoUpload);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.rdoCopy);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.Gray;
            this.groupBox3.Location = new System.Drawing.Point(7, 173);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(1220, 129);
            this.groupBox3.TabIndex = 132;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "FTP Settings";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(23, 43);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 20);
            this.label4.TabIndex = 126;
            this.label4.Text = "Send Files To FTP Folder";
            // 
            // btnFTPAbsPath
            // 
            this.btnFTPAbsPath.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFTPAbsPath.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFTPAbsPath.BackgroundImage")));
            this.btnFTPAbsPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFTPAbsPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFTPAbsPath.ForeColor = System.Drawing.Color.Black;
            this.btnFTPAbsPath.Location = new System.Drawing.Point(1081, 73);
            this.btnFTPAbsPath.Margin = new System.Windows.Forms.Padding(4);
            this.btnFTPAbsPath.Name = "btnFTPAbsPath";
            this.btnFTPAbsPath.Size = new System.Drawing.Size(106, 29);
            this.btnFTPAbsPath.TabIndex = 129;
            this.btnFTPAbsPath.Text = "   Browse";
            this.btnFTPAbsPath.UseVisualStyleBackColor = false;
            // 
            // txtFTPAbsPath
            // 
            this.txtFTPAbsPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFTPAbsPath.ForeColor = System.Drawing.Color.Gray;
            this.txtFTPAbsPath.Location = new System.Drawing.Point(415, 74);
            this.txtFTPAbsPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtFTPAbsPath.MaxLength = 5;
            this.txtFTPAbsPath.Name = "txtFTPAbsPath";
            this.txtFTPAbsPath.ReadOnly = true;
            this.txtFTPAbsPath.Size = new System.Drawing.Size(658, 27);
            this.txtFTPAbsPath.TabIndex = 131;
            // 
            // rdoUpload
            // 
            this.rdoUpload.AutoSize = true;
            this.rdoUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoUpload.Location = new System.Drawing.Point(415, 39);
            this.rdoUpload.Name = "rdoUpload";
            this.rdoUpload.Size = new System.Drawing.Size(129, 24);
            this.rdoUpload.TabIndex = 127;
            this.rdoUpload.TabStop = true;
            this.rdoUpload.Text = "By Uploading";
            this.rdoUpload.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(23, 74);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(201, 20);
            this.label5.TabIndex = 130;
            this.label5.Text = "FTP Folder Absolute Path";
            // 
            // rdoCopy
            // 
            this.rdoCopy.AutoSize = true;
            this.rdoCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoCopy.Location = new System.Drawing.Point(574, 39);
            this.rdoCopy.Name = "rdoCopy";
            this.rdoCopy.Size = new System.Drawing.Size(115, 24);
            this.rdoCopy.TabIndex = 128;
            this.rdoCopy.TabStop = true;
            this.rdoCopy.Text = "By Copying";
            this.rdoCopy.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.chkArch);
            this.groupBox2.Controls.Add(this.chkCompFile);
            this.groupBox2.Controls.Add(this.txtArchFolder);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btnArchFolder);
            this.groupBox2.Controls.Add(this.txtSendFolder);
            this.groupBox2.Controls.Add(this.lblSndFolder);
            this.groupBox2.Controls.Add(this.btnSndFolder);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Gray;
            this.groupBox2.Location = new System.Drawing.Point(7, 17);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(1220, 149);
            this.groupBox2.TabIndex = 87;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Path Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(148, 70);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 18);
            this.label1.TabIndex = 120;
            this.label1.Text = "(files to be archived after sending)";
            // 
            // chkArch
            // 
            this.chkArch.AutoSize = true;
            this.chkArch.Checked = true;
            this.chkArch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkArch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkArch.Location = new System.Drawing.Point(775, 114);
            this.chkArch.Margin = new System.Windows.Forms.Padding(4);
            this.chkArch.Name = "chkArch";
            this.chkArch.Size = new System.Drawing.Size(298, 24);
            this.chkArch.TabIndex = 119;
            this.chkArch.Text = "Archive files while sending to PACS";
            this.chkArch.UseVisualStyleBackColor = true;
            // 
            // chkCompFile
            // 
            this.chkCompFile.AutoSize = true;
            this.chkCompFile.Checked = true;
            this.chkCompFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCompFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCompFile.Location = new System.Drawing.Point(411, 114);
            this.chkCompFile.Margin = new System.Windows.Forms.Padding(4);
            this.chkCompFile.Name = "chkCompFile";
            this.chkCompFile.Size = new System.Drawing.Size(319, 24);
            this.chkCompFile.TabIndex = 118;
            this.chkCompFile.Text = "Compress files while sending to PACS";
            this.chkCompFile.UseVisualStyleBackColor = true;
            // 
            // txtArchFolder
            // 
            this.txtArchFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArchFolder.ForeColor = System.Drawing.Color.Gray;
            this.txtArchFolder.Location = new System.Drawing.Point(408, 67);
            this.txtArchFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtArchFolder.MaxLength = 5;
            this.txtArchFolder.Name = "txtArchFolder";
            this.txtArchFolder.ReadOnly = true;
            this.txtArchFolder.Size = new System.Drawing.Size(659, 27);
            this.txtArchFolder.TabIndex = 93;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Gray;
            this.label9.Location = new System.Drawing.Point(23, 70);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(117, 20);
            this.label9.TabIndex = 92;
            this.label9.Text = "Archive Folder";
            // 
            // btnArchFolder
            // 
            this.btnArchFolder.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnArchFolder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnArchFolder.BackgroundImage")));
            this.btnArchFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnArchFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnArchFolder.ForeColor = System.Drawing.Color.Black;
            this.btnArchFolder.Location = new System.Drawing.Point(1075, 66);
            this.btnArchFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnArchFolder.Name = "btnArchFolder";
            this.btnArchFolder.Size = new System.Drawing.Size(106, 29);
            this.btnArchFolder.TabIndex = 6;
            this.btnArchFolder.Text = "   Browse";
            this.btnArchFolder.UseVisualStyleBackColor = false;
            // 
            // txtSendFolder
            // 
            this.txtSendFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSendFolder.ForeColor = System.Drawing.Color.Gray;
            this.txtSendFolder.Location = new System.Drawing.Point(408, 32);
            this.txtSendFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtSendFolder.MaxLength = 5;
            this.txtSendFolder.Name = "txtSendFolder";
            this.txtSendFolder.ReadOnly = true;
            this.txtSendFolder.Size = new System.Drawing.Size(659, 27);
            this.txtSendFolder.TabIndex = 79;
            // 
            // lblSndFolder
            // 
            this.lblSndFolder.AutoSize = true;
            this.lblSndFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSndFolder.ForeColor = System.Drawing.Color.Gray;
            this.lblSndFolder.Location = new System.Drawing.Point(23, 32);
            this.lblSndFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSndFolder.Name = "lblSndFolder";
            this.lblSndFolder.Size = new System.Drawing.Size(121, 20);
            this.lblSndFolder.TabIndex = 78;
            this.lblSndFolder.Text = "Sending Folder";
            // 
            // btnSndFolder
            // 
            this.btnSndFolder.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSndFolder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSndFolder.BackgroundImage")));
            this.btnSndFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSndFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSndFolder.ForeColor = System.Drawing.Color.Black;
            this.btnSndFolder.Location = new System.Drawing.Point(1075, 31);
            this.btnSndFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnSndFolder.Name = "btnSndFolder";
            this.btnSndFolder.Size = new System.Drawing.Size(106, 29);
            this.btnSndFolder.TabIndex = 5;
            this.btnSndFolder.Text = "   Browse";
            this.btnSndFolder.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LimeGreen;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LimeGreen;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(1064, 576);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 43);
            this.btnSave.TabIndex = 97;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.btnClose.Location = new System.Drawing.Point(1172, 576);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 43);
            this.btnClose.TabIndex = 98;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ucConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tcSettings);
            this.Name = "ucConfig";
            this.Size = new System.Drawing.Size(1296, 666);
            this.Load += new System.EventHandler(this.ucConfig_Load);
            this.tcSettings.ResumeLayout(false);
            this.tpVS.ResumeLayout(false);
            this.tpVS.PerformLayout();
            this.tpRS.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tpSS.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcSettings;
        private System.Windows.Forms.TabPage tpVS;
        private System.Windows.Forms.TextBox txtVETURL;
        private System.Windows.Forms.Label lblVSAddress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblAddr2;
        private System.Windows.Forms.TextBox txtVETLOGIN;
        private System.Windows.Forms.Label lblZip;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblVSSitecode;
        private System.Windows.Forms.Label lblAddr1;
        private System.Windows.Forms.Label lblVSInsName;
        private System.Windows.Forms.Label lblInstName;
        private System.Windows.Forms.Label lblSiteCode;
        private System.Windows.Forms.TabPage tpRS;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkImgAutoDetect;
        private System.Windows.Forms.TextBox txtRcvFolder;
        private System.Windows.Forms.CheckBox chkAutoDetect;
        private System.Windows.Forms.TextBox txtRCVIMGDIR;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtRCVDIRMANUAL;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabPage tpSS;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAddDevice;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkArch;
        private System.Windows.Forms.CheckBox chkCompFile;
        private System.Windows.Forms.TextBox txtArchFolder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnArchFolder;
        private System.Windows.Forms.TextBox txtSendFolder;
        private System.Windows.Forms.Label lblSndFolder;
        private System.Windows.Forms.Button btnSndFolder;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFTPAbsPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnFTPAbsPath;
        private System.Windows.Forms.RadioButton rdoCopy;
        private System.Windows.Forms.RadioButton rdoUpload;
        private System.Windows.Forms.DataGridView dgvDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn aetitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn port_no;
        private System.Windows.Forms.DataGridViewImageColumn del;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
    }
}
