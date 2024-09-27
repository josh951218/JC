using System;
using System.Windows.Forms;
using S_61.Model;
using S_61.MyControl;
using S_61.Basic;
using S_61.menu;
using System.Linq;

namespace S_61.JS
{
    public partial class JSStrip : Form
    {
        public JSStrip()
        {
            InitializeComponent();
        }
        private void JSStrip_Load(object sender, EventArgs e)
        {
        }

        //第一頁
        private void Tool應收票據建檔_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.應收票據建檔_Click(null, null);
        }
        private void Tool應收票據批次託收_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.應收票據批次託收_Click(null, null);
        }
        private void Tool應收票據批次異動_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.應收票據批次異動_Click(null, null);
        }
        private void Tool銀行存提款作業_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.銀行存提款作業_Click(null, null);
        }
        private void Tool銀行轉帳作業_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.銀行轉帳作業_Click(null, null);
        }
        private void Tool應付票據建檔_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.應付票據建檔_Click(null, null);
        }
        private void Tool支票列印領取作廢_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.支票列印領取作廢_Click(null, null);
        }
        private void Tool應付票據批次異動_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.應付票據批次異動_Click(null, null);
        }
        private void Tool帳款匯入作業_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.帳款匯入作業_Click(null, null);
        }
        private void Tool帳款匯出作業_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.帳款匯出作業_Click(null, null);
        }

        //第二頁
        private void Tool應收票據明細_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.應收票據明細_Click(null, null);
        }
        private void Tool應付票據明細_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.應付票據明細_Click(null, null);
        }
        private void Tool客戶票額統計_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.客戶票額統計_Click(null, null);
        }
        private void Tool銀行對帳報表_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.銀行對帳報表_Click(null, null);
        }
        private void Tool廠商票額統計_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.廠商票額統計_Click(null, null);
        }
        private void Tool應收票齡分析_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.應收票齡分析_Click(null, null);
        }
        private void Tool應付票齡分析_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.應付票齡分析_Click(null, null);
        }
        private void Tool銀行資金預估作業_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.銀行資金預估作業_Click(null, null);
        }
        //第三頁
        private void Tool銀行帳號建檔_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.銀行帳號建檔_Click(null, null);
        }
        private void Tool全省銀行建檔_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.全省銀行建檔_Click(null, null);
        }
        private void Tool客戶建檔作業_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.客戶建檔作業_Click(null, null);
        }
        private void Tool客戶類別建檔_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.客戶類別建檔_Click(null, null);
        }
        private void Tool客戶資料瀏覽_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.客戶資料瀏覽_Click(null, null);
        }
        private void Tool客戶郵遞標籤_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.客戶郵遞標籤_Click(null, null);
        }
        private void Tool廠商建檔作業_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.廠商建檔作業_Click(null, null);
        }
        private void Tool廠商類別建檔_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.廠商類別建檔_Click(null, null);
        }
        private void Tool廠商資料瀏覽_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.廠商資料瀏覽_Click(null, null);
        }
        private void Tool廠商郵遞標籤_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.廠商郵遞標籤_Click(null, null);
        }
        private void Tool人員建檔作業_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.人員建檔作業_Click(null, null);
        }
        private void Tool部門基本資料_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.部門基本資料_Click(null, null);
        }
        private void Tool貨幣建檔作業_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.貨幣建檔作業_Click(null, null);
        }
        private void Tool職謂建檔作業_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.職謂建檔作業_Click(null, null);
        }
        private void Tool區域建檔作業_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.區域建檔作業_Click(null, null);
        }
        private void Tool結帳類別建檔_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.結帳類別建檔_Click(null, null);
        }
        private void Tool常用片語建檔_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.常用片語建檔_Click(null, null);
        }

        //第四頁
        private void Tool系統參數設定_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.系統參數設定_Click(null, null);
        }
        private void Tool使用者參數設定_Click(object sender, EventArgs e)
        {
            MainForm.FrmMenu.使用者參數設定_Click(null, null);
        }

        private void tool資料庫備分_Click(object sender, EventArgs e)
        {
            Basic.CHK.FrmDataBackUP = new FrmDataBackUP();
            Basic.CHK.FrmDataBackUP.Name = "FrmDataBackUP";
            Basic.CHK.FrmDataBackUP.MdiParent = MainForm.main;
            Basic.CHK.FrmDataBackUP.Show();
        }














































    }
}
