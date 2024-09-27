using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using System.Runtime.InteropServices;
using System.Text;
using S_61.s_3基本資料;
using System.Net;
using Microsoft.VisualBasic;
using System.IO;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;


namespace S_61.Basic
{
    class CHK
    {
        public static Basic.FrmBackUpList FrmBackUpList;
        public static Basic.FrmDataBackUP FrmDataBackUP;
        //第一頁
        public static s_1單據作業.應收票據建檔 應收票據建檔;
        public static s_1單據作業.應付票據建檔 應付票據建檔;
        public static s_1單據作業.應收票據批次託收 應收票據批次託收;
        public static s_1單據作業.應收票據批次異動 應收票據批次異動;
        public static s_1單據作業.支票列印領取作廢 支票列印領取作廢;
        public static s_1單據作業.銀行存提款作業 銀行存提款作業;
        public static s_1單據作業.應付票據批次異動 應付票據批次異動;
        public static s_1單據作業.銀行轉帳作業 銀行轉帳作業;
        public static s_1單據作業.帳款匯入作業 帳款匯入作業;
        public static s_1單據作業.帳款匯出作業 帳款匯出作業;
        //第二頁
        public static s_2統計圖表.應收票據明細 應收票據明細;
        public static s_2統計圖表.應付票據明細 應付票據明細;
        public static s_2統計圖表.客戶票額統計 客戶票額統計;
        public static s_2統計圖表.廠商票額統計 廠商票額統計;
        public static s_2統計圖表.應收票齡分析 應收票齡分析;
        public static s_2統計圖表.應付票齡分析 應付票齡分析;
        public static s_2統計圖表.銀行對帳報表 銀行對帳報表;
        public static s_2統計圖表.銀行資金預估作業 銀行資金預估作業;
        //第三頁
        public static s_3基本資料.客戶建檔作業 客戶建檔作業;
        public static s_3基本資料.客戶類別建檔 客戶類別建檔;
        public static s_3基本資料.客戶資料瀏覽 客戶資料瀏覽;
        public static s_3基本資料.客戶郵遞標籤 客戶郵遞標籤;
        public static s_3基本資料.廠商建檔作業 廠商建檔作業;
        public static s_3基本資料.廠商類別建檔 廠商類別建檔;
        public static s_3基本資料.廠商資料瀏覽 廠商資料瀏覽;
        public static s_3基本資料.廠商郵遞標籤 廠商郵遞標籤;
        public static s_3基本資料.銀行帳號建檔 銀行帳號建檔;
        public static s_3基本資料.全省銀行建檔 全省銀行建檔;
        public static s_3基本資料.人員建檔作業 人員建檔作業;
        public static s_3基本資料.公司建檔作業 公司建檔作業;
        public static s_3基本資料.貨幣建檔作業 貨幣建檔作業;
        public static s_3基本資料.部門基本資料 部門基本資料;
        public static s_3基本資料.職謂建檔作業 職謂建檔作業;
        public static s_3基本資料.區域建檔作業 區域建檔作業;
        public static s_3基本資料.結帳類別建檔 結帳類別建檔;
        public static s_3基本資料.常用片語建檔 常用片語建檔;

        //第四頁
        public static s_4系統維護.系統參數設定 系統參數設定;
        public static s_4系統維護.使用者參數設定 使用者參數設定;


