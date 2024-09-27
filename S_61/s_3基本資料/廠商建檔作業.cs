using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.s_3基本資料
{
    public partial class 廠商建檔作業 : FormT
    {
        SqlTransaction tran;
        List<Control> CC = new List<Control>();
        DataTable dtXa01 = new DataTable();//幣別檔
        DataTable dtEmpl = new DataTable();//員工資料檔
        DataTable dtXX02 = new DataTable();//區域建檔
        DataTable dtXX03 = new DataTable();//稅別檔
        DataTable dtXX04 = new DataTable();//結帳類別檔
        DataTable dtXX05 = new DataTable();//發票檔
        DataRow drTemp;

        List<DataRow> list = new List<DataRow>();
        DataTable dt = new DataTable();
        DataRow dr;
        string current;
        int temp = 0;
        string btnState = string.Empty;

        List<btnT> SC = new List<btnT>();
        List<btnT> Others = new List<btnT>();
        List<Control> TXTs = new List<Control>();
        List<Control> TXTReadOnly = new List<Control>();
        List<TextBox> alltxt;
        string BeforeText;

        public 廠商建檔作業()
        {
            InitializeComponent();

            SC.Add(btnSave);
            SC.Add(btnCancel);

            Others.Add(btnTop);
            Others.Add(btnPrior);
            Others.Add(btnNext);
            Others.Add(btnBottom);
            Others.Add(btnAppend);
            Others.Add(btnDuplicate);
            Others.Add(btnModify);
            Others.Add(btnDelete);
            Others.Add(btnPrint);
            Others.Add(btnBrow);
            Others.Add(btnExit);

            TXTs.Add(FaNo);
            TXTs.Add(FaName2);
            TXTs.Add(FaName1);
            TXTs.Add(Xa1No);
            TXTs.Add(FaIme);
            TXTs.Add(FaX12No);
            TXTs.Add(FaEmNo1);
            TXTs.Add(FaPer1);
            TXTs.Add(FaTel1);
            TXTs.Add(FaAtel1);
            TXTs.Add(FaPer2);
            TXTs.Add(FaTel2);
            TXTs.Add(FaAtel2);
            TXTs.Add(FaPer);
            TXTs.Add(FaFax1);
            TXTs.Add(FaChkName);
            TXTs.Add(FaArea);
            TXTs.Add(FaTel3);
            TXTs.Add(FaAddr1);
            TXTs.Add(FaR1);
            TXTs.Add(FaAddr2);
            TXTs.Add(FaR2);
            TXTs.Add(FaAddr3);
            TXTs.Add(FaR3);
            TXTs.Add(FaCredit);
            TXTs.Add(FaUno);
            TXTs.Add(FaX3No);
            TXTs.Add(FaX4No);
            TXTs.Add(FaX5No);
            TXTs.Add(FaEmail);
            TXTs.Add(FaWww);
            TXTs.Add(FaEngname);
            TXTs.Add(FaEngr1);
            TXTs.Add(FaEngaddr);
            TXTs.Add(FaMemo1);
            TXTs.Add(FaWork);
            TXTs.Add(FaArea);
            TXTs.Add(FaX2No);
            TXTs.Add(FaUdf1);
            TXTs.Add(FaUdf2);
            TXTs.Add(FaUdf3);
            TXTs.Add(FaUdf4);
            TXTs.Add(FaUdf5);

            TXTReadOnly.Add(FaEmName);
            TXTReadOnly.Add(Xa1Name);
            TXTReadOnly.Add(FaX12Name);
            TXTReadOnly.Add(FaX2Name);
            TXTReadOnly.Add(FaX3Name);
            TXTReadOnly.Add(FaX4Name);
            TXTReadOnly.Add(FaX5Name);
            TXTReadOnly.Add(FaX12Name);
            TXTReadOnly.Add(FaXbh);
            TXTReadOnly.Add(FaDate);
            TXTReadOnly.Add(FaLastday);

            alltxt = TXTs.OrderBy(t => t.TabIndex).OfType<TextBox>().ToList();

            FaCredit.NumFirst = Common.nFirst;
            FaXbh.NumFirst = Common.nFirst;
            FaPayAmt.NumFirst = Common.nFirst;

            FaCredit.NumLast = Convert.ToInt32(Common.金額小數.ToString());
            FaXbh.NumLast = Convert.ToInt32(Common.金額小數.ToString());
            FaPayAmt.NumLast = Convert.ToInt32(Common.金額小數.ToString());
        }

        private void FrmFact_Load(object sender, EventArgs e)
        {
            Common.取得浮動連線字串(Common.使用者預設公司);
            Basic.SetParameter.TabControlItemSize(tabControl1);
            Basic.SetParameter.SetBtnEnable(Others);
            Basic.SetParameter.SetBtnDisable(SC);

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
                    string str = "select * from Fact";
                    SqlDataAdapter da = new SqlDataAdapter(str, conn);
                    dt.Clear();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        list.Clear();
                        list = dt.AsEnumerable().ToList();
                    }
                    else
                    {
                        list.Clear();
                    }

                    da = new SqlDataAdapter("select Xa1No,Xa1Name from Xa01", conn);
                    da.Fill(dtXa01);
                    da = new SqlDataAdapter("select EmNo,EmName from Empl", conn);
                    da.Fill(dtEmpl);
                    da = new SqlDataAdapter("select X2No,X2Name from XX02", conn);
                    da.Fill(dtXX02);
                    da = new SqlDataAdapter("select X3No,X3Name from XX03", conn);
                    da.Fill(dtXX03);
                    da = new SqlDataAdapter("select X4No,X4Name from XX04", conn);
                    da.Fill(dtXX04);
                    da = new SqlDataAdapter("select X5No,X5Name from XX05", conn);
                    da.Fill(dtXX05);
                    da.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void reLoadDB(string strBrow)
        {
            dr = list.Find(r => r.Field<string>("FaNo") == strBrow);
            if (dr != null)
            {
                writeToTxt(dr);
            }
        }

        private void writeToTxt(DataRow dr)
        {
            if (dr != null)
            {
                //
                //表頭欄位
                //
                FaNo.Text = dr["FaNo"].ToString();
                FaIme.Text = dr["FaIme"].ToString();
                FaName2.Text = dr["FaName2"].ToString();
                FaName1.Text = dr["FaName1"].ToString();
                //幣別編號不是空值,帶出幣別名稱
                Xa1No.Text = dr["FaXa1no"].ToString();
                if (Xa1No.Text.Trim() != "")
                {
                    drTemp = dtXa01.AsEnumerable().ToList().Find(o => o.Field<string>("Xa1No") == Xa1No.Text.Trim());
                    if (drTemp != null)
                    {
                        Xa1Name.Text = drTemp["Xa1Name"].ToString();
                    }
                }
                else Xa1Name.Text = "";
                //類別編號不是空值,帶出類別名稱
                pVar.XX12Validate(dr["FaX12no"].ToString(), FaX12No, FaX12Name);
                //業務人員編號不是空值,帶出業務名稱
                FaEmNo1.Text = dr["FaEmno1"].ToString();
                if (FaEmNo1.Text.Trim() != "")
                {
                    drTemp = dtEmpl.AsEnumerable().ToList().Find(o => o.Field<string>("EmNo") == FaEmNo1.Text.Trim());
                    if (drTemp != null)
                    {
                        FaEmName.Text = drTemp["emname"].ToString();
                    }
                }
                else FaEmName.Text = "";
                //
                //分頁一
                //
                FaPer1.Text = dr["FaPer1"].ToString();
                FaPer2.Text = dr["FaPer2"].ToString();
                FaPer.Text = dr["FaPer"].ToString();
                FaTel1.Text = dr["FaTel1"].ToString();
                FaTel2.Text = dr["FaTel2"].ToString();
                FaFax1.Text = dr["FaFax1"].ToString();
                FaAtel1.Text = dr["FaAtel1"].ToString();
                FaAtel2.Text = dr["FaAtel2"].ToString();
                FaTel3.Text = dr["FaTel3"].ToString();
                FaAddr1.Text = dr["FaAddr1"].ToString();
                FaR1.Text = dr["FaR1"].ToString();
                FaAddr2.Text = dr["FaAddr2"].ToString();
                FaR2.Text = dr["FaR2"].ToString();
                FaAddr3.Text = dr["FaAddr3"].ToString();
                FaR3.Text = dr["FaR3"].ToString();
                //現有應付帳款:dt["FaPayable"]

                //現有預付餘額:dt["FaPayAmt"]
                FaPayAmt.Text = dr["FaPayAmt"].ToString();
                decimal d = 0;
                decimal.TryParse(FaPayAmt.Text, out d);
                FaPayAmt.Text = string.Format("{0:F" + FaPayAmt.NumLast + "}", d);

                //信用額度
                FaCredit.Text = dr["FaCredit"].ToString();
                d = 0;
                decimal.TryParse(FaCredit.Text, out d);
                FaCredit.Text = string.Format("{0:F" + FaCredit.NumLast + "}", d);

                //計算信用額度的餘額：
                //餘額[FaXbh] = 信用額度[FaCredit]-現有應付帳款["FaPayable"]
                decimal d_credit = 0, d_receiv = 0;
                d = 0;
                decimal.TryParse(dr["FaCredit"].ToString(), out d_credit);
                decimal.TryParse(dr["FaPayable"].ToString(), out d_receiv);
                d = d_credit - d_receiv;
                FaXbh.Text = string.Format("{0:F" + FaXbh.NumLast + "}", d);


                FaEmail.Text = dr["FaEmail"].ToString();
                FaWww.Text = dr["FaWww"].ToString();
                FaChkName.Text = dr["FaChkName"].ToString();
                //統一編號
                FaUno.Text = dr["FaUno"].ToString();
                //營業稅編號不是空值,帶出營業稅名稱
                FaX3No.Text = dr["FaX3no"].ToString();
                if (FaX3No.Text.Trim() != "")
                {
                    drTemp = dtXX03.AsEnumerable().ToList().Find(o => o.Field<string>("X3No") == FaX3No.Text.Trim());
                    if (drTemp != null)
                    {
                        FaX3Name.Text = drTemp["X3Name"].ToString();
                    }
                }
                else FaX3Name.Text = "";
                //結帳類別編號不是空值,帶出結帳類別名稱
                FaX4No.Text = dr["FaX4no"].ToString();
                if (FaX4No.Text.Trim() != "")
                {
                    drTemp = dtXX04.AsEnumerable().ToList().Find(o => o.Field<string>("X4No") == FaX4No.Text.Trim());
                    if (drTemp != null)
                    {
                        FaX4Name.Text = drTemp["X4Name"].ToString();
                    }
                }
                else FaX4Name.Text = "";
                //發票模式編號不是空值,帶出發票模式名稱
                FaX5No.Text = dr["FaX5no"].ToString();
                if (FaX5No.Text.Trim() != "")
                {
                    drTemp = dtXX05.AsEnumerable().ToList().Find(o => o.Field<string>("X5No") == FaX5No.Text.Trim());
                    if (drTemp != null)
                    {
                        FaX5Name.Text = drTemp["X5Name"].ToString();
                    }
                }
                else
                {
                    FaX5Name.Text = "";
                }
                //
                //分頁二
                //
                FaEngname.Text = dr["FaEngname"].ToString();
                FaEngaddr.Text = dr["FaEngaddr"].ToString();
                FaEngr1.Text = dr["FaEngr1"].ToString();
                FaMemo1.Text = dr["FaMemo1"].ToString();
                FaWork.Text = dr["FaWork"].ToString();
                FaArea.Text = dr["FaArea"].ToString();
                //區域編號不是空值,帶出區域名稱
                FaX2No.Text = dr["FaX2no"].ToString();
                if (FaX2No.Text.Trim() != "")
                {
                    drTemp = dtXX02.AsEnumerable().ToList().Find(o => o.Field<string>("X2No") == FaX2No.Text.Trim());
                    if (drTemp != null)
                    {
                        FaX2Name.Text = drTemp["X2Name"].ToString();
                    }
                }
                else FaX2Name.Text = "";
                FaUdf1.Text = dr["FaUdf1"].ToString();
                FaUdf2.Text = dr["FaUdf2"].ToString();
                FaUdf3.Text = dr["FaUdf3"].ToString();
                FaUdf4.Text = dr["FaUdf4"].ToString();
                FaUdf5.Text = dr["FaUdf5"].ToString();
                FaLastday.Text = dr["FaLastday"].ToString();
                if (Common.User_DateTime == 1)
                {
                    FaDate.Text = dr["FaDate"].ToString();
                }
                else
                {
                    FaDate.Text = dr["FaDate1"].ToString();
                }
            }
            else
            {
                //dr沒有值表示資料庫裡面已經沒有資料
                //清空所有欄位
                Basic.SetParameter.TextBoxClear(TXTs);
                Basic.SetParameter.TextBoxClear(TXTReadOnly);
            }
        }

        private DataRow getCurrentDataRow()
        {
            return list.Find(o => o.Field<string>("FaNo") == (FaNo.Text.Trim()));
        }

        private DataRow getCurrentDataRow(string s)
        {
            return list.Find(o => o.Field<string>("FaNo") == (s));
        }

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
            current = FaNo.Text.Trim();
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
            current = FaNo.Text.Trim();
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
            current = FaNo.Text.Trim();

            Basic.SetParameter.SetTxtEnable(TXTs);
            Basic.SetParameter.TextBoxClear(TXTReadOnly);
            Basic.SetParameter.SetBtnEnable(SC);
            Basic.SetParameter.SetBtnDisable(Others);

            //新增時，清空欄位,焦點於FaNo
            //載入某些欄位的預設值
            FaNo.ReadOnly = false;
            FaNo.Focus();
            setTxtWhenAppend();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            btnState = ((Button)sender).Name.Substring(3);
            current = FaNo.Text.Trim();

            Basic.SetParameter.SetTxtEnableNotClear(TXTs);
            Basic.SetParameter.SetBtnEnable(SC);
            Basic.SetParameter.SetBtnDisable(Others);

            //複製時，更新日期
        
                switch (Common.User_DateTime)
                {
                    case 1:
                        FaDate.Text = Model.Date.GetDateTime(1, false);
                        break;
                    case 2:
                        FaDate.Text = Model.Date.GetDateTime(2, false);
                        break;
                }
        
            //複製時，清空欄位,焦點於FaNo
            FaNo.SelectAll();
            FaNo.ReadOnly = false;
            FaNo.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            btnState = ((Button)sender).Name.Substring(3);
            if (FaNo.Text.Trim() == "") return;//FaNo沒有值，就不執行下面的指令
            current = FaNo.Text.Trim();

            Basic.SetParameter.SetTxtEnableNotClear(TXTs);
            Basic.SetParameter.SetBtnEnable(SC);
            Basic.SetParameter.SetBtnDisable(Others);

            //修改時,焦點於FaNo
            FaNo.ReadOnly = false;
            FaNo.Focus();
            FaNo.SelectAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnState = ((Button)sender).Name.Substring(3);
            if (FaNo.Text == string.Empty) return;//FaNo沒有值，就不執行下面的指令

            string confirm = "請確定是否刪除此筆記錄?";
            if (MessageBox.Show(confirm, "確認視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                return;
            }
            else
            {
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
                try
                {
                    using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "delete from Fact where FaNo=N'"
                            + FaNo.Text + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
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
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            廠商基本建檔_列印 frm = new 廠商基本建檔_列印();
            frm.SetParaeter();
            frm.ShowDialog();
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            loadDB();
            if (dt.Rows.Count > 0)
            {
                using (廠商建檔作業_瀏覽 frm = new 廠商建檔作業_瀏覽())
                {
                    frm.SetParaeter();
                    frm.SeekNo = FaNo.Text.Trim();
                    switch (frm.ShowDialog())
                    {
                        case DialogResult.OK:
                            reLoadDB(frm.Result["FaNo"].ToString());
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
            if (!Common.IsOverRegist("fact"))
            {
                string msg = "目前使用版權為『教育版』，超過筆數限制無法存檔！\n";
                msg += "若要解除筆數限制，請升級為『正式版』。";
                MessageBox.Show(msg, "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (FaNo.Text == string.Empty)
            {
                MessageBox.Show("廠商編號不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FaNo.Focus();
                return;
            }
            else
            {
                //新增，複製，修改時，折數與信用額度幫填入0值
                try
                {
                    FaCredit.Text = Convert.ToDecimal(FaCredit.Text).ToString();
                }
                catch { FaCredit.Text = "0"; }
                dr = getCurrentDataRow();
            }
            if (Xa1No.Text.Trim() == "")
            {
                MessageBox.Show("幣別編號不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Xa1No.Focus();
                return;
            }
            if (FaX3No.Text.Trim() == "")
            {
                MessageBox.Show("營業稅不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FaX3No.Focus();
                return;
            }
            if (FaX5No.Text.Trim() == "")
            {
                MessageBox.Show("發票模式不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FaX5No.Focus();
                return;
            }

            if (btnState == "Append")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    MessageBox.Show("此廠商編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    FaNo.Text = string.Empty;
                    FaNo.Focus();
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

                        cmd.CommandText = "INSERT INTO Fact"
                            + "(FaNo,FaName2,FaName1,FaWork"
                            + ",FaXa1no,FaCono,FaIme,FaX12no"
                            + ",FaEmno1,FaPer1,FaPer2,FaPer,FaTel1"
                            + ",FaTel2,FaFax1,FaAtel1,FaAtel2,FaTel3"
                            + ",FaBbc,FaAddr1,FaR1,FaAddr2,FaR2"
                            + ",FaAddr3,FaR3,FaEmail"
                            + ",FaWww,FaX2no,FaUno,FaX3no,FaX4no"
                            + ",FaCredit,FaEngname,FaEngaddr,FaEngr1,FaMemo1"
                            + ",FaX5no,FaArea,FaUdf1,FaUdf2"
                            + ",FaUdf3,FaUdf4,FaUdf5,FaUdf6,FaDate"
                            + ",FaDate1,FaDate2,FaLastday,FaLastday1,FaLastday2"
                            + ",FaFirPayPar"
                            //+ ",FaFirPayabl,FaSparePay,FaFirPayPar,FaFirPayAmt,cuadvamt"
                            + ",FaChkName"
                            + ") VALUES (N'"
                            + FaNo.Text.Trim() + "',N'" + FaName2.Text + "',N'" + FaName1.Text + "',N'" + FaWork.Text + "',N'"
                            + Xa1No.Text + "','T',N'" + FaIme.Text + "',N'" + FaX12No.Text + "',N'"
                            + FaEmNo1.Text + "',N'" + FaPer1.Text + "',N'" + FaPer2.Text + "',N'" + FaPer.Text + "',N'" + FaTel1.Text + "',N'"
                            + FaTel2.Text + "',N'" + FaFax1.Text + "',N'" + FaAtel1.Text + "',N'" + FaAtel2.Text + "',N'" + FaTel3.Text
                            + "','BBC',N'" + FaAddr1.Text + "',N'" + FaR1.Text + "',N'" + FaAddr2.Text + "',N'" + FaR2.Text + "',N'"
                            + FaAddr3.Text + "',N'" + FaR3.Text + "',N'" + FaEmail.Text + "',N'"
                            + FaWww.Text + "',N'" + FaX2No.Text + "',N'" + FaUno.Text + "',N'" + FaX3No.Text + "',N'" + FaX4No.Text + "',"
                            + FaCredit.Text + ",N'" + FaEngname.Text + "',N'" + FaEngaddr.Text + "',N'" + FaEngr1.Text + "',N'" + FaMemo1.Text + "',N'"
                            + FaX5No.Text + "',N'" + FaArea.Text + "',N'" + FaUdf1.Text + "',N'" + FaUdf2.Text + "',N'"
                            + FaUdf3.Text + "',N'" + FaUdf4.Text + "',N'" + FaUdf5.Text + "','保留',N'" + Model.Date.GetDateTime(1, false)
                            + "',N'" + Model.Date.GetDateTime(2, false) + "',N'" + Model.Date.GetDateTime(2, false) + "','','','',1"
                            //+ ",0,0,0,0,0"
                            + ",'" + FaChkName.Text.Trim() + "')";

                        cmd.ExecuteNonQuery();
                        tran.Commit(); tran.Dispose();
                        cmd.Dispose();
                    }

                    //更新current值
                    current = FaNo.Text.Trim();
                    Basic.SetParameter.TextBoxClear(TXTs);
                    Basic.SetParameter.TextBoxClear(TXTReadOnly);
                    FaNo.Focus();
                    setTxtWhenAppend();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("AppendError:\n" + ex.ToString());
                }
            }

            if (btnState == "Duplicate")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    MessageBox.Show("此廠商編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    FaNo.Text = string.Empty;
                    FaNo.Focus();
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

                        cmd.CommandText = "INSERT INTO Fact"
                            + "(FaNo,FaName2,FaName1,FaWork"
                            + ",FaXa1no,FaCono,FaIme,FaX12no"
                            + ",FaEmno1,FaPer1,FaPer2,FaPer,FaTel1"
                            + ",FaTel2,FaFax1,FaAtel1,FaAtel2,FaTel3"
                            + ",FaBbc,FaAddr1,FaR1,FaAddr2,FaR2"
                            + ",FaAddr3,FaR3,FaEmail"
                            + ",FaWww,FaX2no,FaUno,FaX3no,FaX4no"
                            + ",FaCredit,FaEngname,FaEngaddr,FaEngr1,FaMemo1"
                            + ",FaX5no,FaArea,FaUdf1,FaUdf2"
                            + ",FaUdf3,FaUdf4,FaUdf5,FaUdf6,FaDate"
                            + ",FaDate1,FaDate2,FaLastday,FaLastday1,FaLastday2"
                            + ",FaFirPayPar"
                            //+ ",FaFirPayabl,FaSparePay,FaFirPayPar,FaFirPayAmt,cuadvamt"
                            + ",FaChkName"
                            + ") VALUES (N'"
                            + FaNo.Text.Trim() + "',N'" + FaName2.Text + "',N'" + FaName1.Text + "',N'" + FaWork.Text + "',N'"
                            + Xa1No.Text + "','T',N'" + FaIme.Text + "',N'" + FaX12No.Text + "',N'"
                            + FaEmNo1.Text + "',N'" + FaPer1.Text + "',N'" + FaPer2.Text + "',N'" + FaPer.Text + "',N'" + FaTel1.Text + "',N'"
                            + FaTel2.Text + "',N'" + FaFax1.Text + "',N'" + FaAtel1.Text + "',N'" + FaAtel2.Text + "',N'" + FaTel3.Text
                            + "','BBC',N'" + FaAddr1.Text + "',N'" + FaR1.Text + "',N'" + FaAddr2.Text + "',N'" + FaR2.Text + "',N'"
                            + FaAddr3.Text + "',N'" + FaR3.Text + "',N'" + FaEmail.Text + "',N'"
                            + FaWww.Text + "',N'" + FaX2No.Text + "',N'" + FaUno.Text + "',N'" + FaX3No.Text + "',N'" + FaX4No.Text + "',"
                            + FaCredit.Text + ",N'" + FaEngname.Text + "',N'" + FaEngaddr.Text + "',N'" + FaEngr1.Text + "',N'" + FaMemo1.Text + "',N'"
                            + FaX5No.Text + "',N'" + FaArea.Text + "',N'" + FaUdf1.Text + "',N'" + FaUdf2.Text + "',N'"
                            + FaUdf3.Text + "',N'" + FaUdf4.Text + "',N'" + FaUdf5.Text + "','保留',N'" + Model.Date.GetDateTime(1, false)
                            + "',N'" + Model.Date.GetDateTime(2, false) + "',N'" + Model.Date.GetDateTime(2, false) + "','','','',1"
                            //+ ",0,0,0,0,0"
                            + ",'" + FaChkName.Text.Trim() + "')";

                        cmd.ExecuteNonQuery();
                        tran.Commit(); tran.Dispose();
                        cmd.Dispose();
                    }

                    //更新current值
                    current = FaNo.Text.Trim();
                    Basic.SetParameter.TextBoxClear(TXTs);
                    Basic.SetParameter.TextBoxClear(TXTReadOnly);
                    FaNo.Focus();
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
                    FaNo.Text = string.Empty;
                    FaNo.Focus();
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

                        cmd.CommandText = "Update Fact set "
                          + "FaName2 =N'" + FaName2.Text.Trim() + "',"
                          + "FaName1 =N'" + FaName1.Text.Trim() + "',"
                          + "FaWork =N'" + FaWork.Text + "',"
                          + "FaXa1no =N'" + Xa1No.Text + "',"
                          + "FaIme =N'" + FaIme.Text + "',"
                          + "FaX12no =N'" + FaX12No.Text + "',"
                          + "FaEmno1 =N'" + FaEmNo1.Text + "',"
                          + "FaPer1 =N'" + FaPer1.Text + "',"
                          + "FaPer2 =N'" + FaPer2.Text + "',"
                          + "FaPer =N'" + FaPer.Text + "',"
                          + "FaTel1 =N'" + FaTel1.Text + "',"
                          + "FaTel2 =N'" + FaTel2.Text + "',"
                          + "FaFax1 =N'" + FaFax1.Text + "',"
                          + "FaAtel1 =N'" + FaAtel1.Text + "',"
                          + "FaAtel2 =N'" + FaAtel2.Text + "',"
                          + "FaTel3 =N'" + FaTel3.Text + "',"
                          + "FaAddr1 =N'" + FaAddr1.Text + "',"
                          + "FaR1 =N'" + FaR1.Text + "',"
                          + "FaAddr2 =N'" + FaAddr2.Text + "',"
                          + "FaR2 =N'" + FaR2.Text + "',"
                          + "FaAddr3 =N'" + FaAddr3.Text + "',"
                          + "FaR3 =N'" + FaR3.Text + "',"
                          + "FaEmail =N'" + FaEmail.Text + "',"
                          + "FaWww =N'" + FaWww.Text + "',"
                          + "FaX2no =N'" + FaX2No.Text + "',"
                          + "FaUno =N'" + FaUno.Text + "',"
                          + "FaX3no =N'" + FaX3No.Text + "',"
                          + "FaX4no =N'" + FaX4No.Text + "',"
                          + "FaCredit =" + FaCredit.Text + ","
                          + "FaEngname =N'" + FaEngname.Text + "',"
                          + "FaEngaddr =N'" + FaEngaddr.Text + "',"
                          + "FaEngr1 =N'" + FaEngr1.Text + "',"
                          + "FaChkName =N'" + FaChkName.Text + "',"
                          + "FaMemo1 =N'" + FaMemo1.Text + "',"
                          + "FaX5no =N'" + FaX5No.Text + "',"
                          + "FaArea =N'" + FaArea.Text + "',"
                          + "FaUdf1 =N'" + FaUdf1.Text + "',"
                          + "FaUdf2 =N'" + FaUdf2.Text + "',"
                          + "FaUdf3 =N'" + FaUdf3.Text + "',"
                          + "FaUdf4 =N'" + FaUdf4.Text + "',"
                          + "FaUdf5 =N'" + FaUdf5.Text + "' where FaNo =N'"
                          + FaNo.Text + "' COLLATE Chinese_Taiwan_Stroke_BIN";

                        cmd.ExecuteNonQuery();
                        tran.Commit(); tran.Dispose();
                        cmd.Dispose();
                    }
                    //更新current值
                    current = FaNo.Text.Trim();
                    Basic.SetParameter.TextBoxClear(TXTs);
                    Basic.SetParameter.TextBoxClear(TXTReadOnly);
                    FaNo.Focus();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("ModifyError:\n" + ex.ToString());
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
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

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
            Xa1Name.Text = "新臺幣";
            FaX3No.Text = "1";
            FaX3Name.Text = "外加稅";
            FaX5No.Text = "1";
            FaX5Name.Text = "三聯式";
            FaArea.Text = "1";


         
                switch (Common.User_DateTime)
                {
                    case 1:
                        FaDate.Text = Model.Date.GetDateTime(1, false);
                        break;
                    case 2:
                        FaDate.Text = Model.Date.GetDateTime(2, false);
                        break;
                }
            
        }



        //表頭欄位
        private void FaNo_DoubleClick(object sender, EventArgs e)
        {
            if (FaNo.ReadOnly != true)
            {
                using (廠商建檔作業_瀏覽 frm = new 廠商建檔作業_瀏覽())
                {
                    frm.SetParaeter(ViewMode.Normal);
                    frm.SeekNo = FaNo.Text.Trim();
                    switch (frm.ShowDialog())
                    {
                        case DialogResult.OK:
                            reLoadDB(frm.Result["FaNo"].ToString());
                            break;
                        case DialogResult.Cancel: break;
                    }
                }
            }
        }

        private void Xa1No_DoubleClick(object sender, EventArgs e)
        {
            CHK.Xa1No_OpemFrm(Xa1No, Xa1Name);
        }

        private void FaX12no_DoubleClick(object sender, EventArgs e)
        {
            CHK.X12No_OpemFrm(FaX12No, FaX12Name);
        }

        private void FaEmno1_DoubleClick(object sender, EventArgs e)
        {
            CHK.EmNo_OpemFrm(FaEmNo1, FaEmName);
        }



        //分頁一
        private void FaAddr1_Leave(object sender, EventArgs e)
        {
            if (FaAddr2.Text.Trim() == "") FaAddr2.Text = FaAddr1.Text;
            if (FaAddr3.Text.Trim() == "") FaAddr3.Text = FaAddr1.Text;
        }

        private void FaAddr1_DoubleClick(object sender, EventArgs e)
        {
            if (FaAddr1.ReadOnly != true)
            {
                FrmSaddr frm = new FrmSaddr();
                frm.SetParaeter(ViewMode.Normal);
                if ((sender as TextBox).Name == "FaAddr1")
                {
                    frm.callType = "FaAddr1";
                }
                if ((sender as TextBox).Name == "FaAddr2")
                {
                    frm.callType = "FaAddr2";
                }
                if ((sender as TextBox).Name == "FaAddr3")
                {
                    frm.callType = "FaAddr3";
                }
                if ((sender as TextBox).Name == "FaAddr1_1")
                {
                    frm.callType = "FaAddr1_1";
                }
                frm.ShowDialog();
            }
        }

        private void FaR1_Leave(object sender, EventArgs e)
        {
            if (FaR2.Text.Trim() == "") FaR2.Text = FaR1.Text;
            if (FaR3.Text.Trim() == "") FaR3.Text = FaR1.Text;
        }

        private void FaCredit_Leave(object sender, EventArgs e)
        {
            if (FaCredit.ReadOnly) return;
            if (FaCredit.Text.Trim() == "") return;
            try
            {
                FaCredit.Text = string.Format("{0:F" + FaCredit.NumLast + "}", Convert.ToDecimal(FaCredit.Text));
                if (btnState == "Append" || btnState == "Duplicate")
                {
                    FaXbh.Text = FaCredit.Text;
                }
                if (btnState == "Modify")
                {
                    loadDB();
                    var v = list.Find(r => r.Field<string>("FaNo") == FaNo.Text.Trim());
                    decimal d = 0, d_CuCredit = 0, d_Cureceiv = 0;
                    decimal.TryParse(FaCredit.Text.Trim(), out d_CuCredit);
                    decimal.TryParse(v["FaPayable"].ToString(), out d_Cureceiv);
                    d = d_CuCredit - d_Cureceiv;
                    FaXbh.Text = string.Format("{0:F" + FaCredit.NumLast + "}", d);
                }
            }
            catch
            {
                MessageBox.Show("只能輸入數字", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FaCredit.Focus();
                FaCredit.SelectAll();
            }
        }

        private void FaX3no_DoubleClick(object sender, EventArgs e)
        {
            CHK.X3No_OpemFrm(FaX3No,FaX3Name);
        }

        private void FaX4no_DoubleClick(object sender, EventArgs e)
        {
            CHK.X4No_OpemFrm(FaX4No, FaX4Name);
        }

        private void FaX5no_DoubleClick(object sender, EventArgs e)
        {
            CHK.X5No_OpemFrm(FaX5No, FaX5Name);
        }



        //分頁二
        private void FaArea_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)49 || e.KeyChar == (char)50 || e.KeyChar == (char)51 || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void FaX2no_DoubleClick(object sender, EventArgs e)
        {
            CHK.X2No_OpemFrm(FaX2No, FaX2Name);
        }

        private void FrmFact_KeyUp(object sender, KeyEventArgs e)
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

        private void FaUno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void FaNo_Enter(object sender, EventArgs e)
        {
            BeforeText = FaNo.Text;
        }

        private void FaNo_Validating(object sender, CancelEventArgs e)
        {
            if (FaNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (FaNo.Text.Trim() == "")
            {
                e.Cancel = true;
                FaNo.Text = "";
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
                    if (btnCancel.Focused) return;
                    e.Cancel = true;
                    FaNo.Text = "";
                    MessageBox.Show("此廠商編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    FaNo.Text = "";
                    MessageBox.Show("此廠商編號已經重複，請重新輸入");
                }
            }

            if (btnState == "Modify")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    if (FaNo.Text.Trim() != BeforeText)
                        writeToTxt(dr);
                }
                else
                {
                    e.Cancel = true;
                    FaNo.SelectAll();
                    dr = getCurrentDataRow(current);
                    //開瀏覽視窗
                    using (廠商建檔作業_瀏覽 frm = new 廠商建檔作業_瀏覽())
                    {
                        frm.SetParaeter(ViewMode.Normal);
                        frm.SeekNo = FaNo.Text.Trim();
                        switch (frm.ShowDialog())
                        {
                            case DialogResult.OK:
                                reLoadDB(frm.Result["FaNo"].ToString());
                                break;
                            case DialogResult.Cancel: break;
                        }
                    }
                }
            }
        }

        private void FaName2_Validating(object sender, CancelEventArgs e)
        {
            if (FaName2.ReadOnly) return;
            if (FaName2.Text.Trim() != "" && FaName1.Text.Trim() == "")
            {
                if (FaName2.Text.Length > 4)
                    FaName1.Text = FaName2.Text.Substring(0, 4);
                else
                    FaName1.Text = FaName2.Text;
            }
        }

        private void Xa1No_Validating(object sender, CancelEventArgs e)
        {
            if (Xa1No.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (Xa1No.Text.Trim() == "")
            {
                e.Cancel = true;
                Xa1No.Text = "";
                MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CHK.Xa1No_Validating(Common.浮動連線字串, Xa1No, Xa1Name))
            {
                e.Cancel = true;
                Xa1No.SelectAll();
                CHK.Xa1No_OpemFrm(Xa1No, Xa1Name);
            }
        }

        private void FaX12no_Validating(object sender, CancelEventArgs e)
        {
            if (FaX12No.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (FaX12No.Text.Trim() == "")
            {
                FaX12No.Text = "";
                FaX12Name.Text = "";
                return;
            }

            if (!CHK.X12No_Validating(Common.浮動連線字串, FaX12No, FaX12Name))
            {
                e.Cancel = true;
                FaX12No.SelectAll();
                CHK.X12No_OpemFrm(FaX12No, FaX12Name);
            }
        }

        private void FaEmno1_Validating(object sender, CancelEventArgs e)
        {
            if (FaEmNo1.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (FaEmNo1.Text.Trim() == "")
            {
                FaEmNo1.Text = "";
                FaEmName.Text = "";
                return;
            }

            if (!CHK.EmNo_Validating(Common.浮動連線字串,FaEmNo1, FaEmName))
            {
                e.Cancel = true;
                FaEmNo1.SelectAll();
                CHK.EmNo_OpemFrm(FaEmNo1, FaEmName);
            }
        }

        private void FaX3no_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;

            if (FaX3No.Text.Trim() == "")
            {
                e.Cancel = true;
                FaX3No.Text = "";
                MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CHK.X3No_Validating(Common.浮動連線字串, FaX3No, FaX3Name))
            {
                e.Cancel = true;
                CHK.X3No_OpemFrm(FaX3No, FaX3Name);
            }
        }

        private void FaX5no_Validating(object sender, CancelEventArgs e)
        {
            if (FaX5No.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (FaX5No.Text.Trim() == "")
            {
                e.Cancel = true;
                FaX5No.Text = "";
                MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CHK.X5No_Validating(Common.浮動連線字串, FaX5No, FaX5Name))
            {
                e.Cancel = true;
                FaX5No.SelectAll();
                CHK.X5No_OpemFrm(FaX5No, FaX5Name);
            }
        }

        private void FaX4no_Validating(object sender, CancelEventArgs e)
        {
            if (FaX4No.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (FaX4No.Text.Trim() == "")
            {
                FaX4No.Text = "";
                FaX4Name.Text = "";
                return;
            }

            if (!CHK.X4No_Validating(Common.浮動連線字串, FaX4No, FaX4Name))
            {
                e.Cancel = true;
                FaX4No.SelectAll();
                CHK.X4No_OpemFrm(FaX4No, FaX4Name);
            }
        }

        private void FaArea_Validating(object sender, CancelEventArgs e)
        {
            if (FaArea.ReadOnly) return;
            if (!(FaArea.Text.Trim() == "1" || FaArea.Text == "2" || FaArea.Text == "3" || FaArea.Text.Trim() == ""))
            {
                if (btnCancel.Focused) return;
                e.Cancel = true;
                FaArea.SelectAll();
                MessageBox.Show("只能輸入:\t\n\n  1 國內\n  2 國外\n  3 國內外", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FaX2no_Validating(object sender, CancelEventArgs e)
        {
            if (FaX2No.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (FaX2No.Text.Trim() == "")
            {
                FaX2No.Text = "";
                FaX2Name.Text = "";
                return;
            }

            if (!CHK.X2No_Validating(Common.浮動連線字串, FaX2No, FaX2Name))
            {
                e.Cancel = true;
                FaX2No.SelectAll();
                CHK.X2No_OpemFrm(FaX2No, FaX2Name);
            }
        }







    }
}