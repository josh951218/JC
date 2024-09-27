namespace S_61.Basic
{
    partial class FrmDataBackUP
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
            this.tableLayoutPnl1 = new S_61.MyControl.tableLayoutPnl();
            this.btnBackUp = new S_61.MyControl.btnBrowT();
            this.btnRestore = new S_61.MyControl.btnBrowT();
            this.tableLayoutPnl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPnl1
            // 
            this.tableLayoutPnl1.ColumnCount = 3;
            this.tableLayoutPnl1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPnl1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPnl1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPnl1.Controls.Add(this.btnBackUp, 1, 1);
            this.tableLayoutPnl1.Controls.Add(this.btnRestore, 1, 3);
            this.tableLayoutPnl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPnl1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPnl1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPnl1.Name = "tableLayoutPnl1";
            this.tableLayoutPnl1.RowCount = 5;
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.34267F));
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.98599F));
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.34267F));
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.98599F));
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.34267F));
            this.tableLayoutPnl1.Size = new System.Drawing.Size(540, 388);
            this.tableLayoutPnl1.TabIndex = 0;
            // 
            // btnBackUp
            // 
            this.btnBackUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBackUp.Font = new System.Drawing.Font("細明體", 12F);
            this.btnBackUp.Location = new System.Drawing.Point(182, 93);
            this.btnBackUp.Name = "btnBackUp";
            this.btnBackUp.Size = new System.Drawing.Size(174, 52);
            this.btnBackUp.TabIndex = 0;
            this.btnBackUp.Text = "備份-資料庫";
            this.btnBackUp.UseVisualStyleBackColor = true;
            this.btnBackUp.Click += new System.EventHandler(this.btnBackUp_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRestore.Font = new System.Drawing.Font("細明體", 12F);
            this.btnRestore.Location = new System.Drawing.Point(182, 241);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(174, 52);
            this.btnRestore.TabIndex = 1;
            this.btnRestore.Text = "還原-資料庫";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // FrmDataBackUP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 388);
            this.Controls.Add(this.tableLayoutPnl1);
            this.Name = "FrmDataBackUP";
            this.Text = "資料庫備份";
            this.Load += new System.EventHandler(this.FrmDataBackUP_Load);
            this.tableLayoutPnl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MyControl.tableLayoutPnl tableLayoutPnl1;
        private MyControl.btnBrowT btnBackUp;
        private MyControl.btnBrowT btnRestore;
    }
}