        public static void GetCoName(TextBox CoNo,TextBox Coname1=null,TextBox Coname2=null)
        {
            if (CoNo.Text.Trim() == "")
            {
                if (Coname1 != null) Coname1.Text = "";
                if (Coname2 != null) Coname2.Text = "";
                return;
            }
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "select Coname1,Coname2 from comp where cono='" + CoNo.Text.Trim() + "'";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    if (Coname1 != null) Coname1.Text = reader["Coname1"].ToString().Trim();
                    if (Coname2 != null) Coname2.Text = reader["Coname2"].ToString().Trim();
                }
                reader.Dispose(); reader.Close();
                cmd.Dispose();
            }
        }
        public static string GetCoName2(string CoNo)
        {
            if (CoNo == "") return "";
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "select coname2 from comp where cono='" + CoNo + "'";
                if (cmd.ExecuteScalar().IsNullOrEmpty()) return "";
                else return cmd.ExecuteScalar().ToString().Trim();
            }
        }
        public static void GetXa1Name(TextBox Xa1No, TextBox Xa1Name = null)
        {
            if (Xa1No.Text.Trim() == "")
            {
                if (Xa1Name != null) Xa1Name.Text = "";
                return;
            }
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "select xa1name from xa01 where xa1no='" + Xa1No.Text.Trim() + "'";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    if (Xa1Name != null) Xa1Name.Text = reader["Xa1Name"].ToString().Trim();
                }
                reader.Dispose(); reader.Close();
                cmd.Dispose();
            }
        }

        //開窗事件
        public static void CoNo_OpemFrm(TextBox CoNo, TextBox CoName1 = null, TextBox CoName2 = null, bool CanAppend = true)
        {
            if (CoNo.ReadOnly) return;
            using (公司開窗 frm = new 公司開窗())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = CoNo.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.ShowDialog();
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        CoNo.Text = frm.Result["CoNo"].ToString().Trim();
                        if (CoName1 != null) CoName1.Text = frm.Result["CoName1"].ToString().Trim();
                        if (CoName2 != null) CoName2.Text = frm.Result["CoName2"].ToString().Trim();
                        break;
                }
            }
        }
        public static void Xa1No_OpemFrm(TextBox Xa1No, TextBox Xa1Name = null, bool CanAppend = true)
        {
            if (Xa1No.ReadOnly) return;
            using (貨幣建檔作業_瀏覽 frm = new 貨幣建檔作業_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = Xa1No.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.開窗模式 = true;
                frm.ShowDialog();
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        Xa1No.Text = frm.Result["Xa1No"].ToString().Trim();
                        if (Xa1Name != null) Xa1Name.Text = frm.Result["Xa1Name"].ToString().Trim();
                        break;
                }
            }
        }
        public static void DeNo_OpemFrm(TextBox DeNo, TextBox DeName = null, bool CanAppend = true)
        {
            if (DeNo.ReadOnly) return;
            using (部門基本資料_瀏覽 frm = new 部門基本資料_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = DeNo.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.開窗模式 = true;
                frm.ShowDialog();
                if (frm.Result == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        DeNo.Text = frm.Result["DeNo"].ToString().Trim();
                        if (DeName != null) DeName.Text = frm.Result["DeName1"].ToString().Trim();
                        break;
                }
            }
        }
        public static void X6No_OpemFrm(TextBox X6No, TextBox X6Name = null, bool CanAppend = true)
        {
            if (X6No.ReadOnly) return;
            using (職謂建檔作業_瀏覽 frm = new 職謂建檔作業_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = X6No.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.開窗模式 = true;
                frm.ShowDialog();
                if (frm.Result == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        X6No.Text = frm.Result["X6No"].ToString().Trim();
                        if (X6Name != null) X6Name.Text = frm.Result["X6Name"].ToString().Trim();
                        break;
                }
            }
        }
        public static void EmNo_OpemFrm(TextBox EmNo, TextBox EmName = null, bool CanAppend = true)
        {
            if (EmNo.ReadOnly) return;
            using (人員建檔作業_瀏覽 frm = new 人員建檔作業_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = EmNo.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.ShowDialog();
                if (frm.Result == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        EmNo.Text = frm.Result["EmNo"].ToString().Trim();
                        if (EmName != null) EmName.Text = frm.Result["EmName"].ToString().Trim();
                        break;
                }
            }
        }
        public static void CuNo_OpemFrm(TextBox CuNo, TextBox CuName1 = null, TextBox CuName2 = null, bool CanAppend = true)
        {
            if (CuNo.ReadOnly) return;
            using (客戶建檔作業_瀏覽 frm = new 客戶建檔作業_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = CuNo.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.ShowDialog();
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        CuNo.Text = frm.Result["CuNo"].ToString().Trim();
                        if (CuName1 != null) CuName1.Text = frm.Result["CuName1"].ToString().Trim();
                        if (CuName2 != null) CuName2.Text = frm.Result["CuName2"].ToString().Trim();
                        break;
                }
            }
        }
        public static void FaNo_OpemFrm(TextBox FaNo, TextBox FaName1 = null, TextBox FaName2 = null, bool CanAppend = true)
        {
            if (FaNo.ReadOnly) return;
            using (廠商建檔作業_瀏覽 frm = new 廠商建檔作業_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = FaNo.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.ShowDialog();
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        FaNo.Text = frm.Result["FaNo"].ToString().Trim();
                        if (FaName1 != null) FaName1.Text = frm.Result["FaName1"].ToString().Trim();
                        if (FaName2 != null) FaName2.Text = frm.Result["FaName2"].ToString().Trim();
                        break;
                }
            }
        }
        public static void X3No_OpemFrm(TextBox X3No, TextBox X3Name = null)
        {
            if (X3No.ReadOnly) return;
            using (FrmXX03Brow frm = new FrmXX03Brow())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = X3No.Text.Trim();
                frm.ShowDialog();
                if (frm.Result == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        X3No.Text = frm.Result["X3No"].ToString().Trim();
                        if (X3Name != null) X3Name.Text = frm.Result["X3Name"].ToString().Trim();
                        break;
                }
            }
        }
        public static void X4No_OpemFrm(TextBox X4No, TextBox X4Name = null, bool CanAppend = true)
        {
            if (X4No.ReadOnly) return;
            using (結帳類別建檔_瀏覽 frm = new 結帳類別建檔_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = X4No.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.開窗模式 = true;
                frm.ShowDialog();
                if (frm.Result == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        X4No.Text = frm.Result["X4No"].ToString().Trim();
                        if (X4Name != null) X4Name.Text = frm.Result["X4Name"].ToString().Trim();
                        break;
                }
            }
        }
        public static void X5No_OpemFrm(TextBox X5No, TextBox X5Name = null)
        {
            if (X5No.ReadOnly) return;
            using (FrmXX05Brow frm = new FrmXX05Brow())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = X5No.Text.Trim();
                frm.ShowDialog();
                if (frm.Result == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        X5No.Text = frm.Result["X5No"].ToString().Trim();
                        if (X5Name != null) X5Name.Text = frm.Result["X5Name"].ToString().Trim();
                        break;
                }
            }
        }
        public static void X2No_OpemFrm(TextBox X2No, TextBox X2Name = null, bool CanAppend = true)
        {
            if (X2No.ReadOnly) return;
            using (區域建檔作業_瀏覽 frm = new 區域建檔作業_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = X2No.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.開窗模式 = true;
                frm.ShowDialog();
                if (frm.Result == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        X2No.Text = frm.Result["X2No"].ToString().Trim();
                        if (X2Name != null) X2Name.Text = frm.Result["X2Name"].ToString().Trim();
                        break;
                }
            }
        }
        public static void X1No_OpemFrm(TextBox X1No, TextBox X1Name = null, bool CanAppend = true)
        {
            if (X1No.ReadOnly) return;
            using (客戶類別建檔_瀏覽 frm = new 客戶類別建檔_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = X1No.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.開窗模式 = true;
                frm.ShowDialog();
                if (frm.Result == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        X1No.Text = frm.Result["X1No"].ToString().Trim();
                        if (X1Name != null) X1Name.Text = frm.Result["X1Name"].ToString().Trim();
                        break;
                }
            }
        }
        public static void X12No_OpemFrm(TextBox X12No, TextBox X12Name = null, bool CanAppend = true)
        {
            if (X12No.ReadOnly) return;
            using (廠商類別建檔_瀏覽 frm = new 廠商類別建檔_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = X12No.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.開窗模式 = true;
                frm.ShowDialog();
                if (frm.Result == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        X12No.Text = frm.Result["X12No"].ToString().Trim();
                        if (X12Name != null) X12Name.Text = frm.Result["X12Name"].ToString().Trim();
                        break;
                }
            }
        }
        public static void BaNo_OpemFrm(TextBox BaNo, TextBox BaName = null, bool CanAppend = true)
        {
            if (BaNo.ReadOnly) return;
            using (全省銀行建檔_瀏覽 frm = new 全省銀行建檔_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = BaNo.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.開窗模式 = true;
                frm.ShowDialog();
                if (frm.Result == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        BaNo.Text = frm.Result["BaNo"].ToString().Trim();
                        if (BaName != null) BaName.Text = frm.Result["BaName"].ToString().Trim();
                        break;
                }
            }
        }
        public static void Memo_OpemFrm(TextBox Memo)
        {
            if (Memo.ReadOnly) return;
            using (備註開窗 frm = new 備註開窗())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = Memo.Text.Trim();
                frm.ShowDialog();
                if (frm.Memo == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        Memo.Text = frm.Memo;
                        break;
                }
            }
        }
        public static void AcNo_OpemFrm(string CoNo,TextBox AcNo, TextBox AcName1 = null, TextBox AcName = null, bool CanAppend = true,bool 去除外幣帳戶 = false)
        {
            if (AcNo.ReadOnly) return;
            using (銀行帳號建檔_瀏覽 frm = new 銀行帳號建檔_瀏覽())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.SeekNo = AcNo.Text.Trim();
                frm.CanAppend = CanAppend;
                frm.開窗模式 = true;
                frm.去除外幣帳戶 = 去除外幣帳戶;
                frm.CoNo = CoNo;
                frm.ShowDialog();
                if (frm.Result == null) return;
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        AcNo.Text = frm.Result["AcNo"].ToString().Trim();
                        if (AcName1 != null) AcName1.Text = frm.Result["AcName1"].ToString().Trim();
                        if (AcName != null) AcName.Text = frm.Result["AcName"].ToString().Trim();
                        break;
                }
            }
        }

        //驗證事件
        public static bool CoNo_Validating(TextBox CoNo, TextBox CoName1 = null, TextBox CoName2 = null)
        {
            if (CoNo.ReadOnly) return true;
            if (CoNo.Text.Trim() == "")
            {
                if (CoName1 != null)CoName1.Text = "";
                if (CoName2 != null)CoName2.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from comp where cono='" + CoNo.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (CoName1 != null) CoName1.Text = reader["CoName1"].ToString().Trim();
                        if (CoName2 != null) CoName2.Text = reader["CoName2"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool Xa1No_Validating(string 浮動連線字串,TextBox Xa1No, TextBox Xa1Name = null)
        {
            if (Xa1No.ReadOnly) return true;
            if (Xa1No.Text.Trim() == "")
            {
                if (Xa1Name != null) Xa1Name.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from xa01 where xa1no='" + Xa1No.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (Xa1Name != null) Xa1Name.Text = reader["Xa1Name"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool DeNo_Validating(string 浮動連線字串, TextBox DeNo, TextBox DeName = null)
        {
            if (DeNo.ReadOnly) return true;
            if (DeNo.Text.Trim() == "")
            {
                if (DeName != null) DeName.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from dept where deno='" + DeNo.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (DeName != null) DeName.Text = reader["DeName1"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool X6No_Validating(string 浮動連線字串,TextBox X6No, TextBox X6Name = null)
        {
            if (X6No.ReadOnly) return true;
            if (X6No.Text.Trim() == "")
            {
                if (X6Name != null) X6Name.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from xx06 where x6no='" + X6No.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (X6Name != null) X6Name.Text = reader["X6Name"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool CuNo_Validating(string 浮動連線字串, TextBox CuNo, TextBox CuName1 = null, TextBox CuName2 = null)
        {
            if (CuNo.ReadOnly) return true;
            if (CuNo.Text.Trim() == "")
            {
                if (CuName1 != null) CuName1.Text = "";
                if (CuName2 != null) CuName2.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from cust where cuno='" + CuNo.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (CuName1 != null) CuName1.Text = reader["CuName1"].ToString().Trim();
                        if (CuName2 != null) CuName2.Text = reader["CuName2"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool FaNo_Validating(string 浮動連線字串, TextBox FaNo, TextBox FaName1 = null, TextBox FaName2 = null)
        {
            if (FaNo.ReadOnly) return true;
            if (FaNo.Text.Trim() == "")
            {
                if (FaName1 != null) FaName1.Text = "";
                if (FaName2 != null) FaName2.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from fact where FaNo='" + FaNo.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (FaName1 != null) FaName1.Text = reader["FaName1"].ToString().Trim();
                        if (FaName2 != null) FaName2.Text = reader["FaName2"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool EmNo_Validating(string 浮動連線字串, TextBox EmNo, TextBox EmName = null)
        {
            if (EmNo.ReadOnly) return true;
            if (EmNo.Text.Trim() == "")
            {
                if (EmName != null) EmName.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from empl where EmNo='" + EmNo.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (EmName != null) EmName.Text = reader["EmName"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool X3No_Validating(string 浮動連線字串, TextBox X3No, TextBox X3Name = null)
        {
            if (X3No.ReadOnly) return true;
            if (X3No.Text.Trim() == "")
            {
                if (X3Name != null) X3Name.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from xx03 where X3No='" + X3No.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (X3Name != null) X3Name.Text = reader["X3Name"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool X4No_Validating(string 浮動連線字串, TextBox X4No, TextBox X4Name = null)
        {
            if (X4No.ReadOnly) return true;
            if (X4No.Text.Trim() == "")
            {
                if (X4Name != null) X4Name.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from xx04 where X4No='" + X4No.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (X4Name != null) X4Name.Text = reader["X4Name"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool X5No_Validating(string 浮動連線字串, TextBox X5No, TextBox X5Name = null)
        {
            if (X5No.ReadOnly) return true;
            if (X5No.Text.Trim() == "")
            {
                if (X5Name != null) X5Name.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from xx05 where X5No='" + X5No.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (X5Name != null) X5Name.Text = reader["X5Name"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool X2No_Validating(string 浮動連線字串, TextBox X2No, TextBox X2Name = null)
        {
            if (X2No.ReadOnly) return true;
            if (X2No.Text.Trim() == "")
            {
                if (X2Name != null) X2Name.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from xx02 where X2No='" + X2No.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (X2Name != null) X2Name.Text = reader["X2Name"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool X1No_Validating(string 浮動連線字串, TextBox X1No, TextBox X1Name = null)
        {
            if (X1No.ReadOnly) return true;
            if (X1No.Text.Trim() == "")
            {
                if (X1Name != null) X1Name.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from xx01 where X1No='" + X1No.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (X1Name != null) X1Name.Text = reader["X1Name"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool X12No_Validating(string 浮動連線字串, TextBox X12No, TextBox X12Name = null)
        {
            if (X12No.ReadOnly) return true;
            if (X12No.Text.Trim() == "")
            {
                if (X12Name != null) X12Name.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from xx12 where X12No='" + X12No.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (X12Name != null) X12Name.Text = reader["X12Name"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool BaNo_Validating(TextBox BaNo, TextBox BaName = null)
        {
            if (BaNo.ReadOnly) return true;
            if (BaNo.Text.Trim() == "")
            {
                if (BaName != null) BaName.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select * from bank where bano='" + BaNo.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (BaName != null) BaName.Text = reader["BaName"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
        public static bool AcNo_Validating(string CoNo, TextBox AcNo, TextBox AcName1 = null, TextBox AcName = null, bool 去除外幣帳戶 = false)
        {
            if (AcNo.ReadOnly) return true;
            if (AcNo.Text.Trim() == "")
            {
                if (AcName1 != null) AcName1.Text = "";
                if (AcName != null) AcName.Text = "";
                return false;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();

                    cmd.CommandText = "select * from acct where acno='" + AcNo.Text.Trim() + "'";
                    if (去除外幣帳戶) cmd.CommandText += " and ackind=1";
                    cmd.CommandText += " and cono='" + CoNo + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (AcName1 != null) AcName1.Text = reader["AcName1"].ToString().Trim();
                        if (AcName != null) AcName.Text = reader["AcName"].ToString().Trim();
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }

        //銀行牌告匯率
        public static DataTable 銀行牌告匯率dt = new DataTable();
        public static string 獲得台灣銀行牌告匯率文字檔位址()
        {
            try
            {
                string url = "http://rate.bot.com.tw/Pages/Static/UIP003.zh-TW.htm";
                WebRequest request = WebRequest.Create(url);
                WebResponse response5;
                response5 = request.GetResponse();
                //宣告WebRequest物件 
                WebRequest myRequest5 = WebRequest.Create(url);
                //取得網頁資訊流 
                WebResponse myResponse5 = myRequest5.GetResponse();
                //宣告StreamReader讀取資料 
                StreamReader sr5 = new StreamReader(myResponse5.GetResponseStream(), System.Text.Encoding.Default);
                //資料讀取到字串變數中 
                String content5 = sr5.ReadToEnd();
                //關閉StreamReader 
                sr5.Close();
                //關閉連線 
                myResponse5.Close();

                //WebClient wc = new WebClient();
                //wc.DownloadFile("http://blog.darkthread.net/images/darkthreadbanner.gif", 
                //"b:\\darkthread.gif");
                content5 = content5.Substring(0, content5.IndexOf("'\" value=\"下載文字檔"));
                content5 = content5.Substring(content5.IndexOf(@"href='/Pages/"));
                content5 = "http://rate.bot.com.tw" + content5.Replace("amp;", "").Replace("href='", "");
                return content5;
            }
            catch { }
            return "";
        }
        public static void 獲得台灣銀行牌告匯率()
        {
            try
            {
                WebClient wc = new WebClient();
                wc.DownloadFile(獲得台灣銀行牌告匯率文字檔位址(),
                "temp\\台灣銀行公告匯率" + Date.GetDateTime(2, false) + ".txt");
                銀行牌告匯率dt = new DataTable();
                銀行牌告匯率dt.Columns.Add("幣別編號");
                銀行牌告匯率dt.Columns.Add("匯率");
                DataRow dr = 銀行牌告匯率dt.NewRow();
                dr["幣別編號"] = "TWD";
                dr["匯率"] = "1";
                銀行牌告匯率dt.Rows.Add(dr);
                String line;
                StreamReader sr = new StreamReader("temp\\台灣銀行公告匯率" + Date.GetDateTime(2, false) + ".txt");
                line = sr.ReadLine();

                while (line != null)
                {
                    dr = 銀行牌告匯率dt.NewRow();
                    while (line.IndexOf("  ") != -1)
                    {
                        line = line.Replace("  ", " ");
                    }
                    if (line.Split(' ')[0] != "幣別")
                    {
                        dr["幣別編號"] = line.Split(' ')[1];
                        dr["匯率"] = line.Split(' ')[3];
                        try
                        {
                            銀行牌告匯率dt.Rows.Add(dr);
                        }
                        catch { }
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch { }
        }

        //稽核會計年度
        public static bool 稽核會計年度(string str)
        {
            if (str.Length == 7)
            {
                if (str.Substring(0, 3).ToDecimal() < Common.系統民國.ToDecimal())
                {
                    MessageBox.Show("輸入日期不可小於『系統票據年度』", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                    return true;
            }
            else if(str.Length == 8)
            {
                if (str.Substring(0, 4).ToDecimal() < Common.系統西元.ToDecimal())
                {
                    MessageBox.Show("輸入日期不可小於『系統票據年度』", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                    return true;
            }
            return false;
        }
    }
}
