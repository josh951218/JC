using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;
using System.Data;
using System.IO;


namespace S_61.Basic
{
    public class RPT
    {
        DataTable dt;
        string path = "";

        public string office;
        public List<string[]> lobj = new List<string[]>();
        public List<string[]> lval = new List<string[]>();

        public RPT() { }
        public RPT(DataTable d, String p)
        {
            dt = d.Copy();
            path = p;
        }

        void oRptInit()
        {
            lval.Add(new string[] { "date", Common.User_DateTime == 1 ? "民國" : "西元" });
            lval.Add(new string[] { "金額小數",Common.金額小數.ToString()});


            Common.FrmReport = new Report.Frmreport();
            Common.FrmReport.rpt1 = new ReportDocument();
            Common.FrmReport.rpt1.Load(path);
            Common.FrmReport.rpt1.SetDataSource(dt);

            if (Common.Sql_LogMod == 2)//混合驗證
            {
                Common.logOnInfo.ConnectionInfo.IntegratedSecurity = true;
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in Common.FrmReport.rpt1.Database.Tables)
                {
                    table.ApplyLogOnInfo(Common.logOnInfo);
                }
                Common.FrmReport.rpt1.Refresh();
            }
            else if (Common.Sql_LogMod == 1)//SQL驗證
            {
                Common.logOnInfo.ConnectionInfo.IntegratedSecurity = false;
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in Common.FrmReport.rpt1.Database.Tables)
                {
                    table.ApplyLogOnInfo(Common.logOnInfo);
                }
                Common.FrmReport.rpt1.Refresh();
            }
            TextObject myFieldTitleName;
            List<TextObject> Txt = Common.FrmReport.rpt1.ReportDefinition.ReportObjects.OfType<TextObject>().ToList();
            for (int i = 0; i < lobj.Count; i++)
            {
                if (Txt.Find(t => t.Name == lobj[i][0]) != null)
                {
                    myFieldTitleName = (TextObject)Common.FrmReport.rpt1.ReportDefinition.ReportObjects[lobj[i][0]];
                    myFieldTitleName.Text = lobj[i][1];
                }
            }
            List<ParameterField> Num = Common.FrmReport.rpt1.ParameterFields.OfType<ParameterField>().ToList();
            for (int i = 0; i < lval.Count; i++)
            {
                if (Num.Find(t => t.Name == lval[i][0]) != null)
                {
                    if (lval[i][0] == "金額小數") Common.FrmReport.rpt1.SetParameterValue(lval[i][0], lval[i][1].ToDecimal());
                    else Common.FrmReport.rpt1.SetParameterValue(lval[i][0], lval[i][1]);
                }
            }
            Common.FrmReport.cview.ReportSource = Common.FrmReport.rpt1;
        }

        void doDispose()
        {
            if (Common.FrmReport == null) return;
            Common.FrmReport.rpt1.Close();
            Common.FrmReport.cview.Dispose();
            Common.FrmReport.rpt1.Dispose();
        }

        public void PreView()
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("報表檔案不存在\n" + path, "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dt.Rows.Count == 0)
                MessageBox.Show("查無資料", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                oRptInit();
                Common.FrmReport.ShowDialog();
                Common.FrmReport.Dispose();
            }
        }

        public void Print()
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("報表檔案不存在\n" + path, "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dt.Rows.Count == 0)
                MessageBox.Show("查無資料", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                oRptInit();
                Common.FrmReport.button1_Click(null, null);
            }
            doDispose();
        }

        public void Word()
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("報表檔案不存在\n" + path, "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dt.Rows.Count == 0)
                MessageBox.Show("查無資料", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                oRptInit();
                Random Rn = new Random();
                int intRn = Rn.Next(1000);
                if (!System.IO.Directory.Exists(Application.StartupPath + "\\temp"))
                {
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "\\temp");
                }
                Common.FrmReport.rpt1.ExportToDisk(ExportFormatType.WordForWindows, Application.StartupPath + "\\temp\\" + office + intRn + ".doc");
                Process.Start(Application.StartupPath + "\\temp\\" + office + intRn + ".doc");
            }
            doDispose();
        }

        public void Excel()
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("報表檔案不存在\n" + path, "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dt.Rows.Count == 0)
                MessageBox.Show("查無資料", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                oRptInit();
                string 使用者變數 = Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.User);
                try
                {
                    Random Rn = new Random();
                    int intRn = Rn.Next(1000);
                    if (!System.IO.Directory.Exists(Application.StartupPath + "\\temp"))
                    {
                        System.IO.Directory.CreateDirectory(Application.StartupPath + "\\temp");
                    }
                    Environment.SetEnvironmentVariable("TEMP", Application.StartupPath + "\\temp", EnvironmentVariableTarget.User);
                    Common.FrmReport.rpt1.ExportToDisk(ExportFormatType.Excel, Application.StartupPath + "\\temp\\" + office + intRn + ".xls");
                    Process.Start(Application.StartupPath + "\\temp\\" + office + intRn + ".xls");
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Environment.SetEnvironmentVariable("TEMP", 使用者變數, EnvironmentVariableTarget.User);
                    doDispose();
                }
            }
        }
    }
}
