using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.s_4系統維護
{
    public partial class 系統參數設定 : FormT
    {
        List<btnT> SC;
        List<btnT> Others;
        List<TextBox> Txt;
        SqlTransaction tran;
        public 系統參數設定()
        {
            InitializeComponent();
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT> { btnModify,  btnExit };
            Txt = new List<TextBox> { stcco, CoNo,  DeCi };
        }

        private void 系統參數設定_Load(object sender, EventArgs e)
        {
            ChkLine.BackColor = ChkDis.BackColor = rd1.BackColor = rd2.BackColor = rd3.BackColor = rd4.BackColor = rd5.BackColor = rd6.BackColor = ck1.BackColor =linkLabel1.BackColor= Color.FromArgb(215, 227, 239);
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            load();
        }

        void load()
        {
            try
            {
                using (SqlConnection cn  = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from chksys";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        stcco.Text = reader["stcco"].ToString();
                        CoNo.Text = reader["CoNo"].ToString();
                        ChkYear2.Text = reader["ChkYear2"].ToString();
                        ChkYear1.Text = reader["ChkYear1"].ToString();
                        DeCi.Text = reader["DeCi"].ToString();
                        if (reader["chkline"].ToDecimal() == 1)
                            ChkLine.Checked = true;
                        else
                            ChkLine.Checked = false;
                        if (reader["chkdis"].ToDecimal() == 1)
                            ChkDis.Checked = true;
                        else
                            ChkDis.Checked = false;
                        if (reader["noadd"].ToDecimal() == 1)
                            rd3.Checked = true;
                        else
                            rd4.Checked = true;
                        if (reader["sysdate"].ToDecimal() == 1)
                            rd1.Checked = true;
                        else
                            rd2.Checked = true;
                        if (reader["autoget"].ToDecimal() == 1)
                            ck1.Checked = true;
                        else
                            ck1.Checked = false;
                    }
                    CHK.GetCoName(CoNo, CoName1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            Txt.ForEach(r => r.ReadOnly = false);
            ChkYear1.ReadOnly = false;
            ChkLine.Enabled = ChkDis.Enabled = true;
            rd1.Enabled = rd2.Enabled = rd3.Enabled = rd4.Enabled = rd5.Enabled = rd6.Enabled =  ck1.Enabled= true;
            bool CanChamge = true;
            using (SqlConnection cn  = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "select Top(1) lono from lodgm";
                if (CanChamge)
                    CanChamge = cmd.ExecuteScalar().IsNullOrEmpty() ? true : false;
                cmd.CommandText = "select Top(1) cano from carry";
                if (CanChamge)
                    CanChamge = cmd.ExecuteScalar().IsNullOrEmpty() ? true : false;
                cmd.CommandText = "select Top(1) reno from remiti";
                if (CanChamge)
                    CanChamge = cmd.ExecuteScalar().IsNullOrEmpty() ? true : false;
                cmd.CommandText = "select Top(1) reno from remito";
                if (CanChamge)
                    CanChamge = cmd.ExecuteScalar().IsNullOrEmpty() ? true : false;
            }
            if (!CanChamge)
            {
                if (rd3.Checked || rd4.Checked) rd5.Enabled = rd6.Enabled = false;
                if (rd5.Checked || rd6.Checked) rd3.Enabled = rd4.Enabled = false;
                ChkYear1.ReadOnly =  true;
            }
            stcco.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                tran = cn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    cmd.CommandText = "update chksys set "
                        + " stcco='" + stcco.Text.Trim() + "',"
                        + " cono='" + CoNo.Text.Trim() + "',"
                        + " chkyear1='" + ChkYear1.Text.Trim() + "',"
                        + " chkyear2='" + ChkYear2.Text.Trim() + "',"
                        + " deci='" + DeCi.Text.Trim() + "',";
                    if (ChkLine.Checked)
                        cmd.CommandText += " ChkLine=1,";
                    else
                        cmd.CommandText += " ChkLine=0,";
                    if (ChkDis.Checked)
                        cmd.CommandText += " ChkDis=1,";
                    else
                        cmd.CommandText += " ChkDis=0,";
                    if (rd1.Checked)
                        cmd.CommandText += " sysdate=1,";
                    else
                        cmd.CommandText += " sysdate=2,";
                    if (rd3.Checked)
                        cmd.CommandText += " noadd=1,";
                    else
                        cmd.CommandText += " noadd=2,";
                    if (ck1.Checked)
                        cmd.CommandText += " autoget=1";
                    else
                        cmd.CommandText += " autoget=2";
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.ToString());
                }
                btnCancel_Click(null,null);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            Txt.ForEach(r => r.ReadOnly = true);
            ChkLine.Enabled = ChkDis.Enabled = false;
            ck1.Enabled = false;
            rd1.Enabled = rd2.Enabled = rd3.Enabled = rd4.Enabled = rd5.Enabled = rd6.Enabled = false;
            load();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
            {
                //讀系統參數設定
                string str = "select * from chksys";
                using (SqlDataAdapter da = new SqlDataAdapter(str, conn))
                {
                    Common.dtSysSettings.Clear();
                    da.Fill(Common.dtSysSettings);
                    if (Common.dtSysSettings.Rows.Count > 0)
                    {
                        Common.listSysSettings.Clear();
                        Common.listSysSettings = Common.dtSysSettings.AsEnumerable().ToList();

                        Common.CoNo = Common.dtSysSettings.Rows[0]["CoNo"].ToString().Trim();
                        Common.系統民國 = Common.dtSysSettings.Rows[0]["chkyear1"].ToString().Trim();
                        Common.系統西元 = Common.dtSysSettings.Rows[0]["chkyear2"].ToString().Trim();
                        Common.User_DateTime = Convert.ToInt32(Common.dtSysSettings.Rows[0]["sysdate"].ToString());
                        Common.傳票編碼方式 = Convert.ToInt32(Common.dtSysSettings.Rows[0]["noadd"].ToString());
                        Common.金額小數 = Convert.ToInt32( Common.dtSysSettings.Rows[0]["deci"].ToDecimal());
                        Common.chkline = Common.dtSysSettings.Rows[0]["chkline"].ToDecimal();
                        Common.chkdis = Common.dtSysSettings.Rows[0]["chkdis"].ToDecimal();
                        Common.自動產生匯率 =Convert.ToInt32( Common.dtSysSettings.Rows[0]["autoget"].ToDecimal());
                    }
                    else
                    {
                        Common.listSysSettings.Clear();
                    }
                }
            }
            this.Dispose();
            this.Close();
        }

        private void 系統參數設定_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D2:
                case Keys.NumPad2:
                    btnModify.PerformClick();
                    break;
                case Keys.D0:
                case Keys.NumPad0:
                case Keys.F11:
                    btnExit.PerformClick();
                    break;
                case Keys.F9:
                    btnSave.PerformClick();
                    break;
                case Keys.F4:
                    btnCancel.Focus();
                    btnCancel.PerformClick();
                    break;
            }
        }

        private void CoNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CoNo_OpemFrm(CoNo, CoName1);
        }

        private void CoNo_Validating(object sender, CancelEventArgs e)
        {
            if (!CHK.CoNo_Validating(CoNo, CoName1))
            {
                e.Cancel = true;
                CHK.CoNo_OpemFrm(CoNo, CoName1);
            }
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://rate.bot.com.tw/Pages/Static/UIP003.zh-TW.htm"); 
        }

        private void ChkYear1_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused ) return;
            if (ChkYear1.Text.Trim() == "")
            {
                MessageBox.Show("『系統票據年度』不可為空，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            ChkYear2.Text = Date.ToUSDate(ChkYear1.Text.Trim() + "0101").Substring(0, 4);
        }
    }
}
