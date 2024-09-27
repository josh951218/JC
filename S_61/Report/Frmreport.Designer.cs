namespace S_61.Report
{
    partial class Frmreport
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
            this.cview = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.rpt1 = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            this.btnPrint = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.cview.ActiveViewIndex = -1;
            this.cview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cview.Cursor = System.Windows.Forms.Cursors.Default;
            this.cview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cview.Location = new System.Drawing.Point(0, 0);
            this.cview.Name = "crystalReportViewer1";
            this.cview.ShowGroupTreeButton = false;
            this.cview.ShowParameterPanelButton = false;
            this.cview.ShowPrintButton = false;
            this.cview.ShowRefreshButton = false;
            this.cview.ShowTextSearchButton = false;
            this.cview.Size = new System.Drawing.Size(792, 566);
            this.cview.TabIndex = 0;
            this.cview.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(324, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(57, 23);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "列印";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.button1_Click);
            // 
            // Frmreport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.cview);
            this.KeyPreview = true;
            this.Name = "Frmreport";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frmreport_FormClosing);
            this.Load += new System.EventHandler(this.Frmreport_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Frmreport_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion
        public CrystalDecisions.CrystalReports.Engine.ReportDocument rpt1;
        public CrystalDecisions.Windows.Forms.CrystalReportViewer cview;
        private System.Windows.Forms.Button btnPrint;

    }
}