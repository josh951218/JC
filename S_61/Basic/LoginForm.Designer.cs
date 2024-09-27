namespace S_61.Basic
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.tableLayoutPnl1 = new S_61.MyControl.tableLayoutPnl();
            this.tableLayoutPnl2 = new S_61.MyControl.tableLayoutPnl();
            this.ScName = new System.Windows.Forms.TextBox();
            this.ScPass = new System.Windows.Forms.TextBox();
            this.tableLayoutPnl1.SuspendLayout();
            this.tableLayoutPnl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPnl1
            // 
            this.tableLayoutPnl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tableLayoutPnl1.BackgroundImage")));
            this.tableLayoutPnl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPnl1.ColumnCount = 3;
            this.tableLayoutPnl1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.89931F));
            this.tableLayoutPnl1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.46726F));
            this.tableLayoutPnl1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.63343F));
            this.tableLayoutPnl1.Controls.Add(this.tableLayoutPnl2, 1, 1);
            this.tableLayoutPnl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPnl1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPnl1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPnl1.Name = "tableLayoutPnl1";
            this.tableLayoutPnl1.RowCount = 2;
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.64063F));
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.35938F));
            this.tableLayoutPnl1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPnl1.Size = new System.Drawing.Size(1024, 768);
            this.tableLayoutPnl1.TabIndex = 4;
            this.tableLayoutPnl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPnl1_MouseClick);
            // 
            // tableLayoutPnl2
            // 
            this.tableLayoutPnl2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPnl2.ColumnCount = 1;
            this.tableLayoutPnl2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPnl2.Controls.Add(this.ScName, 0, 1);
            this.tableLayoutPnl2.Controls.Add(this.ScPass, 0, 2);
            this.tableLayoutPnl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPnl2.Location = new System.Drawing.Point(623, 626);
            this.tableLayoutPnl2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPnl2.Name = "tableLayoutPnl2";
            this.tableLayoutPnl2.RowCount = 4;
            this.tableLayoutPnl2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.69444F));
            this.tableLayoutPnl2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.55556F));
            this.tableLayoutPnl2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.55556F));
            this.tableLayoutPnl2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.80556F));
            this.tableLayoutPnl2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPnl2.Size = new System.Drawing.Size(148, 142);
            this.tableLayoutPnl2.TabIndex = 0;
            // 
            // ScName
            // 
            this.ScName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ScName.BackColor = System.Drawing.Color.White;
            this.ScName.Font = new System.Drawing.Font("細明體", 12F);
            this.ScName.Location = new System.Drawing.Point(3, 44);
            this.ScName.Name = "ScName";
            this.ScName.Size = new System.Drawing.Size(134, 27);
            this.ScName.TabIndex = 1;
            this.ScName.Enter += new System.EventHandler(this.ScName_Enter);
            this.ScName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScName_KeyDown);
            this.ScName.Leave += new System.EventHandler(this.ScName_Leave);
            // 
            // ScPass
            // 
            this.ScPass.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ScPass.BackColor = System.Drawing.Color.White;
            this.ScPass.Font = new System.Drawing.Font("細明體", 12F);
            this.ScPass.Location = new System.Drawing.Point(3, 88);
            this.ScPass.Name = "ScPass";
            this.ScPass.PasswordChar = '*';
            this.ScPass.Size = new System.Drawing.Size(134, 27);
            this.ScPass.TabIndex = 2;
            this.ScPass.Enter += new System.EventHandler(this.ScName_Enter);
            this.ScPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScName_KeyDown);
            this.ScPass.Leave += new System.EventHandler(this.ScName_Leave);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPnl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "LoginForm";
            this.Text = "loginForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginForm_KeyDown);
            this.Resize += new System.EventHandler(this.LoginForm_Resize);
            this.tableLayoutPnl1.ResumeLayout(false);
            this.tableLayoutPnl2.ResumeLayout(false);
            this.tableLayoutPnl2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MyControl.tableLayoutPnl tableLayoutPnl1;
        private MyControl.tableLayoutPnl tableLayoutPnl2;
        private System.Windows.Forms.TextBox ScName;
        private System.Windows.Forms.TextBox ScPass;


    }
}