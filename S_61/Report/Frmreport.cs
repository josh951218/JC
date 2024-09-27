using System;
using System.Windows.Forms;
using System.Drawing.Printing;
using S_61.Basic;

namespace S_61.Report
{
    public partial class Frmreport : Form
    {
        public Frmreport()
        {
            InitializeComponent();
            Common.FrmReport = this;
        }

        private void Frmreport_Load(object sender, EventArgs e)
        {
            SetParameter.FormParameters(this);
            this.MaximizeBox = true;
            string str = rpt1.FileName.ToUpper();
            if (str.Contains("REPORT"))
            {
                int i = str.IndexOf("REPORT");
                this.Text = str.Substring(i + 7);
            }
        }

        private void Frmreport_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            PrintDialog print = new PrintDialog();
            print.AllowSelection = true;
            print.AllowSomePages = true;
            print.PrinterSettings.Collate = false;
            if (print.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    rpt1.PrintOptions.PrinterName = print.PrinterSettings.PrinterName;
                    rpt1.PrintToPrinter(print.PrinterSettings.Copies, print.PrinterSettings.Collate, print.PrinterSettings.FromPage, print.PrinterSettings.ToPage);
                }
                catch
                {
                    PrinterSettings printerSettings = new PrinterSettings();
                    PageSettings pageSettings = new PageSettings();
                    rpt1.PrintOptions.CopyTo(printerSettings, pageSettings);
                    printerSettings.PrinterName = print.PrinterSettings.PrinterName;
                    printerSettings.PrintRange = print.PrinterSettings.PrintRange;
                    printerSettings.Collate = print.PrinterSettings.Collate;
                    printerSettings.Copies = print.PrinterSettings.Copies;
                    printerSettings.FromPage = print.PrinterSettings.FromPage;
                    printerSettings.ToPage = print.PrinterSettings.ToPage;
                    pageSettings.PrinterSettings = printerSettings;
                    rpt1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                    rpt1.PrintToPrinter(printerSettings, pageSettings, false);
                }
            }
            print.Dispose();
        }

        private void Frmreport_FormClosing(object sender, FormClosingEventArgs e)
        {
            rpt1.Close();
            cview.Dispose();
            rpt1.Dispose();
        }
    }
}
