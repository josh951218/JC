using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace S_61.s_3基本資料
{
    public partial class 客戶建檔作業 : FormT
    {
        SqlTransaction tran;
        List<Control> CC = new List<Control>();
        List<DataRow> list = new List<DataRow>();
        DataTable dt = new DataTable();
        DataRow dr;
        string current;
        int temp = 0;
        string btnState = "";
        string BeforeText;
        List<btnT> SC;
        List<btnT> Others;
        List<Control> TXTs;
        List<Control> TXTReadOnly;

        string machine = "";

        public 客戶建檔作業()
        {
            InitializeComponent();
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT>{btnTop, btnPrior, btnNext, btnBottom, btnAppend, btnDuplicate, btnModify, btnDelete, btnPrint, btnBrow, btnExit};
            TXTs = new List<Control> { CuNo, CuName2, CuName1, Xa1No, CuPareNo, CuIme, CuX1No, CuEmNo1, CuPer1, CuTel1, CuAtel1, CuPer2, CuTel2 ,
                CuAtel2,CuPer,CuFax1,CuAtel3,CuAddr1,CuR1,CuAddr2,CuR2,CuAddr3,CuR3,CuCredit,CuUno,CuDisc,CuDisc1,CuX3No,CuX3No1,CuX4No,CuX4No1,
                CuX5No,CuX5No1,CuEmail,CuWww,CuDatep,CuEngname,CuEngr1,CuEngaddr,CuMemo1,CuInvoName,CuArea,CuX2No,CuUdf1,CuUdf2,CuUdf3,CuUdf4,
                CuUdf5,CuBirth,CuIdNo,CuPer1_1,CuBlood,CuTel1_1,CuAtel1_1,CuFax1_1,CuAddr1_1,CuR1_1,CuEmail_1,CuWww_1,CuMemo1_1,CuMemo2_1
            };
            TXTReadOnly = new List<Control> { Xa1Name, CuX1Name, CuPareName, CuEmName, CuX2Name, CuXbh, CuAdvamt, CuX3Name, CuX3Name1,
                CuX4Name,CuX4Name1,CuX5Name,CuX5Name1,CuDate,CuDate_1,CuLastday,CuLastday_1,CuPoint
            };

            CuCredit.NumFirst = Common.nFirst;
            CuXbh.NumFirst = Common.nFirst;
            CuAdvamt.NumFirst = Common.nFirst;

            CuCredit.NumLast = Convert.ToInt32(Common.金額小數.ToString());
            CuXbh.NumLast = Convert.ToInt32(Common.金額小數.ToString());
            CuAdvamt.NumLast = Convert.ToInt32(Common.金額小數.ToString());
        }

        private void FrmCust1_Load(object sender, EventArgs e)
        {
            Common.取得浮動連線字串(Common.使用者預設公司);
            //多國語
            //Model.SetLanguage.list.Clear();
            //Model.SetLanguage.CastControl(this);
            //CC = Model.SetLanguage.list;
            ////Model.SetLanguage.CreateXml(this, CC);
            //Model.SetLanguage.Change(CC, MainForm.Lan, "Cust1");
            Basic.SetParameter.TabControlItemSize(tabControl1);
            Basic.SetParameter.SetBtnEnable(Others);
            Basic.SetParameter.SetBtnDisable(SC);
            Basic.SetParameter.SetRadioDisable(pnlCuSlevel);
            Basic.SetParameter.SetRadioDisable(pnlCuSlevel1);

            //if (Common.Sys_StockKind == 2) tabControl1.SelectTab(2);
            //else tabControl1.SelectTab(0);


            //日期格式

            switch (Common.User_DateTime)
            {
                case 1:
                    CuBirth.MaxLength = 7;
                    CuLastday.MaxLength = 7;
                    CuDate.MaxLength = 7;
                    CuLastday_1.MaxLength = 7;
                    CuDate_1.MaxLength = 7;
                    CuDatep.MaxLength = 7;
                    break;
                case 2:
                    CuBirth.MaxLength = 8;
                    CuBirth.MaxLength = 8;
                    CuLastday.MaxLength = 8;
                    CuDate.MaxLength = 8;
                    CuLastday_1.MaxLength = 8;
                    CuDate_1.MaxLength = 8;
                    CuDatep.MaxLength = 8;
                    break;
            }

            //載入資料庫，預設顯示第一筆
            loadDB();
            if (dt.Rows.Count > 0)
            {
                dr = list.First();
                writeToTxt(dr);
            }

            btnAppend.Focus();
        }

        public void loadDB()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    string sql = "select * from Cust order by cuno COLLATE Chinese_Taiwan_Stroke_BIN";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        dt.Clear(); list.Clear();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            list.Clear();
                            list = dt.AsEnumerable().ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        public void reLoadDB(string strBrow)
        {
            dr = list.Find(r => r.Field<string>("CuNo") == strBrow);
            if (dr != null)
            {
                writeToTxt(dr);
            }
        }



        private void writeToTxt(DataRow dr)
        {
            if (dr != null)
            {
                //表頭
                CuNo.Text = dr["cuno"].ToString();
                CuIme.Text = dr["cuime"].ToString();
                CuName2.Text = dr["cuname2"].ToString();
                CuName1.Text = dr["cuname1"].ToString();
                pVar.Xa01Validate(dr["cuxa1no"].ToString(), Xa1No, Xa1Name);
                pVar.XX01Validate(dr["cux1no"].ToString(), CuX1No, CuX1Name);
                pVar.CuPareValidate(dr["cupareno"].ToString(), CuPareNo, CuPareName);
                pVar.EmplValidate(dr["cuemno1"].ToString(), CuEmNo1, CuEmName);
                //分頁一
                CuPer1.Text = dr["cuper1"].ToString();
                CuPer2.Text = dr["cuper2"].ToString();
                CuPer.Text = dr["cuper"].ToString();
                CuTel1.Text = dr["cutel1"].ToString();
                CuTel2.Text = dr["cutel2"].ToString();
                CuFax1.Text = dr["cufax1"].ToString();
                CuAtel1.Text = dr["cuatel1"].ToString();
                CuAtel2.Text = dr["cuatel2"].ToString();
                CuAtel3.Text = dr["cuatel3"].ToString();
                CuAddr1.Text = dr["cuaddr1"].ToString();
                CuR1.Text = dr["cur1"].ToString();
                CuAddr2.Text = dr["cuaddr2"].ToString();
                CuR2.Text = dr["cur2"].ToString();
                CuAddr3.Text = dr["cuaddr3"].ToString();
                CuR3.Text = dr["cur3"].ToString();
                //現有預收餘額:dt["CuAdvamt"]
                decimal dtemp = 0;
                CuAdvamt.Text = dr["CuAdvamt"].ToString();
                decimal.TryParse(CuAdvamt.Text, out dtemp);
                CuAdvamt.Text = dtemp.ToString("f" + CuAdvamt.NumLast);
                //信用額度
                dtemp = 0;
                CuCredit.Text = dr["CuCredit"].ToString();
                decimal.TryParse(CuCredit.Text, out dtemp);
                CuCredit.Text = dtemp.ToString("f" + CuCredit.NumLast);
                //計算信用額度的餘額：
                //餘額[CuXbh] = 信用額度[CuCredit]-現有應收帳款["Cureceiv"]
                dtemp = 0;
                decimal dcredit = 0, dreceiv = 0;
                decimal.TryParse(dr["CuCredit"].ToString(), out dcredit);
                decimal.TryParse(dr["Cureceiv"].ToString(), out dreceiv);
                dtemp = dcredit - dreceiv;
                CuXbh.Text = dtemp.ToString("f" + CuXbh.NumLast);
                //建議售價
                switch (dr["cuslevel"].ToString())
                {
                    case "1":
                        radio1.Checked = true;
                        radio11.Checked = true; break;
                    case "2":
                        radio2.Checked = true;
                        radio12.Checked = true; break;
                    case "3":
                        radio3.Checked = true;
                        radio13.Checked = true; break;
                    case "4":
                        radio4.Checked = true;
                        radio14.Checked = true; break;
                    case "5":
                        radio5.Checked = true;
                        radio15.Checked = true; break;
                    case "6":
                        radio6.Checked = true;
                        radio16.Checked = true; break;
                    default:
                        radio6.Checked = true;
                        radio16.Checked = true; break;
                }
                CuDisc.Text = dr["cudisc"].ToString();
                CuDisc1.Text = dr["cudisc"].ToString();
                CuEmail.Text = dr["cuemail"].ToString();
                CuWww.Text = dr["cuwww"].ToString();
                CuUno.Text = dr["cuuno"].ToString();
                pVar.XX03Validate(dr["CuX3No"].ToString(), CuX3No, CuX3Name);
                pVar.XX03Validate(dr["CuX3No"].ToString(), CuX3No1, CuX3Name1);
                pVar.XX04Validate(dr["CuX4No"].ToString(), CuX4No, CuX4Name);
                pVar.XX04Validate(dr["CuX4No"].ToString(), CuX4No1, CuX4Name1);
                pVar.XX05Validate(dr["CuX5No"].ToString(), CuX5No, CuX5Name);
                pVar.XX05Validate(dr["CuX5No"].ToString(), CuX5No1, CuX5Name1);
                //分頁二
                CuEngname.Text = dr["cuengname"].ToString();
                CuEngaddr.Text = dr["cuengaddr"].ToString();
                CuEngr1.Text = dr["cuengr1"].ToString();
                CuMemo1.Text = dr["cumemo1"].ToString();
                CuInvoName.Text = dr["cuinvoname"].ToString();
                CuArea.Text = dr["cuarea"].ToString();
                CuUdf1.Text = dr["cuudf1"].ToString();
                CuUdf2.Text = dr["cuudf2"].ToString();
                CuUdf3.Text = dr["cuudf3"].ToString();
                CuUdf4.Text = dr["cuudf4"].ToString();
                CuUdf5.Text = dr["cuudf5"].ToString();
                pVar.XX02Validate(dr["cux2no"].ToString(), CuX2No, CuX2Name);
                if (Common.User_DateTime == 1)
                {
                    CuDate.Text = dr["CuDate"].ToString();
                    CuDate_1.Text = dr["CuDate"].ToString();
                    CuLastday.Text = dr["CuLastday"].ToString();
                    CuLastday_1.Text = dr["CuLastday"].ToString();
                    CuBirth.Text = dr["cubirth"].ToString();
                }
                else
                {
                    CuDate.Text = dr["CuDate1"].ToString();
                    CuDate_1.Text = dr["CuDate1"].ToString();
                    CuLastday.Text = dr["CuLastday1"].ToString();
                    CuLastday_1.Text = dr["CuLastday1"].ToString();
                    CuBirth.Text = dr["cubirth1"].ToString();
                }
                //分頁三
                CuIdNo.Text = dr["cuidno"].ToString();
                CuSex.Text = dr["cusex"].ToString();
                CuPer1_1.Text = dr["cuper1"].ToString();
                CuBlood.Text = dr["cublood"].ToString();
                CuTel1_1.Text = dr["cutel1"].ToString();
                CuAtel1_1.Text = dr["cuatel1"].ToString();
                CuFax1_1.Text = dr["cufax1"].ToString();
                CuAddr1_1.Text = dr["cuaddr1"].ToString();
                CuR1_1.Text = dr["cur1"].ToString();
                CuEmail_1.Text = dr["cuemail"].ToString();
                CuWww_1.Text = dr["cuwww"].ToString();
                CuMemo1_1.Text = dr["cumemo1"].ToString();
                CuMemo2_1.Text = dr["cumemo2"].ToString();
                CuDatep.Text = dr["CuDatep"].ToString();
                CuPoint.Text = dr["CuPoint"].ToDecimal().ToString("f0");
            }
            else
            {
                //清空所有欄位
                Basic.SetParameter.TextBoxClear(TXTs);
                Basic.SetParameter.TextBoxClear(TXTReadOnly);
            }
        }

        private DataRow getCurrentDataRow()
        {
            return list.Find(o => o.Field<string>("CuNo") == (CuNo.Text.Trim()));
        }

        private DataRow getCurrentDataRow(string s)
        {
            return list.Find(o => o.Field<string>("CuNo") == (s));
        }



        ////功能按鈕
        private void btnTop_Click(object sender, EventArgs e)
        {
            loadDB();
            if (list.Count > 0)
            {
                dr = list.First();
                writeToTxt(dr);
            }
            btnTop.Enabled = false;
            btnPrior.Enabled = false;
            btnNext.Enabled = true;
            btnBottom.Enabled = true;
        }

        private void btnPrior_Click(object sender, EventArgs e)
        {
            current = CuNo.Text.Trim();
            dr = getCurrentDataRow();
            temp = list.IndexOf(dr);
            loadDB();
            if (list.Count > 0)
            {
                dr = getCurrentDataRow(current);
                int i = list.IndexOf(dr);
                if (i == -1)
                {
                    if (temp == 0)
                    {
                        dr = list.First();
                        writeToTxt(dr);
                        MessageBox.Show("已最上一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnTop.Enabled = false;
                        btnPrior.Enabled = false;
                        btnNext.Enabled = true;
                        btnBottom.Enabled = true;
                        return;
                    }
                    else
                    {
                        dr = list[--temp];
                        writeToTxt(dr);
                        btnNext.Enabled = true;
                        btnBottom.Enabled = true;
                        return;
                    }
                }
                if (i > 0)
                {
                    dr = list[--i];
                    writeToTxt(dr);
                    btnNext.Enabled = true;
                    btnBottom.Enabled = true;
                }
                else
                {
                    MessageBox.Show("已最上一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnTop.Enabled = false;
                    btnPrior.Enabled = false;
                    btnNext.Enabled = true;
                    btnBottom.Enabled = true;
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            current = CuNo.Text.Trim();
            dr = getCurrentDataRow();
            temp = list.IndexOf(dr);
            loadDB();
            if (list.Count > 0)
            {
                dr = getCurrentDataRow(current);
                int i = list.IndexOf(dr);
                if (i == -1)
                {
                    if (temp >= list.Count)
                    {
                        dr = list.Last();
                        writeToTxt(dr);
                        MessageBox.Show("已至最後一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnTop.Enabled = true;
                        btnPrior.Enabled = true;
                        btnNext.Enabled = false;
                        btnBottom.Enabled = false;
                        return;
                    }
                    else
                    {
                        dr = list[++i];
                        writeToTxt(dr);
                        btnTop.Enabled = true;
                        btnPrior.Enabled = true;
                        return;
                    }
                }
                if (i < list.Count - 1)
                {
                    dr = list[++i];
                    writeToTxt(dr);
                    btnTop.Enabled = true;
                    btnPrior.Enabled = true;
                }
                else
                {
                    MessageBox.Show("已至最後一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnTop.Enabled = true;
                    btnPrior.Enabled = true;
                    btnNext.Enabled = false;
                    btnBottom.Enabled = false;
                }
            }
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            loadDB();
            if (list.Count > 0)
            {
                dr = list.Last();
                writeToTxt(dr);
            }
            btnTop.Enabled = true;
            btnPrior.Enabled = true;
            btnNext.Enabled = false;
            btnBottom.Enabled = false;
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            btnState = ((Button)sender).Name.Substring(3);
            current = CuNo.Text.Trim();

            Basic.SetParameter.SetTxtEnable(TXTs);
            Basic.SetParameter.TextBoxClear(TXTReadOnly);
            Basic.SetParameter.SetBtnEnable(SC);
            Basic.SetParameter.SetBtnDisable(Others);
            Basic.SetParameter.SetRadioEnable(pnlCuSlevel);
            Basic.SetParameter.SetRadioEnable(pnlCuSlevel1);

            //新增時，清空欄位,焦點於CuNo
            //載入某些欄位的預設值
            CuNo.ReadOnly = false;
            CuNo.Focus();
            setTxtWhenAppend();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            btnState = ((Button)sender).Name.Substring(3);
            current = CuNo.Text.Trim();

            Basic.SetParameter.SetTxtEnableNotClear(TXTs);
            Basic.SetParameter.SetBtnEnable(SC);
            Basic.SetParameter.SetBtnDisable(Others);
            Basic.SetParameter.SetRadioEnable(pnlCuSlevel);
            Basic.SetParameter.SetRadioEnable(pnlCuSlevel1);

            //複製時，更新日期
            switch (Common.User_DateTime)
            {
                case 1:
                    CuDate.Text = Model.Date.GetDateTime(1, false);
                    CuDate_1.Text = Model.Date.GetDateTime(1, false);
                    break;
                case 2:
                    CuDate.Text = Model.Date.GetDateTime(2, false);
                    CuDate_1.Text = Model.Date.GetDateTime(2, false);
                    break;
            }
            CuPoint.Text = "0";


            //複製時，清空欄位,焦點於CuNo
            CuNo.SelectAll();
            CuNo.ReadOnly = false;
            CuNo.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            btnState = ((Button)sender).Name.Substring(3);
            if (CuNo.Text.Trim() == "") return;//CuNo沒有值，就不執行下面的指令
            current = CuNo.Text.Trim();

            Basic.SetParameter.SetTxtEnableNotClear(TXTs);
            Basic.SetParameter.SetBtnEnable(SC);
            Basic.SetParameter.SetBtnDisable(Others);
            Basic.SetParameter.SetRadioEnable(pnlCuSlevel);
            Basic.SetParameter.SetRadioEnable(pnlCuSlevel1);

            //修改時,焦點於CuNo
            CuNo.ReadOnly = false;
            CuNo.Focus();
            CuNo.SelectAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CuNo.Text == "") return;
            try
            {
                btnState = "Delete";
                if (MessageBox.Show("請確定是否刪除此筆記錄?", "確認視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel) return;

                //執行刪除指令前，檢查此筆資料是否已被別人刪除
                loadDB();
                dr = getCurrentDataRow();
                if (list.IndexOf(dr) == -1)
                {
                    if (list.Count > 0)
                    {
                        dr = list.Last();
                        writeToTxt(dr);
                    }
                    else
                    {
                        dr = null;
                        writeToTxt(dr);
                    }
                    return;//資料已被刪除，以下程式碼不執行
                }
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "delete from Cust where CuNo=N'" + CuNo.Text + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        cmd.ExecuteNonQuery();
                    }
                }

                //刪除完成重載DB
                //如果list還有值,秀出最後一筆
                //如果沒有值,清空當前欄位
                loadDB();
                if (list.Count > 0)
                {
                    dr = list.Last();
                    writeToTxt(dr);
                }
                else
                {
                    dr = null;
                    writeToTxt(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            客戶基本建檔_列印 frm = new 客戶基本建檔_列印();
            frm.SetParaeter();
            frm.ShowDialog();
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            loadDB();
            if (dt.Rows.Count > 0)
            {
                using (客戶建檔作業_瀏覽 frm = new 客戶建檔作業_瀏覽())
                {
                    frm.SetParaeter();
                    frm.SeekNo = CuNo.Text.Trim();
                    switch (frm.ShowDialog())
                    {
                        case DialogResult.OK:
                            reLoadDB(frm.Result["CuNo"].ToString());
                            break;
                        case DialogResult.Cancel: break;
                    }
                }
            }
            else
            {
                MessageBox.Show("空資料庫，請先新增", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Common.IsOverRegist("cust"))
            {
                string msg = "目前使用版權為『教育版』，超過筆數限制無法存檔！\n";
                msg += "若要解除筆數限制，請升級為『正式版』。";
                MessageBox.Show(msg, "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (CuNo.Text == string.Empty)//儲存或修改時，CuNo不能為空值
            {
                MessageBox.Show("客戶編號不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CuNo.Focus();
                return;
            }
            else
            {
                //新增，複製，修改時，折數與信用額度幫填入0值
                try
                {
                    CuDisc1.Text = Convert.ToDecimal(CuDisc1.Text).ToString();
                }
                catch { CuDisc1.Text = "0"; }
                try
                {
                    CuCredit.Text = Convert.ToDecimal(CuCredit.Text).ToString();
                }
                catch { CuCredit.Text = "0"; }
                dr = getCurrentDataRow();
            }

            if (Xa1No.Text.Trim() == "")
            {
                MessageBox.Show("幣別編號不可為空值", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Xa1No.Focus();
                return;
            }
            if (CuX3No1.Text.Trim() == "")
            {
                MessageBox.Show("營業稅編號不可為空值", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CuX3No1.Focus();
                return;
            }
            if (CuX5No1.Text.Trim() == "")
            {
                MessageBox.Show("發票模式編號不可為空值", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CuX5No1.Focus();
                return;
            }

            if (btnState == "Append")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    MessageBox.Show("此客戶編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CuNo.Text = string.Empty;
                    CuNo.Focus();
                    return;
                }
                try
                {
                    using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();

                        tran = conn.BeginTransaction();
                        cmd.Transaction = tran;

                        cmd.CommandText = "INSERT INTO Cust"
                            + "(cuno,cuname2,cuname1,cuinvoname,cuchkname"
                            + ",cuxa1no,cupareno,cucono,cuime,cux1no"
                            + ",cuemno1,cuper1,cuper2,cuper,cutel1"
                            + ",cutel2,cufax1,cuatel1,cuatel2,cuatel3"
                            + ",cubbc,cuaddr1,cur1,cuaddr2,cur2"
                            + ",cuaddr3,cur3,cuslevel,cudisc,cuemail"
                            + ",cuwww,cux2no,cuuno,cux3no,cux4no"
                            + ",cucredit,cuengname,cuengaddr,cuengr1,cumemo1"
                            + ",cumemo2,cux5no,cuarea,cuudf1,cuudf2"
                            + ",cuudf3,cuudf4,cuudf5,cuudf6,cudate"
                            + ",cudate1,cudate2,culastday,culastday1,culastday2"
                            + ",cufirrcvpar"
                            //+ ",cufirreceiv,cusparercv,cureceiv,cufiradvamt,cuadvamt"
                            + ",cunote,cubirth,cubirth1,cubirth2,CuDatep,CuDatep1,CuDatep2"
                            + ",cusex,cublood,cuidno,IsTrans,CuPoint) VALUES (N'"
                            + CuNo.Text.Trim() + "',N'" + CuName2.Text + "',N'" + CuName1.Text + "',N'" + CuInvoName.Text + "','支票抬頭',N'"
                            + Xa1No.Text + "',N'" + CuPareNo.Text + "','T',N'" + CuIme.Text + "',N'" + CuX1No.Text + "',N'"
                            + CuEmNo1.Text + "',N'" + CuPer1.Text + "',N'" + CuPer2.Text + "',N'" + CuPer.Text + "',N'" + CuTel1.Text + "',N'"
                            + CuTel2.Text + "',N'" + CuFax1.Text + "',N'" + CuAtel1.Text + "',N'" + CuAtel2.Text + "',N'" + CuAtel3.Text
                            + "','BBC',N'" + CuAddr1.Text + "',N'" + CuR1.Text + "',N'" + CuAddr2.Text + "',N'" + CuR2.Text + "',N'"
                            + CuAddr3.Text + "',N'" + CuR3.Text + "'," + getRadioNumber(pnlCuSlevel1) + "," + CuDisc1.Text + ",N'" + CuEmail.Text + "',N'"
                            + CuWww.Text + "',N'" + CuX2No.Text + "',N'" + CuUno.Text + "',N'" + CuX3No1.Text + "',N'" + CuX4No1.Text + "',"
                            + CuCredit.Text + ",N'" + CuEngname.Text + "',N'" + CuEngaddr.Text + "',N'" + CuEngr1.Text + "',N'" + CuMemo1.Text + "',N'"
                            + CuMemo2_1.Text + "',N'" + CuX5No1.Text + "',N'" + CuArea.Text + "',N'" + CuUdf1.Text + "',N'" + CuUdf2.Text + "',N'"
                            + CuUdf3.Text + "',N'" + CuUdf4.Text + "',N'" + CuUdf5.Text + "','保留',N'" + Model.Date.GetDateTime(1, false)
                            + "',N'" + Model.Date.GetDateTime(2, false) + "',N'" + Model.Date.GetDateTime(2, false) + "','','','',1"
                            //+ ",0,0,0,0,0"
                            + ",'備忘錄',N'" + Date.ToTWDate(CuBirth.Text) + "',N'" + Date.ToUSDate(CuBirth.Text) + "',N'" + CuBirth.Text
                            + "',N'" + Date.ToTWDate(CuDatep.Text) + "',N'" + Date.ToUSDate(CuDatep.Text) + "',N'" + CuDatep.Text
                            + "',N'" + CuSex.Text + "',N'" + CuBlood.Text + "',N'" + CuIdNo.Text + "',N'" + machine + "',0)";


                        cmd.ExecuteNonQuery();
                        tran.Commit(); tran.Dispose();
                        cmd.Dispose();
                    }
                    //更新current值
                    current = CuNo.Text.Trim();
                    Basic.SetParameter.TextBoxClear(TXTs);
                    Basic.SetParameter.TextBoxClear(TXTReadOnly);
                    CuNo.Focus();
                    setTxtWhenAppend();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.ToString());
                }
            }

            if (btnState == "Duplicate")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    MessageBox.Show("此客戶編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CuNo.Text = string.Empty;
                    CuNo.Focus();
                    return;
                }
                try
                {
                    using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        tran = conn.BeginTransaction();
                        cmd.Transaction = tran;

                        cmd.CommandText = "INSERT INTO Cust"
                            + "(cuno,cuname2,cuname1,cuinvoname,cuchkname"
                            + ",cuxa1no,cupareno,cucono,cuime,cux1no"
                            + ",cuemno1,cuper1,cuper2,cuper,cutel1"
                            + ",cutel2,cufax1,cuatel1,cuatel2,cuatel3"
                            + ",cubbc,cuaddr1,cur1,cuaddr2,cur2"
                            + ",cuaddr3,cur3,cuslevel,cudisc,cuemail"
                            + ",cuwww,cux2no,cuuno,cux3no,cux4no"
                            + ",cucredit,cuengname,cuengaddr,cuengr1,cumemo1"
                            + ",cumemo2,cux5no,cuarea,cuudf1,cuudf2"
                            + ",cuudf3,cuudf4,cuudf5,cuudf6,cudate"
                            + ",cudate1,cudate2,culastday,culastday1,culastday2"
                            + ",cufirrcvpar"
                            //+ ",cufirreceiv,cusparercv,cureceiv,cufiradvamt,cuadvamt"
                            + ",cunote,cubirth,cubirth1,cubirth2,CuDatep,CuDatep1,CuDatep2"
                            + ",cusex,cublood,cuidno,IsTrans,CuPoint) VALUES (N'"
                            + CuNo.Text.Trim() + "',N'" + CuName2.Text + "',N'" + CuName1.Text + "',N'" + CuInvoName.Text + "','支票抬頭',N'"
                            + Xa1No.Text + "',N'" + CuPareNo.Text + "','T',N'" + CuIme.Text + "',N'" + CuX1No.Text + "',N'"
                            + CuEmNo1.Text + "',N'" + CuPer1.Text + "',N'" + CuPer2.Text + "',N'" + CuPer.Text + "',N'" + CuTel1.Text + "',N'"
                            + CuTel2.Text + "',N'" + CuFax1.Text + "',N'" + CuAtel1.Text + "',N'" + CuAtel2.Text + "',N'" + CuAtel3.Text
                            + "','BBC',N'" + CuAddr1.Text + "',N'" + CuR1.Text + "',N'" + CuAddr2.Text + "',N'" + CuR2.Text + "',N'"
                            + CuAddr3.Text + "',N'" + CuR3.Text + "'," + getRadioNumber(pnlCuSlevel1) + "," + CuDisc1.Text + ",N'" + CuEmail.Text + "',N'"
                            + CuWww.Text + "',N'" + CuX2No.Text + "',N'" + CuUno.Text + "',N'" + CuX3No1.Text + "',N'" + CuX4No1.Text + "',"
                            + CuCredit.Text + ",N'" + CuEngname.Text + "',N'" + CuEngaddr.Text + "',N'" + CuEngr1.Text + "',N'" + CuMemo1.Text + "',N'"
                            + CuMemo2_1.Text + "',N'" + CuX5No1.Text + "',N'" + CuArea.Text + "',N'" + CuUdf1.Text + "',N'" + CuUdf2.Text + "',N'"
                            + CuUdf3.Text + "',N'" + CuUdf4.Text + "',N'" + CuUdf5.Text + "','保留',N'" + Model.Date.GetDateTime(1, false)
                            + "',N'" + Model.Date.GetDateTime(2, false) + "',N'" + Model.Date.GetDateTime(2, false) + "','','','',1"
                            //+ ",0,0,0,0,0"
                            + ",'備忘錄',N'" + Date.ToTWDate(CuBirth.Text) + "',N'" + Date.ToUSDate(CuBirth.Text) + "',N'" + CuBirth.Text
                            + "',N'" + Date.ToTWDate(CuDatep.Text) + "',N'" + Date.ToUSDate(CuDatep.Text) + "',N'" + CuDatep.Text
                            + "',N'" + CuSex.Text + "',N'" + CuBlood.Text + "',N'" + CuIdNo.Text + "',N'" + machine + "',0)";

                        cmd.ExecuteNonQuery();
                        tran.Commit(); tran.Dispose();
                        cmd.Dispose();
                    }

                    //更新current值
                    current = CuNo.Text.Trim();
                    Basic.SetParameter.TextBoxClear(TXTs);
                    Basic.SetParameter.TextBoxClear(TXTReadOnly);
                    CuNo.Focus();
                    setTxtWhenAppend();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.ToString());
                }
            }

            if (btnState == "Modify")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i == -1)
                {
                    MessageBox.Show("此筆資料已被刪除", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CuNo.Text = string.Empty;
                    CuNo.Focus();
                    return;
                }
                try
                {
                    using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        tran = conn.BeginTransaction();
                        cmd.Transaction = tran;

                        cmd.CommandText = "Update Cust set "
                          + "cuname2 =N'" + CuName2.Text.Trim() + "',"
                          + "cuname1 =N'" + CuName1.Text.Trim() + "',"
                          + "cuinvoname =N'" + CuInvoName.Text + "',"
                          + "cuxa1no =N'" + Xa1No.Text + "',"
                          + "cupareno =N'" + CuPareNo.Text + "',"
                          + "cuime =N'" + CuIme.Text + "',"
                          + "cux1no =N'" + CuX1No.Text + "',"
                          + "cuemno1 =N'" + CuEmNo1.Text + "',"
                          + "cuper1 =N'" + CuPer1.Text + "',"
                          + "cuper2 =N'" + CuPer2.Text + "',"
                          + "cuper =N'" + CuPer.Text + "',"
                          + "cutel1 =N'" + CuTel1.Text + "',"
                          + "cutel2 =N'" + CuTel2.Text + "',"
                          + "cufax1 =N'" + CuFax1.Text + "',"
                          + "cuatel1 =N'" + CuAtel1.Text + "',"
                          + "cuatel2 =N'" + CuAtel2.Text + "',"
                          + "cuatel3 =N'" + CuAtel3.Text + "',"
                          + "cuaddr1 =N'" + CuAddr1.Text + "',"
                          + "cur1 =N'" + CuR1.Text + "',"
                          + "cuaddr2 =N'" + CuAddr2.Text + "',"
                          + "cur2 =N'" + CuR2.Text + "',"
                          + "cuaddr3 =N'" + CuAddr3.Text + "',"
                          + "cur3 =N'" + CuR3.Text + "',"
                          + "cuslevel =N'" + getRadioNumber(pnlCuSlevel1) + "',"
                          + "cudisc =" + CuDisc1.Text + ","
                          + "cuemail =N'" + CuEmail.Text + "',"
                          + "cuwww =N'" + CuWww.Text + "',"
                          + "cux2no =N'" + CuX2No.Text + "',"
                          + "cuuno =N'" + CuUno.Text + "',"
                          + "cux3no =N'" + CuX3No1.Text + "',"
                          + "cux4no =N'" + CuX4No1.Text + "',"
                          + "cucredit =" + CuCredit.Text + ","
                          + "cuengname =N'" + CuEngname.Text + "',"
                          + "cuengaddr =N'" + CuEngaddr.Text + "',"
                          + "cuengr1 =N'" + CuEngr1.Text + "',"
                          + "cumemo1 =N'" + CuMemo1.Text + "',"
                          + "cumemo2 =N'" + CuMemo2_1.Text + "',"
                          + "cux5no =N'" + CuX5No1.Text + "',"
                          + "cuarea =N'" + CuArea.Text + "',"
                          + "cuudf1 =N'" + CuUdf1.Text + "',"
                          + "cuudf2 =N'" + CuUdf2.Text + "',"
                          + "cuudf3 =N'" + CuUdf3.Text + "',"
                          + "cuudf4 =N'" + CuUdf4.Text + "',"
                          + "cuudf5 =N'" + CuUdf5.Text + "',"
                          + "cubirth =N'" + Date.ToTWDate(CuBirth.Text) + "',"
                          + "cubirth1 =N'" + Date.ToUSDate(CuBirth.Text) + "',"
                          + "CuDatep =N'" + Date.ToTWDate(CuDatep.Text) + "',"
                          + "CuDatep1 =N'" + Date.ToUSDate(CuDatep.Text) + "',"
                          + "cusex =N'" + CuSex.Text + "',"
                          + "cublood =N'" + CuBlood.Text + "',"
                          + "CuPoint =" + CuPoint.Text + ","
                          + "IsTrans =N'" + machine + "',"
                          + "cuidno =N'" + CuIdNo.Text + "' where CuNo =N'"
                          + CuNo.Text + "' COLLATE Chinese_Taiwan_Stroke_BIN";

                        cmd.ExecuteNonQuery();
                        tran.Commit(); tran.Dispose();
                        cmd.Dispose();
                    }

                    //更新current值
                    current = CuNo.Text.Trim();
                    Basic.SetParameter.TextBoxClear(TXTs);
                    Basic.SetParameter.TextBoxClear(TXTReadOnly);
                    CuNo.Focus();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //取消其它動作時,清空按鈕狀態值
            //載回當前欄位
            btnState = string.Empty;
            Basic.SetParameter.SetTxtDisable(TXTs);
            Basic.SetParameter.SetBtnEnable(Others);
            Basic.SetParameter.SetBtnDisable(SC);
            Basic.SetParameter.SetRadioDisable(pnlCuSlevel1);
            //取消時，檢查預載回的資料是否已被別人刪除
            //已被刪除，改顯示最後一筆
            //若再沒資料，清空欄位
            //沒被刪除，則載回預存資料
            loadDB();
            dr = getCurrentDataRow(current);
            if (list.IndexOf(dr) == -1)
            {
                if (list.Count > 0)
                {
                    dr = list.Last();
                    writeToTxt(dr);
                }
                else
                {
                    dr = null;
                    writeToTxt(dr);
                }
            }
            else
            {
                writeToTxt(dr);
            }
            btnAppend.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }



        //其它
        private string getRadioNumber(tableLayoutPnl pnl)
        {
            string str = "";
            foreach (Control rd in pnl.Controls)
            {
                if (rd is RadioButton)
                {
                    if (((RadioButton)rd).Checked)
                        str = ((RadioButton)rd).Name.Substring(5);
                }
            }
            return str;
        }

        private void setTxtWhenAppend()
        {
            //新增一筆資料時,載入欄位預設值
            Xa1No.Text = "TWD";
            CuX3No.Text = CuX3No1.Text = CuX5No.Text = CuX5No1.Text = "1";
            CHK.Xa1No_Validating(Common.浮動連線字串,Xa1No, Xa1Name);
            CHK.X3No_Validating(Common.浮動連線字串, CuX3No, CuX3Name);
            CHK.X3No_Validating(Common.浮動連線字串, CuX3No1, CuX3Name1);
            CHK.X5No_Validating(Common.浮動連線字串, CuX5No, CuX5Name);
            CHK.X5No_Validating(Common.浮動連線字串, CuX5No1, CuX5Name1);

            CuArea.Text = "1";
            radio6.Checked = true;
            radio16.Checked = true;
            CuDisc.Text = "1.000";
            CuDisc1.Text = "1.000";


            switch (Common.User_DateTime)
            {
                case 1:
                    CuDate.Text = Model.Date.GetDateTime(1, false);
                    CuDate_1.Text = Model.Date.GetDateTime(1, false);
                    break;
                case 2:
                    CuDate.Text = Model.Date.GetDateTime(2, false);
                    CuDate_1.Text = Model.Date.GetDateTime(2, false);
                    break;
            }

        }

        private void FrmCust1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    btnAppend.PerformClick();
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    btnModify.PerformClick();
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    btnDelete.PerformClick();
                    break;
                case Keys.D4:
                    btnBrow.PerformClick();
                    break;
                case Keys.D0:
                case Keys.NumPad0:
                case Keys.F11:
                    btnExit.PerformClick();
                    break;
                case Keys.Home:
                    btnTop.PerformClick();
                    break;
                case Keys.PageUp:
                    btnPrior.PerformClick();
                    break;
                case Keys.PageDown:
                    btnNext.PerformClick();
                    break;
                case Keys.End:
                    btnBottom.PerformClick();
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



        //表頭欄位
        private void CuNo_DoubleClick(object sender, EventArgs e)
        {
            if (CuNo.ReadOnly) return;
            using (客戶建檔作業_瀏覽 frm = new 客戶建檔作業_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.CanAppend = true;
                frm.SeekNo = CuNo.Text.Trim();
                switch (frm.ShowDialog())
                {
                    case DialogResult.OK:
                        reLoadDB(frm.Result["CuNo"].ToString());
                        break;
                    case DialogResult.Cancel: break;
                }
            }
        }

        private void CuName2_Leave(object sender, EventArgs e)
        {
            if (CuName2.ReadOnly) return;
            if (CuName2.Text.Trim() != "" && CuName1.Text.Trim() == "")
            {
                CuName1.Text = CuName2.Text.GetUTF8(10);
            }
        }

        private void Xa1No_DoubleClick(object sender, EventArgs e)
        {
            CHK.Xa1No_OpemFrm(Xa1No, Xa1Name);
        }

        private void CuPareNo_DoubleClick(object sender, EventArgs e)
        {
            if (CuPareNo.ReadOnly) return;
            CHK.CuNo_OpemFrm(CuPareNo, CuPareName);
        }

        private void CuX1No_DoubleClick(object sender, EventArgs e)
        {
            CHK.X1No_OpemFrm(CuX1No, CuX1Name);
        }

        private void CuEmNo1_DoubleClick(object sender, EventArgs e)
        {
            CHK.EmNo_OpemFrm(CuEmNo1, CuEmName);
        }



        //分頁
        private void CuAddr1_DoubleClick(object sender, EventArgs e)
        {
            if (CuAddr1.ReadOnly != true)
            {
                FrmSaddr frm = new FrmSaddr();
                frm.SetParaeter(ViewMode.Normal);
                frm.callType = ((TextBox)sender).Name;
                frm.ShowDialog();
            }
        }

        private void CuX3No_DoubleClick(object sender, EventArgs e)
        {
            TextBox tx = ((TextBox)sender);
            if (tx.ReadOnly) return;
            TextBox tx2 = new TextBox();
            CHK.X3No_OpemFrm(tx, tx2);
            CuX3No.Text = tx.Text;
            CuX3No1.Text = tx.Text;
            CuX3Name.Text = tx2.Text;
            CuX3Name1.Text = tx2.Text;
        }

        private void CuX4No_DoubleClick(object sender, EventArgs e)
        {
            TextBox tx = ((TextBox)sender);
            if (tx.ReadOnly) return;
            TextBox tx2 = new TextBox();
            CHK.X4No_OpemFrm(tx, tx2);
            CuX4No.Text = tx.Text;
            CuX4No1.Text = tx.Text;
            CuX4Name.Text = tx2.Text;
            CuX4Name1.Text = tx2.Text;
        }

        private void CuX5No_DoubleClick(object sender, EventArgs e)
        {
            TextBox tx = ((TextBox)sender);
            if (tx.ReadOnly) return;
            TextBox tx2 = new TextBox();
            CHK.X5No_OpemFrm(tx, tx2);
            CuX5No.Text = tx.Text;
            CuX5No1.Text = tx.Text;
            CuX5Name.Text = tx2.Text;
            CuX5Name1.Text = tx2.Text;
        }

        private void CuArea_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)49 || e.KeyChar == (char)50 || e.KeyChar == (char)51 || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void CuX2No_DoubleClick(object sender, EventArgs e)
        {
            CHK.X2No_OpemFrm(CuX2No,CuX2Name);
        }

        private void CuSex_DoubleClick(object sender, EventArgs e)
        {
            if (CuSex.Text.Trim() == "" || CuSex.Text == "女")
                CuSex.Text = "男";
            else
                CuSex.Text = "女";
        }



        //建議售價
        private void labels_Click(object sender, EventArgs e)
        {
            if (!radio1.Enabled) return;
            Label lbl = sender as Label;
            switch (lbl.Name.Last())
            {
                case '1':
                    radio1.Checked = true;
                    radio11.Checked = true;
                    break;
                case '2':
                    radio2.Checked = true;
                    radio12.Checked = true;
                    break;
                case '3':
                    radio3.Checked = true;
                    radio13.Checked = true;
                    break;
                case '4':
                    radio4.Checked = true;
                    radio14.Checked = true;
                    break;
                case '5':
                    radio5.Checked = true;
                    radio15.Checked = true;
                    break;
                case '6':
                    radio6.Checked = true;
                    radio16.Checked = true;
                    break;
            }
        }

        private void radios_CheckedChanged(object sender, EventArgs e)
        {
            pnlCuSlevel.Controls.OfType<Label>().ToList().ForEach(l => l.BackColor = Color.Transparent);
            pnlCuSlevel1.Controls.OfType<Label>().ToList().ForEach(l => l.BackColor = Color.Transparent);

            if (radio1.Checked || radio11.Checked)
            {
                label1.BackColor = Color.LightBlue;
                label11.BackColor = Color.LightBlue;
            }
            else if (radio2.Checked || radio12.Checked)
            {
                label2.BackColor = Color.LightBlue;
                label12.BackColor = Color.LightBlue;
            }
            else if (radio3.Checked || radio13.Checked)
            {
                label3.BackColor = Color.LightBlue;
                label13.BackColor = Color.LightBlue;
            }
            else if (radio4.Checked || radio14.Checked)
            {
                label4.BackColor = Color.LightBlue;
                label14.BackColor = Color.LightBlue;
            }
            else if (radio5.Checked || radio15.Checked)
            {
                label5.BackColor = Color.LightBlue;
                label15.BackColor = Color.LightBlue;
            }
            else if (radio6.Checked || radio16.Checked)
            {
                label6.BackColor = Color.LightBlue;
                label16.BackColor = Color.LightBlue;
            }
        }

        private void CuUno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void CuNo_Enter(object sender, EventArgs e)
        {
            BeforeText = CuNo.Text;
        }

        private void CuNo_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (CuNo.Text.Trim() == "")
            {
                e.Cancel = true;
                CuNo.Text = "";
                MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (btnState == "Append")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    e.Cancel = true;
                    CuNo.Text = "";
                    MessageBox.Show("此客戶編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Duplicate")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    e.Cancel = true;
                    CuNo.Text = "";
                    MessageBox.Show("此客戶編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Modify")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    if (CuNo.Text.Trim() != BeforeText)
                    {
                        writeToTxt(dr);
                    }
                }
                else
                {
                    e.Cancel = true;
                    CuNo.SelectAll();
                    dr = getCurrentDataRow(current);
                    ////開瀏覽視窗
                    using (客戶建檔作業_瀏覽 frm = new 客戶建檔作業_瀏覽())
                    {
                        frm.SetParaeter(ViewMode.Normal);
                        frm.SeekNo = CuNo.Text.Trim();
                        frm.CanAppend = false;
                        switch (frm.ShowDialog())
                        {
                            case DialogResult.OK:
                                reLoadDB(frm.Result["CuNo"].ToString());
                                break;
                            case DialogResult.Cancel: break;
                        }
                    }
                }
            }
        }

        private void Xa1No_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;
            if (!CHK.Xa1No_Validating(Common.浮動連線字串, Xa1No, Xa1Name))
            {
                e.Cancel = true;
                CHK.Xa1No_OpemFrm(Xa1No, Xa1Name);
            }

        }

        private void CuPareNo_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;
            if (CuPareNo.Text == "")
            {
                CuPareName.Text = "";
                return;
            }
            if (!CHK.CuNo_Validating(Common.浮動連線字串,CuPareNo, CuPareName))
            {
                e.Cancel = true;
                CHK.CuNo_OpemFrm(CuPareNo, CuPareName);
            }
        }

        private void CuX1No_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;
            if (CuX1No.Text == "")
            {
                CuX1Name.Text = "";
                return;
            }
            if (!CHK.X1No_Validating(Common.浮動連線字串, CuX1No, CuX1Name))
            {
                e.Cancel = true;
                CHK.CuNo_OpemFrm(CuPareNo, CuPareName);
            }
        }

        private void CuEmNo1_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;
            if (CuEmNo1.Text == "")
            {
                CuEmName.Text = "";
                return;
            }
            if (!CHK.EmNo_Validating(Common.浮動連線字串, CuEmNo1, CuEmName))
            {
                e.Cancel = true;
                CHK.EmNo_OpemFrm(CuEmNo1, CuEmName);
            }
        }

        private void CuX3No_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;

            TextBox tx = ((TextBox)sender);
            if (tx.Text.Trim() == "")
            {
                e.Cancel = true;
                tx.Text = "";
                MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CuX3No.Text = CuX3No1.Text = tx.Text;
            if (CHK.X3No_Validating(Common.浮動連線字串, CuX3No, CuX3Name))
            {
                CHK.X3No_Validating(Common.浮動連線字串, CuX3No1, CuX3Name1);
            }
            else
            {
                e.Cancel = true;
                tx.SelectAll();
                TextBox tx2 = new TextBox();
                CHK.X3No_OpemFrm(tx, tx2);
                CuX3No.Text = tx.Text;
                CuX3No1.Text = tx.Text;
                CuX3Name.Text = tx2.Text;
                CuX3Name1.Text = tx2.Text;
            }
        }

        private void CuX4No_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;

            TextBox tx = ((TextBox)sender);
            if (tx.Text.Trim() == "")
            {
                CuX4No.Text = CuX4No1.Text = CuX4Name.Text = CuX4Name1.Text = "";
                return;
            }
            CuX4No.Text = CuX4No1.Text = tx.Text;
            if (CHK.X4No_Validating(Common.浮動連線字串, CuX4No, CuX4Name))
            {
                CHK.X4No_Validating(Common.浮動連線字串, CuX4No1, CuX4Name1);
            }
            else
            {
                e.Cancel = true;
                tx.SelectAll();
                TextBox tx2 = new TextBox();
                CHK.X4No_OpemFrm(tx, tx2);
                CuX4No.Text = tx.Text;
                CuX4No1.Text = tx.Text;
                CuX4Name.Text = tx2.Text;
                CuX4Name1.Text = tx2.Text;
            }
        }

        private void CuX5No_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;

            TextBox tx = ((TextBox)sender);
            if (tx.Text.Trim() == "")
            {
                e.Cancel = true;
                tx.Text = "";
                MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CuX5No.Text = CuX5No1.Text = tx.Text;
            if (CHK.X5No_Validating(Common.浮動連線字串, CuX5No, CuX5Name))
            {
                CHK.X5No_Validating(Common.浮動連線字串, CuX5No1, CuX5Name1);
            }
            else
            {
                e.Cancel = true;
                tx.SelectAll();
                TextBox tx2 = new TextBox();
                CHK.X5No_OpemFrm(tx, tx2);
                CuX5No.Text = tx.Text;
                CuX5No1.Text = tx.Text;
                CuX5Name.Text = tx2.Text;
                CuX5Name1.Text = tx2.Text;
            }
        }

        private void CuArea_Validating(object sender, CancelEventArgs e)
        {
            if (CuArea.ReadOnly) return;
            if (!(CuArea.Text.Trim() == "1" || CuArea.Text == "2" || CuArea.Text == "3" || CuArea.Text.Trim() == ""))
            {
                e.Cancel = true;
                CuArea.SelectAll();
                MessageBox.Show("只能輸入:\t\n\n  1 國內\n  2 國外\n  3 國內外", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CuX2No_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;
            if (!CHK.X2No_Validating(Common.浮動連線字串, CuX2No, CuX2Name))
            {
                if (CuX2No.Text == "") return;
                e.Cancel = true;
                CHK.X2No_OpemFrm(CuX2No, CuX2Name);
            }
        }

        private void CuBirth_Validating(object sender, CancelEventArgs e)
        {
            if (CuBirth.ReadOnly) return;
            if (btnCancel.Focused) return;
            if (CuBirth.Text.Trim() == "")
            {
                CuBirth.Text = "";
                return;
            }

            if (!CuBirth.IsDateTime())
            {
                e.Cancel = true;
                CuBirth.SelectAll();
                MessageBox.Show("輸入日期格式錯誤", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (Common.User_DateTime == 1)
                    CuBirth.Text = Model.Date.GetDateTime(1, false);
                else
                    CuBirth.Text = Model.Date.GetDateTime(2, false);
                return;
            }
        }

        private void CuMemo1_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            string str = (sender as TextBox).Text;
            CuMemo1.Text = str;
            CuMemo1_1.Text = str;
        }

        private void CuWww_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            string str = (sender as TextBox).Text;
            CuWww.Text = str;
            CuWww_1.Text = str;
        }

        private void CuEmail_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            string str = (sender as TextBox).Text;
            CuEmail.Text = str;
            CuEmail_1.Text = str;
        }

        private void CuR1_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            string str = (sender as TextBox).Text;
            CuR1.Text = str;
            CuR1_1.Text = str;
            if (CuR2.Text.Trim() == "") CuR2.Text = str;
            if (CuR3.Text.Trim() == "") CuR3.Text = str;
        }

        private void CuAddr2_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (CuAddr2.Text.Trim() == "") CuR2.Text = "";
            if (CuAddr3.Text.Trim() == "") CuR3.Text = "";
        }

        private void CuAddr1_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            string str = (sender as TextBox).Text;
            CuAddr1.Text = str;
            CuAddr1_1.Text = str;
            if (CuAddr1.Text.Trim() == "") CuAddr1.Text = str;
            if (CuAddr2.Text.Trim() == "") CuAddr2.Text = str;
            if (CuAddr3.Text.Trim() == "") CuAddr3.Text = str;
        }

        private void CuFax1_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            string str = (sender as TextBox).Text;
            CuFax1.Text = str;
            CuFax1_1.Text = str;
        }

        private void CuAtel1_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            string str = (sender as TextBox).Text;
            CuAtel1.Text = str;
            CuAtel1_1.Text = str;
        }

        private void CuTel1_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            string str = (sender as TextBox).Text;
            CuTel1.Text = str;
            CuTel1_1.Text = str;
        }

        private void CuPer1_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            string str = (sender as TextBox).Text;
            CuPer1.Text = str;
            CuPer1_1.Text = str;
        }

        private void CuDisc_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            string str = ((TextBox)sender).Text;
            decimal d = 0;
            decimal.TryParse(str, out d);

            if (d > 1)
            {
                e.Cancel = true;
                ((TextBox)sender).SelectAll();
                MessageBox.Show("折數設定不可大於1.000", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                CuDisc.Text = d.ToString("f" + CuDisc.NumLast);
                CuDisc1.Text = d.ToString("f" + CuDisc1.NumLast);
            }
        }

        private void CuCredit_Validating(object sender, CancelEventArgs e)
        {
            if (CuNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (btnState == "Append" || btnState == "Duplicate")
            {
                CuXbh.Text = CuCredit.Text;
            }
            if (btnState == "Modify")
            {
                loadDB();
                var row = list.Find(r => r.Field<string>("CuNo") == CuNo.Text.Trim());

                decimal d = 0, cucredit = 0, cureceiv = 0;
                decimal.TryParse(CuCredit.Text.Trim(), out cucredit);
                decimal.TryParse(row["Cureceiv"].ToString(), out cureceiv);
                d = cucredit - cureceiv;
                CuXbh.Text = d.ToString("f" + CuXbh.NumLast);
            }
        }

        private void CuDatep_Validating(object sender, CancelEventArgs e)
        {
            if (CuDatep.ReadOnly) return;
            if (btnCancel.Focused) return;
            if (CuDatep.Text.Trim().Length == 0)
            {
                CuDatep.Text = "";
                return;
            }
            if (!CuDatep.IsDateTime())
            {
                e.Cancel = true;
                MessageBox.Show("輸入日期格式錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CuDatep.SelectAll();
                return;
            }
        }






























































    }
}