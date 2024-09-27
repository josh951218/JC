namespace S_61.s_1單據作業
{
    partial class 應付票據批次異動_瀏覽
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPnl_for_main1 = new S_61.MyControl.tableLayoutPnl_for_main();
            this.dataGridViewT1 = new S_61.MyControl.dataGridViewT();
            this.支票號碼 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.廠商簡稱 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.開票帳戶 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.到期日 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.預兌日 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.票面金額 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.異動日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPnl_for_main2 = new S_61.MyControl.tableLayoutPnl_for_main();
            this.lblT1 = new S_61.MyControl.lblT();
            this.lblT2 = new S_61.MyControl.lblT();
            this.CoNo = new S_61.MyControl.TextBoxT();
            this.CoName1 = new S_61.MyControl.TextBoxT();
            this.ChStatus = new S_61.MyControl.TextBoxT();
            this.ChName = new S_61.MyControl.TextBoxT();
            this.lblT3 = new S_61.MyControl.lblT();
            this.ChMemo = new S_61.MyControl.TextBoxT();
            this.tableLayoutPnl_for_main3 = new S_61.MyControl.tableLayoutPnl_for_main();
            this.lblT4 = new S_61.MyControl.lblT();
            this.Count = new S_61.MyControl.TextBoxT();
            this.btnGet = new S_61.MyControl.btnBrowT();
            this.btnExit = new S_61.MyControl.btnBrowT();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPnl_for_main1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewT1)).BeginInit();
            this.tableLayoutPnl_for_main2.SuspendLayout();
            this.tableLayoutPnl_for_main3.SuspendLayout();
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
            this.statusStrip1.TabIndex = 8;
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
            // tableLayoutPnl_for_main1
            // 
            this.tableLayoutPnl_for_main1.ColumnCount = 1;
            this.tableLayoutPnl_for_main1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPnl_for_main1.Controls.Add(this.dataGridViewT1, 0, 1);
            this.tableLayoutPnl_for_main1.Controls.Add(this.tableLayoutPnl_for_main2, 0, 0);
            this.tableLayoutPnl_for_main1.Controls.Add(this.tableLayoutPnl_for_main3, 0, 2);
            this.tableLayoutPnl_for_main1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPnl_for_main1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPnl_for_main1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPnl_for_main1.Name = "tableLayoutPnl_for_main1";
            this.tableLayoutPnl_for_main1.RowCount = 3;
            this.tableLayoutPnl_for_main1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPnl_for_main1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPnl_for_main1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPnl_for_main1.Size = new System.Drawing.Size(987, 603);
            this.tableLayoutPnl_for_main1.TabIndex = 9;
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
            this.支票號碼,
            this.廠商簡稱,
            this.開票帳戶,
            this.到期日,
            this.預兌日,
            this.票面金額,
            this.異動日期});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("細明體", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewT1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewT1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewT1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewT1.EnableHeadersVisualStyles = false;
            this.dataGridViewT1.Font = new System.Drawing.Font("細明體", 12F);
            this.dataGridViewT1.GridWidth = 8;
            this.dataGridViewT1.Location = new System.Drawing.Point(3, 75);
            this.dataGridViewT1.MultiSelect = false;
            this.dataGridViewT1.Name = "dataGridViewT1";
            this.dataGridViewT1.ReadOnly = true;
            this.dataGridViewT1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("細明體", 12F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewT1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewT1.RowHeadersWidth = 20;
            this.dataGridViewT1.RowTemplate.Height = 24;
            this.dataGridViewT1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewT1.ShowCellToolTips = false;
            this.dataGridViewT1.Size = new System.Drawing.Size(981, 476);
            this.dataGridViewT1.TabIndex = 0;
            this.dataGridViewT1.編輯時單元格的顏色 = System.Drawing.Color.Yellow;
            this.dataGridViewT1.表頭終止顏色 = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(200)))), ((int)(((byte)(204)))));
            this.dataGridViewT1.表頭起始顏色 = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(246)))), ((int)(((byte)(247)))));
            this.dataGridViewT1.選擇單元格的顏色_可寫 = System.Drawing.Color.Yellow;
            this.dataGridViewT1.選擇單元格的顏色_唯讀 = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(69)))), ((int)(((byte)(45)))));
            this.dataGridViewT1.選擇行的顏色 = System.Drawing.Color.Yellow;
            this.dataGridViewT1.限制輸入_列名_整數_小數_空值 = null;
            this.dataGridViewT1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewT1_CellDoubleClick);
            this.dataGridViewT1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewT1_CellValidating);
            // 
            // 支票號碼
            // 
            this.支票號碼.DataPropertyName = "chno";
            this.支票號碼.HeaderText = "支票號碼";
            this.支票號碼.MaxInputLength = 20;
            this.支票號碼.Name = "支票號碼";
            this.支票號碼.ReadOnly = true;
            // 
            // 廠商簡稱
            // 
            this.廠商簡稱.DataPropertyName = "faname1";
            this.廠商簡稱.HeaderText = "廠商簡稱";
            this.廠商簡稱.MaxInputLength = 10;
            this.廠商簡稱.Name = "廠商簡稱";
            this.廠商簡稱.ReadOnly = true;
            // 
            // 開票帳戶
            // 
            this.開票帳戶.DataPropertyName = "acno";
            this.開票帳戶.HeaderText = "開票帳戶";
            this.開票帳戶.MaxInputLength = 20;
            this.開票帳戶.Name = "開票帳戶";
            this.開票帳戶.ReadOnly = true;
            // 
            // 到期日
            // 
            this.到期日.DataPropertyName = "chdate2";
            this.到期日.HeaderText = "到期日";
            this.到期日.MaxInputLength = 10;
            this.到期日.Name = "到期日";
            this.到期日.ReadOnly = true;
            // 
            // 預兌日
            // 
            this.預兌日.DataPropertyName = "chdate3";
            this.預兌日.HeaderText = "預兌日";
            this.預兌日.MaxInputLength = 10;
            this.預兌日.Name = "預兌日";
            this.預兌日.ReadOnly = true;
            // 
            // 票面金額
            // 
            this.票面金額.DataPropertyName = "chmny";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.票面金額.DefaultCellStyle = dataGridViewCellStyle2;
            this.票面金額.HeaderText = "票面金額";
            this.票面金額.MaxInputLength = 20;
            this.票面金額.Name = "票面金額";
            this.票面金額.ReadOnly = true;
            // 
            // 異動日期
            // 
            this.異動日期.DataPropertyName = "chdate";
            this.異動日期.HeaderText = "異動日期";
            this.異動日期.MaxInputLength = 8;
            this.異動日期.Name = "異動日期";
            this.異動日期.ReadOnly = true;
            // 
            // tableLayoutPnl_for_main2
            // 
            this.tableLayoutPnl_for_main2.ColumnCount = 8;
            this.tableLayoutPnl_for_main2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPnl_for_main2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl_for_main2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl_for_main2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl_for_main2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPnl_for_main2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl_for_main2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl_for_main2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPnl_for_main2.Controls.Add(this.lblT1, 1, 0);
            this.tableLayoutPnl_for_main2.Controls.Add(this.lblT2, 1, 1);
            this.tableLayoutPnl_for_main2.Controls.Add(this.CoNo, 2, 0);
            this.tableLayoutPnl_for_main2.Controls.Add(this.CoName1, 3, 0);
            this.tableLayoutPnl_for_main2.Controls.Add(this.ChStatus, 2, 1);
            this.tableLayoutPnl_for_main2.Controls.Add(this.ChName, 3, 1);
            this.tableLayoutPnl_for_main2.Controls.Add(this.lblT3, 5, 0);
            this.tableLayoutPnl_for_main2.Controls.Add(this.ChMemo, 6, 0);
            this.tableLayoutPnl_for_main2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPnl_for_main2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPnl_for_main2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPnl_for_main2.Name = "tableLayoutPnl_for_main2";
            this.tableLayoutPnl_for_main2.RowCount = 2;
            this.tableLayoutPnl_for_main2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.38889F));
            this.tableLayoutPnl_for_main2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.61111F));
            this.tableLayoutPnl_for_main2.Size = new System.Drawing.Size(987, 72);
            this.tableLayoutPnl_for_main2.TabIndex = 1;
            // 
            // lblT1
            // 
            this.lblT1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblT1.AutoSize = true;
            this.lblT1.BackColor = System.Drawing.Color.Transparent;
            this.lblT1.Font = new System.Drawing.Font("細明體", 12F);
            this.lblT1.Location = new System.Drawing.Point(108, 10);
            this.lblT1.Name = "lblT1";
            this.lblT1.Size = new System.Drawing.Size(72, 16);
            this.lblT1.TabIndex = 0;
            this.lblT1.Text = "公司編號";
            // 
            // lblT2
            // 
            this.lblT2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblT2.AutoSize = true;
            this.lblT2.BackColor = System.Drawing.Color.Transparent;
            this.lblT2.Font = new System.Drawing.Font("細明體", 12F);
            this.lblT2.Location = new System.Drawing.Point(108, 46);
            this.lblT2.Name = "lblT2";
            this.lblT2.Size = new System.Drawing.Size(72, 16);
            this.lblT2.TabIndex = 1;
            this.lblT2.Text = "票態異動";
            // 
            // CoNo
            // 
            this.CoNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CoNo.CanReSize = true;
            this.CoNo.Font = new System.Drawing.Font("細明體", 12F);
            this.CoNo.GrayView = false;
            this.CoNo.InputMode = S_61.MyControl.TextInputMode.Insert;
            this.CoNo.Location = new System.Drawing.Point(186, 5);
            this.CoNo.MaxLength = 2;
            this.CoNo.Name = "CoNo";
            this.CoNo.Size = new System.Drawing.Size(26, 27);
            this.CoNo.TabIndex = 2;
            // 
            // CoName1
            // 
            this.CoName1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CoName1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.CoName1.CanReSize = true;
            this.CoName1.Font = new System.Drawing.Font("細明體", 12F);
            this.CoName1.GrayView = false;
            this.CoName1.InputMode = S_61.MyControl.TextInputMode.Insert;
            this.CoName1.Location = new System.Drawing.Point(218, 5);
            this.CoName1.MaxLength = 10;
            this.CoName1.Name = "CoName1";
            this.CoName1.ReadOnly = true;
            this.CoName1.Size = new System.Drawing.Size(87, 27);
            this.CoName1.TabIndex = 3;
            this.CoName1.TabStop = false;
            // 
            // ChStatus
            // 
            this.ChStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ChStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.ChStatus.CanReSize = true;
            this.ChStatus.Font = new System.Drawing.Font("細明體", 12F);
            this.ChStatus.GrayView = false;
            this.ChStatus.InputMode = S_61.MyControl.TextInputMode.Insert;
            this.ChStatus.Location = new System.Drawing.Point(186, 41);
            this.ChStatus.MaxLength = 2;
            this.ChStatus.Name = "ChStatus";
            this.ChStatus.ReadOnly = true;
            this.ChStatus.Size = new System.Drawing.Size(26, 27);
            this.ChStatus.TabIndex = 4;
            this.ChStatus.TabStop = false;
            // 
            // ChName
            // 
            this.ChName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ChName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.ChName.CanReSize = true;
            this.ChName.Font = new System.Drawing.Font("細明體", 12F);
            this.ChName.GrayView = false;
            this.ChName.InputMode = S_61.MyControl.TextInputMode.Insert;
            this.ChName.Location = new System.Drawing.Point(218, 41);
            this.ChName.MaxLength = 10;
            this.ChName.Name = "ChName";
            this.ChName.ReadOnly = true;
            this.ChName.Size = new System.Drawing.Size(87, 27);
            this.ChName.TabIndex = 5;
            this.ChName.TabStop = false;
            // 
            // lblT3
            // 
            this.lblT3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblT3.AutoSize = true;
            this.lblT3.BackColor = System.Drawing.Color.Transparent;
            this.lblT3.Font = new System.Drawing.Font("細明體", 12F);
            this.lblT3.Location = new System.Drawing.Point(416, 10);
            this.lblT3.Name = "lblT3";
            this.lblT3.Size = new System.Drawing.Size(40, 16);
            this.lblT3.TabIndex = 6;
            this.lblT3.Text = "備註";
            // 
            // ChMemo
            // 
            this.ChMemo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ChMemo.CanReSize = true;
            this.ChMemo.Font = new System.Drawing.Font("細明體", 12F);
            this.ChMemo.GrayView = false;
            this.ChMemo.InputMode = S_61.MyControl.TextInputMode.Insert;
            this.ChMemo.Location = new System.Drawing.Point(462, 5);
            this.ChMemo.MaxLength = 50;
            this.ChMemo.Name = "ChMemo";
            this.ChMemo.Size = new System.Drawing.Size(415, 27);
            this.ChMemo.TabIndex = 7;
            this.ChMemo.DoubleClick += new System.EventHandler(this.ChMemo_DoubleClick);
            // 
            // tableLayoutPnl_for_main3
            // 
            this.tableLayoutPnl_for_main3.ColumnCount = 6;
            this.tableLayoutPnl_for_main3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPnl_for_main3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl_for_main3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPnl_for_main3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPnl_for_main3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPnl_for_main3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPnl_for_main3.Controls.Add(this.lblT4, 1, 0);
            this.tableLayoutPnl_for_main3.Controls.Add(this.Count, 2, 0);
            this.tableLayoutPnl_for_main3.Controls.Add(this.btnGet, 3, 0);
            this.tableLayoutPnl_for_main3.Controls.Add(this.btnExit, 4, 0);
            this.tableLayoutPnl_for_main3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPnl_for_main3.Location = new System.Drawing.Point(0, 554);
            this.tableLayoutPnl_for_main3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPnl_for_main3.Name = "tableLayoutPnl_for_main3";
            this.tableLayoutPnl_for_main3.RowCount = 1;
            this.tableLayoutPnl_for_main3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPnl_for_main3.Size = new System.Drawing.Size(987, 49);
            this.tableLayoutPnl_for_main3.TabIndex = 2;
            // 
            // lblT4
            // 
            this.lblT4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblT4.AutoSize = true;
            this.lblT4.BackColor = System.Drawing.Color.Transparent;
            this.lblT4.Font = new System.Drawing.Font("細明體", 12F);
            this.lblT4.Location = new System.Drawing.Point(207, 16);
            this.lblT4.Name = "lblT4";
            this.lblT4.Size = new System.Drawing.Size(72, 16);
            this.lblT4.TabIndex = 0;
            this.lblT4.Text = "異動張數";
            // 
            // Count
            // 
            this.Count.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Count.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.Count.CanReSize = true;
            this.Count.Font = new System.Drawing.Font("細明體", 12F);
            this.Count.GrayView = false;
            this.Count.InputMode = S_61.MyControl.TextInputMode.Insert;
            this.Count.Location = new System.Drawing.Point(285, 11);
            this.Count.MaxLength = 10;
            this.Count.Name = "Count";
            this.Count.ReadOnly = true;
            this.Count.Size = new System.Drawing.Size(87, 27);
            this.Count.TabIndex = 1;
            this.Count.TabStop = false;
            this.Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnGet
            // 
            this.btnGet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGet.Font = new System.Drawing.Font("細明體", 12F);
            this.btnGet.Location = new System.Drawing.Point(378, 3);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(198, 43);
            this.btnGet.TabIndex = 2;
            this.btnGet.Text = "F9:確定異動";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExit.Font = new System.Drawing.Font("細明體", 12F);
            this.btnExit.Location = new System.Drawing.Point(582, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(198, 43);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "F11:結束";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // 應付票據批次異動_瀏覽
            // 
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(987, 625);
            this.Controls.Add(this.tableLayoutPnl_for_main1);
            this.Controls.Add(this.statusStrip1);
            this.KeyPreview = true;
            this.Name = "應付票據批次異動_瀏覽";
            this.Tag = "";
            this.Text = "應付票據批次異動";
            this.Load += new System.EventHandler(this.應付票據批次異動_瀏覽_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.應付票據批次異動_瀏覽_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPnl_for_main1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewT1)).EndInit();
            this.tableLayoutPnl_for_main2.ResumeLayout(false);
            this.tableLayoutPnl_for_main2.PerformLayout();
            this.tableLayoutPnl_for_main3.ResumeLayout(false);
            this.tableLayoutPnl_for_main3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private MyControl.tableLayoutPnl_for_main tableLayoutPnl_for_main1;
        private MyControl.dataGridViewT dataGridViewT1;
        private MyControl.tableLayoutPnl_for_main tableLayoutPnl_for_main2;
        private MyControl.lblT lblT1;
        private MyControl.lblT lblT2;
        private MyControl.TextBoxT CoNo;
        private MyControl.TextBoxT CoName1;
        private MyControl.TextBoxT ChStatus;
        private MyControl.TextBoxT ChName;
        private MyControl.lblT lblT3;
        private MyControl.TextBoxT ChMemo;
        private MyControl.tableLayoutPnl_for_main tableLayoutPnl_for_main3;
        private MyControl.lblT lblT4;
        private MyControl.TextBoxT Count;
        private MyControl.btnBrowT btnGet;
        private MyControl.btnBrowT btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn 支票號碼;
        private System.Windows.Forms.DataGridViewTextBoxColumn 廠商簡稱;
        private System.Windows.Forms.DataGridViewTextBoxColumn 開票帳戶;
        private System.Windows.Forms.DataGridViewTextBoxColumn 到期日;
        private System.Windows.Forms.DataGridViewTextBoxColumn 預兌日;
        private System.Windows.Forms.DataGridViewTextBoxColumn 票面金額;
        private System.Windows.Forms.DataGridViewTextBoxColumn 異動日期;
    }
}
