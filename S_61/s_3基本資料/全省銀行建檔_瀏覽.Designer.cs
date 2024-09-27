namespace S_61.s_3基本資料
{
    partial class 全省銀行建檔_瀏覽
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(全省銀行建檔_瀏覽));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPnl1 = new S_61.MyControl.tableLayoutPnl();
            this.tableLayoutPnl2 = new S_61.MyControl.tableLayoutPnl();
            this.lblT1 = new S_61.MyControl.lblT();
            this.BaNo = new S_61.MyControl.TextBoxT();
            this.lblT2 = new S_61.MyControl.lblT();
            this.Count = new S_61.MyControl.lblT();
            this.BaName = new S_61.MyControl.TextBoxT();
            this.tableLayoutPnl3 = new S_61.MyControl.tableLayoutPnl();
            this.Append = new S_61.MyControl.btnBrowT();
            this.Get = new S_61.MyControl.btnBrowT();
            this.Exit = new S_61.MyControl.btnBrowT();
            this.btnBrowT1 = new S_61.MyControl.btnBrowT();
            this.Query = new S_61.MyControl.btnBrowT();
            this.dataGridViewT1 = new S_61.MyControl.dataGridViewT();
            this.銀行編號 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.銀行名稱 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.電話 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.地址 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.郵遞區號 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.備註 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPnl4 = new S_61.MyControl.tableLayoutPnl();
            this.panelT1 = new S_61.MyControl.panelT();
            this.btnExit = new S_61.MyControl.btnT();
            this.btnCancel = new S_61.MyControl.btnT();
            this.btnSave = new S_61.MyControl.btnT();
            this.btnModify = new S_61.MyControl.btnT();
            this.lblT3 = new S_61.MyControl.lblT();
            this.lblT4 = new S_61.MyControl.lblT();
            this.qBaNo = new S_61.MyControl.TextBoxT();
            this.qBaName = new S_61.MyControl.TextBoxT();
            this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
            this.CN = new System.Data.SqlClient.SqlConnection();
            this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
            this.Bank = new System.Data.SqlClient.SqlDataAdapter();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPnl1.SuspendLayout();
            this.tableLayoutPnl2.SuspendLayout();
            this.tableLayoutPnl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewT1)).BeginInit();
            this.tableLayoutPnl4.SuspendLayout();
            this.panelT1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 603);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(987, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(940, 17);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel3.Text = "插入";
            // 
            // tableLayoutPnl1
            // 
            this.tableLayoutPnl1.ColumnCount = 1;
            this.tableLayoutPnl1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPnl1.Controls.Add(this.tableLayoutPnl2, 0, 2);
            this.tableLayoutPnl1.Controls.Add(this.tableLayoutPnl3, 0, 3);
            this.tableLayoutPnl1.Controls.Add(this.dataGridViewT1, 0, 0);
            this.tableLayoutPnl1.Controls.Add(this.tableLayoutPnl4, 0, 1);
            this.tableLayoutPnl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPnl1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPnl1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPnl1.Name = "tableLayoutPnl1";
            this.tableLayoutPnl1.RowCount = 4;
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPnl1.Size = new System.Drawing.Size(987, 603);
            this.tableLayoutPnl1.TabIndex = 0;
            // 
            // tableLayoutPnl2
            // 
            this.tableLayoutPnl2.ColumnCount = 6;
            this.tableLayoutPnl2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPnl2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
            this.tableLayoutPnl2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPnl2.Controls.Add(this.lblT1, 1, 0);
            this.tableLayoutPnl2.Controls.Add(this.BaNo, 2, 0);
            this.tableLayoutPnl2.Controls.Add(this.lblT2, 3, 0);
            this.tableLayoutPnl2.Controls.Add(this.Count, 5, 0);
            this.tableLayoutPnl2.Controls.Add(this.BaName, 4, 0);
            this.tableLayoutPnl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPnl2.Location = new System.Drawing.Point(0, 504);
            this.tableLayoutPnl2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPnl2.Name = "tableLayoutPnl2";
            this.tableLayoutPnl2.RowCount = 1;
            this.tableLayoutPnl2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPnl2.Size = new System.Drawing.Size(987, 42);
            this.tableLayoutPnl2.TabIndex = 0;
            // 
            // lblT1
            // 
            this.lblT1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblT1.AutoSize = true;
            this.lblT1.BackColor = System.Drawing.Color.Transparent;
            this.lblT1.Font = new System.Drawing.Font("細明體", 12F);
            this.lblT1.Location = new System.Drawing.Point(202, 13);
            this.lblT1.Name = "lblT1";
            this.lblT1.Size = new System.Drawing.Size(72, 16);
            this.lblT1.TabIndex = 0;
            this.lblT1.Text = "銀行編號";
            // 
            // BaNo
            // 
            this.BaNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BaNo.CanReSize = true;
            this.BaNo.Font = new System.Drawing.Font("細明體", 12F);
            this.BaNo.GrayView = false;
            this.BaNo.InputMode = S_61.MyControl.TextInputMode.Insert;
            this.BaNo.Location = new System.Drawing.Point(280, 7);
            this.BaNo.MaxLength = 20;
            this.BaNo.Name = "BaNo";
            this.BaNo.Size = new System.Drawing.Size(169, 27);
            this.BaNo.TabIndex = 0;
            this.BaNo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BaNo_KeyUp);
            // 
            // lblT2
            // 
            this.lblT2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblT2.AutoSize = true;
            this.lblT2.BackColor = System.Drawing.Color.Transparent;
            this.lblT2.Font = new System.Drawing.Font("細明體", 12F);
            this.lblT2.Location = new System.Drawing.Point(455, 13);
            this.lblT2.Name = "lblT2";
            this.lblT2.Size = new System.Drawing.Size(72, 16);
            this.lblT2.TabIndex = 0;
            this.lblT2.Text = "銀行名稱";
            // 
            // Count
            // 
            this.Count.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Count.AutoSize = true;
            this.Count.BackColor = System.Drawing.Color.Transparent;
            this.Count.Font = new System.Drawing.Font("細明體", 12F);
            this.Count.Location = new System.Drawing.Point(863, 13);
            this.Count.Name = "Count";
            this.Count.Size = new System.Drawing.Size(48, 16);
            this.Count.TabIndex = 3;
            this.Count.Text = "lblT2";
            // 
            // BaName
            // 
            this.BaName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BaName.CanReSize = true;
            this.BaName.Font = new System.Drawing.Font("細明體", 12F);
            this.BaName.GrayView = false;
            this.BaName.InputMode = S_61.MyControl.TextInputMode.Insert;
            this.BaName.Location = new System.Drawing.Point(533, 7);
            this.BaName.MaxLength = 30;
            this.BaName.Name = "BaName";
            this.BaName.Size = new System.Drawing.Size(251, 27);
            this.BaName.TabIndex = 1;
            this.BaName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BaName_KeyUp);
            // 
            // tableLayoutPnl3
            // 
            this.tableLayoutPnl3.ColumnCount = 7;
            this.tableLayoutPnl3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
            this.tableLayoutPnl3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
            this.tableLayoutPnl3.Controls.Add(this.Append, 1, 0);
            this.tableLayoutPnl3.Controls.Add(this.Get, 4, 0);
            this.tableLayoutPnl3.Controls.Add(this.Exit, 5, 0);
            this.tableLayoutPnl3.Controls.Add(this.btnBrowT1, 2, 0);
            this.tableLayoutPnl3.Controls.Add(this.Query, 3, 0);
            this.tableLayoutPnl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPnl3.Location = new System.Drawing.Point(0, 546);
            this.tableLayoutPnl3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPnl3.Name = "tableLayoutPnl3";
            this.tableLayoutPnl3.RowCount = 1;
            this.tableLayoutPnl3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPnl3.Size = new System.Drawing.Size(987, 57);
            this.tableLayoutPnl3.TabIndex = 1;
            // 
            // Append
            // 
            this.Append.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Append.Font = new System.Drawing.Font("細明體", 12F);
            this.Append.Location = new System.Drawing.Point(131, 3);
            this.Append.Name = "Append";
            this.Append.Size = new System.Drawing.Size(140, 51);
            this.Append.TabIndex = 0;
            this.Append.Text = "F2:新增";
            this.Append.UseVisualStyleBackColor = true;
            this.Append.Click += new System.EventHandler(this.Append_Click);
            // 
            // Get
            // 
            this.Get.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Get.Font = new System.Drawing.Font("細明體", 12F);
            this.Get.Location = new System.Drawing.Point(569, 3);
            this.Get.Name = "Get";
            this.Get.Size = new System.Drawing.Size(140, 51);
            this.Get.TabIndex = 1;
            this.Get.Text = "F9:取回";
            this.Get.UseVisualStyleBackColor = true;
            this.Get.Click += new System.EventHandler(this.Get_Click);
            // 
            // Exit
            // 
            this.Exit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Exit.Font = new System.Drawing.Font("細明體", 12F);
            this.Exit.Location = new System.Drawing.Point(715, 3);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(140, 51);
            this.Exit.TabIndex = 2;
            this.Exit.Text = "F11:結束";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // btnBrowT1
            // 
            this.btnBrowT1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowT1.Font = new System.Drawing.Font("細明體", 12F);
            this.btnBrowT1.Location = new System.Drawing.Point(277, 3);
            this.btnBrowT1.Name = "btnBrowT1";
            this.btnBrowT1.Size = new System.Drawing.Size(140, 51);
            this.btnBrowT1.TabIndex = 4;
            this.btnBrowT1.Text = "F5:查詢";
            this.btnBrowT1.UseVisualStyleBackColor = true;
            this.btnBrowT1.Click += new System.EventHandler(this.btnBrowT1_Click);
            // 
            // Query
            // 
            this.Query.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Query.Font = new System.Drawing.Font("細明體", 12F);
            this.Query.Location = new System.Drawing.Point(423, 3);
            this.Query.Name = "Query";
            this.Query.Size = new System.Drawing.Size(140, 51);
            this.Query.TabIndex = 5;
            this.Query.Text = "F6:字元查詢";
            this.Query.UseVisualStyleBackColor = true;
            this.Query.Click += new System.EventHandler(this.Query_Click);
            // 
            // dataGridViewT1
            // 
            this.dataGridViewT1.AllowUserToAddRows = false;
            this.dataGridViewT1.AllowUserToDeleteRows = false;
            this.dataGridViewT1.AllowUserToOrderColumns = true;
            this.dataGridViewT1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewT1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewT1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("細明體", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewT1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewT1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewT1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.銀行編號,
            this.銀行名稱,
            this.電話,
            this.地址,
            this.郵遞區號,
            this.備註});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("細明體", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewT1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewT1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewT1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewT1.EnableHeadersVisualStyles = false;
            this.dataGridViewT1.Font = new System.Drawing.Font("細明體", 12F);
            this.dataGridViewT1.GridWidth = 8;
            this.dataGridViewT1.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewT1.MultiSelect = false;
            this.dataGridViewT1.Name = "dataGridViewT1";
            this.dataGridViewT1.ReadOnly = true;
            this.dataGridViewT1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("細明體", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewT1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewT1.RowHeadersWidth = 20;
            this.dataGridViewT1.RowTemplate.Height = 24;
            this.dataGridViewT1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewT1.ShowCellToolTips = false;
            this.dataGridViewT1.Size = new System.Drawing.Size(981, 398);
            this.dataGridViewT1.TabIndex = 3;
            this.dataGridViewT1.編輯時單元格的顏色 = System.Drawing.Color.Yellow;
            this.dataGridViewT1.表頭終止顏色 = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(200)))), ((int)(((byte)(204)))));
            this.dataGridViewT1.表頭起始顏色 = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(247)))));
            this.dataGridViewT1.選擇單元格的顏色_可寫 = System.Drawing.Color.Yellow;
            this.dataGridViewT1.選擇單元格的顏色_唯讀 = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(69)))), ((int)(((byte)(45)))));
            this.dataGridViewT1.選擇行的顏色 = System.Drawing.Color.Yellow;
            this.dataGridViewT1.限制輸入_列名_整數_小數_空值 = null;
            this.dataGridViewT1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewT1_CellDoubleClick);
            // 
            // 銀行編號
            // 
            this.銀行編號.DataPropertyName = "bano";
            this.銀行編號.HeaderText = "銀行編號";
            this.銀行編號.MaxInputLength = 20;
            this.銀行編號.Name = "銀行編號";
            this.銀行編號.ReadOnly = true;
            // 
            // 銀行名稱
            // 
            this.銀行名稱.DataPropertyName = "baname";
            this.銀行名稱.HeaderText = "銀行名稱";
            this.銀行名稱.MaxInputLength = 30;
            this.銀行名稱.Name = "銀行名稱";
            this.銀行名稱.ReadOnly = true;
            // 
            // 電話
            // 
            this.電話.DataPropertyName = "batel";
            this.電話.HeaderText = "電話";
            this.電話.MaxInputLength = 20;
            this.電話.Name = "電話";
            this.電話.ReadOnly = true;
            // 
            // 地址
            // 
            this.地址.DataPropertyName = "baaddr1";
            this.地址.HeaderText = "地址";
            this.地址.MaxInputLength = 60;
            this.地址.Name = "地址";
            this.地址.ReadOnly = true;
            // 
            // 郵遞區號
            // 
            this.郵遞區號.DataPropertyName = "bar1";
            this.郵遞區號.HeaderText = "郵遞區號";
            this.郵遞區號.MaxInputLength = 5;
            this.郵遞區號.MinimumWidth = 100;
            this.郵遞區號.Name = "郵遞區號";
            this.郵遞區號.ReadOnly = true;
            // 
            // 備註
            // 
            this.備註.DataPropertyName = "bamemo";
            this.備註.HeaderText = "備註";
            this.備註.MaxInputLength = 60;
            this.備註.Name = "備註";
            this.備註.ReadOnly = true;
            // 
            // tableLayoutPnl4
            // 
            this.tableLayoutPnl4.ColumnCount = 5;
            this.tableLayoutPnl4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPnl4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPnl4.Controls.Add(this.panelT1, 4, 0);
            this.tableLayoutPnl4.Controls.Add(this.lblT3, 0, 0);
            this.tableLayoutPnl4.Controls.Add(this.lblT4, 2, 0);
            this.tableLayoutPnl4.Controls.Add(this.qBaNo, 1, 0);
            this.tableLayoutPnl4.Controls.Add(this.qBaName, 3, 0);
            this.tableLayoutPnl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPnl4.Location = new System.Drawing.Point(0, 404);
            this.tableLayoutPnl4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPnl4.Name = "tableLayoutPnl4";
            this.tableLayoutPnl4.RowCount = 1;
            this.tableLayoutPnl4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPnl4.Size = new System.Drawing.Size(987, 100);
            this.tableLayoutPnl4.TabIndex = 2;
            // 
            // panelT1
            // 
            this.panelT1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panelT1.Controls.Add(this.btnExit);
            this.panelT1.Controls.Add(this.btnCancel);
            this.panelT1.Controls.Add(this.btnSave);
            this.panelT1.Controls.Add(this.btnModify);
            this.panelT1.Location = new System.Drawing.Point(644, 2);
            this.panelT1.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.panelT1.Name = "panelT1";
            this.panelT1.Size = new System.Drawing.Size(286, 79);
            this.panelT1.TabIndex = 4;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExit.Font = new System.Drawing.Font("細明體", 9F);
            this.btnExit.Location = new System.Drawing.Point(207, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(69, 79);
            this.btnExit.TabIndex = 12;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("細明體", 9F);
            this.btnCancel.Location = new System.Drawing.Point(138, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 79);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("細明體", 9F);
            this.btnSave.Location = new System.Drawing.Point(69, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(69, 79);
            this.btnSave.TabIndex = 10;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnModify
            // 
            this.btnModify.BackColor = System.Drawing.SystemColors.Control;
            this.btnModify.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnModify.BackgroundImage")));
            this.btnModify.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnModify.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnModify.Font = new System.Drawing.Font("細明體", 9F);
            this.btnModify.Location = new System.Drawing.Point(0, 0);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(69, 79);
            this.btnModify.TabIndex = 6;
            this.btnModify.UseVisualStyleBackColor = false;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // lblT3
            // 
            this.lblT3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblT3.AutoSize = true;
            this.lblT3.BackColor = System.Drawing.Color.Transparent;
            this.lblT3.Font = new System.Drawing.Font("細明體", 12F);
            this.lblT3.Location = new System.Drawing.Point(3, 42);
            this.lblT3.Name = "lblT3";
            this.lblT3.Size = new System.Drawing.Size(72, 16);
            this.lblT3.TabIndex = 5;
            this.lblT3.Text = "銀行編號";
            // 
            // lblT4
            // 
            this.lblT4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblT4.AutoSize = true;
            this.lblT4.BackColor = System.Drawing.Color.Transparent;
            this.lblT4.Font = new System.Drawing.Font("細明體", 12F);
            this.lblT4.Location = new System.Drawing.Point(256, 42);
            this.lblT4.Name = "lblT4";
            this.lblT4.Size = new System.Drawing.Size(72, 16);
            this.lblT4.TabIndex = 5;
            this.lblT4.Text = "銀行名稱";
            // 
            // qBaNo
            // 
            this.qBaNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.qBaNo.CanReSize = true;
            this.qBaNo.Font = new System.Drawing.Font("細明體", 12F);
            this.qBaNo.GrayView = false;
            this.qBaNo.InputMode = S_61.MyControl.TextInputMode.Insert;
            this.qBaNo.Location = new System.Drawing.Point(81, 36);
            this.qBaNo.MaxLength = 20;
            this.qBaNo.Name = "qBaNo";
            this.qBaNo.Size = new System.Drawing.Size(169, 27);
            this.qBaNo.TabIndex = 0;
            this.qBaNo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.qBaNo_KeyUp);
            // 
            // qBaName
            // 
            this.qBaName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.qBaName.CanReSize = true;
            this.qBaName.Font = new System.Drawing.Font("細明體", 12F);
            this.qBaName.GrayView = false;
            this.qBaName.InputMode = S_61.MyControl.TextInputMode.Insert;
            this.qBaName.Location = new System.Drawing.Point(334, 36);
            this.qBaName.MaxLength = 30;
            this.qBaName.Name = "qBaName";
            this.qBaName.Size = new System.Drawing.Size(251, 27);
            this.qBaName.TabIndex = 1;
            this.qBaName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.qBaName_KeyUp);
            // 
            // sqlSelectCommand1
            // 
            this.sqlSelectCommand1.CommandText = "select * from  bank order by bano";
            this.sqlSelectCommand1.Connection = this.CN;
            // 
            // CN
            // 
            this.CN.ConnectionString = "Data Source=.;Initial Catalog=CHK;Integrated Security=True";
            this.CN.FireInfoMessageEventOnUserErrors = false;
            // 
            // sqlInsertCommand1
            // 
            this.sqlInsertCommand1.CommandText = resources.GetString("sqlInsertCommand1.CommandText");
            this.sqlInsertCommand1.Connection = this.CN;
            this.sqlInsertCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@bano", System.Data.SqlDbType.NVarChar, 0, "bano"),
            new System.Data.SqlClient.SqlParameter("@baname", System.Data.SqlDbType.NVarChar, 0, "baname"),
            new System.Data.SqlClient.SqlParameter("@batel", System.Data.SqlDbType.NVarChar, 0, "batel"),
            new System.Data.SqlClient.SqlParameter("@baaddr1", System.Data.SqlDbType.NVarChar, 0, "baaddr1"),
            new System.Data.SqlClient.SqlParameter("@bar1", System.Data.SqlDbType.NVarChar, 0, "bar1"),
            new System.Data.SqlClient.SqlParameter("@bamemo", System.Data.SqlDbType.NVarChar, 0, "bamemo")});
            // 
            // sqlUpdateCommand1
            // 
            this.sqlUpdateCommand1.CommandText = resources.GetString("sqlUpdateCommand1.CommandText");
            this.sqlUpdateCommand1.Connection = this.CN;
            this.sqlUpdateCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@bano", System.Data.SqlDbType.NVarChar, 0, "bano"),
            new System.Data.SqlClient.SqlParameter("@baname", System.Data.SqlDbType.NVarChar, 0, "baname"),
            new System.Data.SqlClient.SqlParameter("@batel", System.Data.SqlDbType.NVarChar, 0, "batel"),
            new System.Data.SqlClient.SqlParameter("@baaddr1", System.Data.SqlDbType.NVarChar, 0, "baaddr1"),
            new System.Data.SqlClient.SqlParameter("@bar1", System.Data.SqlDbType.NVarChar, 0, "bar1"),
            new System.Data.SqlClient.SqlParameter("@bamemo", System.Data.SqlDbType.NVarChar, 0, "bamemo"),
            new System.Data.SqlClient.SqlParameter("@Original_bano", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "bano", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_baname", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "baname", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_baname", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "baname", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_batel", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "batel", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_batel", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "batel", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_baaddr1", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "baaddr1", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_baaddr1", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "baaddr1", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_bar1", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "bar1", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_bar1", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "bar1", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_bamemo", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "bamemo", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_bamemo", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "bamemo", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlDeleteCommand1
            // 
            this.sqlDeleteCommand1.CommandText = resources.GetString("sqlDeleteCommand1.CommandText");
            this.sqlDeleteCommand1.Connection = this.CN;
            this.sqlDeleteCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_bano", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "bano", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_baname", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "baname", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_baname", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "baname", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_batel", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "batel", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_batel", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "batel", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_baaddr1", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "baaddr1", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_baaddr1", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "baaddr1", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_bar1", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "bar1", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_bar1", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "bar1", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_bamemo", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "bamemo", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_bamemo", System.Data.SqlDbType.NVarChar, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "bamemo", System.Data.DataRowVersion.Original, null)});
            // 
            // Bank
            // 
            this.Bank.DeleteCommand = this.sqlDeleteCommand1;
            this.Bank.InsertCommand = this.sqlInsertCommand1;
            this.Bank.SelectCommand = this.sqlSelectCommand1;
            this.Bank.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "bank", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("bano", "bano"),
                        new System.Data.Common.DataColumnMapping("baname", "baname"),
                        new System.Data.Common.DataColumnMapping("batel", "batel"),
                        new System.Data.Common.DataColumnMapping("baaddr1", "baaddr1"),
                        new System.Data.Common.DataColumnMapping("bar1", "bar1"),
                        new System.Data.Common.DataColumnMapping("bamemo", "bamemo")})});
            this.Bank.UpdateCommand = this.sqlUpdateCommand1;
            // 
            // 全省銀行建檔_瀏覽
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(987, 625);
            this.Controls.Add(this.tableLayoutPnl1);
            this.Controls.Add(this.statusStrip1);
            this.KeyPreview = true;
            this.Name = "全省銀行建檔_瀏覽";
            this.Text = "全省銀行建檔[瀏覽]";
            this.Load += new System.EventHandler(this.全省銀行建檔_瀏覽_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.全省銀行建檔_瀏覽_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPnl1.ResumeLayout(false);
            this.tableLayoutPnl2.ResumeLayout(false);
            this.tableLayoutPnl2.PerformLayout();
            this.tableLayoutPnl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewT1)).EndInit();
            this.tableLayoutPnl4.ResumeLayout(false);
            this.tableLayoutPnl4.PerformLayout();
            this.panelT1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private MyControl.tableLayoutPnl tableLayoutPnl1;
        private MyControl.tableLayoutPnl tableLayoutPnl2;
        private MyControl.lblT lblT1;
        private MyControl.TextBoxT BaNo;
        private MyControl.tableLayoutPnl tableLayoutPnl3;
        private MyControl.btnBrowT Append;
        private MyControl.btnBrowT Get;
        private MyControl.btnBrowT Exit;
        private MyControl.lblT Count;
        private MyControl.dataGridViewT dataGridViewT1;
        private MyControl.lblT lblT2;
        private MyControl.TextBoxT BaName;
        private MyControl.btnBrowT btnBrowT1;
        private MyControl.btnBrowT Query;
        private MyControl.tableLayoutPnl tableLayoutPnl4;
        private MyControl.panelT panelT1;
        private MyControl.btnT btnExit;
        private MyControl.btnT btnCancel;
        private MyControl.btnT btnSave;
        private MyControl.btnT btnModify;
        private MyControl.lblT lblT3;
        private MyControl.lblT lblT4;
        private MyControl.TextBoxT qBaNo;
        private MyControl.TextBoxT qBaName;
        private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
        private System.Data.SqlClient.SqlConnection CN;
        private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
        private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
        private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
        private System.Data.SqlClient.SqlDataAdapter Bank;
        private System.Windows.Forms.DataGridViewTextBoxColumn 銀行編號;
        private System.Windows.Forms.DataGridViewTextBoxColumn 銀行名稱;
        private System.Windows.Forms.DataGridViewTextBoxColumn 電話;
        private System.Windows.Forms.DataGridViewTextBoxColumn 地址;
        private System.Windows.Forms.DataGridViewTextBoxColumn 郵遞區號;
        private System.Windows.Forms.DataGridViewTextBoxColumn 備註;
    }
}