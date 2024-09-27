using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using S_61.Basic;


namespace S_61.menu
{
    public partial class MenuForm : Form
    {
        public PictureBox menuhelp = new PictureBox();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        public MenuForm()
        {
            InitializeComponent();
            MainForm.FrmMenu = this;
            tabControl1.ItemSize = new Size(0, 1);

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - (MainForm.main.Width - MainForm.main.ClientSize.Width) - 2;
            this.Height = MainForm.main.ClientSize.Height - MainForm.main.stripHeight - 6;
            this.Location = new Point(1, 1);

            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ResumeLayout(false);
            this.PerformLayout();
            this.Text = Common.name1 + "功能選單";


        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            this.Location = new Point(1, 1);
            Application.DoEvents();

            Basic.SetParameter.FontSize(tabControl1);
            menuhelp.Size = new Size(17, 17);
            this.Controls.Add(menuhelp);
            menuhelp.BackgroundImage = Properties.Resources.help;
            menuhelp.Location = new Point(10, 10);
            menuhelp.BackgroundImageLayout = ImageLayout.Stretch;
            menuhelp.Click += new System.EventHandler(this.menuhelpClick);

            tabControl1.SelectTab(0);
        }

        private void tabControl1_Resize(object sender, EventArgs e)
        {
            this.tabControl1.Region = new Region(new RectangleF(this.tabPage0.Left, this.tabPage0.Top, this.tabControl1.Width, this.tabControl1.Height));
        }

        private void MenuForm_Deactivate(object sender, EventArgs e)
        {
            this.menuhelp.Visible = false;
        }

        private void menuhelpClick(object sender, EventArgs e)
        {
            try
            {
                helpProvider1.SetHelpNavigator(this, HelpNavigator.TableOfContents);
                Help.ShowHelp(menuhelp, Application.StartupPath + @"\進銷存系統.CHM", menuhelp.Tag.ToString() + ".mht");
            }
            catch
            {
            }
            finally
            {
                IntPtr hWnd = FindWindow("HH Parent", null);
                if (hWnd != IntPtr.Zero)
                    SetWindowText(hWnd, Common.name1 + "說明手冊");
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 0)
                menuhelp.Visible = false;
            tabControl1.SelectTab(0);
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 1)
                menuhelp.Visible = false;
            tabControl1.SelectTab(1);
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 2)
                menuhelp.Visible = false;
            tabControl1.SelectTab(2);
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 3)
                menuhelp.Visible = false;
            tabControl1.SelectTab(3);
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("請確定是否離開?", "訊息視窗", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            this.Dispose();
            MainForm.main = null;
            Application.Exit();
        }

        private void MenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        //第一頁
        public void 應收票據建檔_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("應收票據建檔"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("應收票據建檔"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.應收票據建檔);
            }
            else
            {
                Basic.CHK.應收票據建檔 = new s_1單據作業.應收票據建檔();
                Basic.CHK.應收票據建檔.Name = "應收票據建檔";
                Basic.CHK.應收票據建檔.MdiParent = MainForm.main;
                Basic.CHK.應收票據建檔.MdiParaeter();
                Basic.CHK.應收票據建檔.Show();
            }
        }
        public void 應付票據建檔_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("應付票據建檔"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("應付票據建檔"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.應付票據建檔);
            }
            else
            {
                Basic.CHK.應付票據建檔 = new s_1單據作業.應付票據建檔();
                Basic.CHK.應付票據建檔.Name = "應付票據建檔";
                Basic.CHK.應付票據建檔.MdiParent = MainForm.main;
                Basic.CHK.應付票據建檔.MdiParaeter();
                Basic.CHK.應付票據建檔.Show();
            }
        }
        public void 應收票據批次託收_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("應收票據批次託收"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("應收票據批次託收"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.應收票據批次託收);
            }
            else
            {
                Basic.CHK.應收票據批次託收 = new s_1單據作業.應收票據批次託收();
                Basic.CHK.應收票據批次託收.Name = "應收票據批次託收";
                Basic.CHK.應收票據批次託收.MdiParent = MainForm.main;
                Basic.CHK.應收票據批次託收.MdiParaeter();
                Basic.CHK.應收票據批次託收.Show();
            }
        }
        public void 支票列印領取作廢_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("支票列印領取作廢"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("支票列印領取作廢"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.應收票據批次異動);
            }
            else
            {
                Basic.CHK.支票列印領取作廢 = new s_1單據作業.支票列印領取作廢();
                Basic.CHK.支票列印領取作廢.Name = "支票列印領取作廢";
                Basic.CHK.支票列印領取作廢.MdiParent = MainForm.main;
                Basic.CHK.支票列印領取作廢.MdiParaeter();
                Basic.CHK.支票列印領取作廢.Show();
            }
        }
        public void 應收票據批次異動_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("應收票據批次異動"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("應收票據批次異動"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.應收票據批次異動);
            }
            else
            {
                Basic.CHK.應收票據批次異動 = new s_1單據作業.應收票據批次異動();
                Basic.CHK.應收票據批次異動.Name = "應收票據批次異動";
                Basic.CHK.應收票據批次異動.MdiParent = MainForm.main;
                Basic.CHK.應收票據批次異動.MdiParaeter();
                Basic.CHK.應收票據批次異動.Show();
            }
        }
        public void 銀行存提款作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("銀行存提款作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("銀行存提款作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.銀行存提款作業);
            }
            else
            {
                Basic.CHK.銀行存提款作業 = new s_1單據作業.銀行存提款作業();
                Basic.CHK.銀行存提款作業.Name = "銀行存提款作業";
                Basic.CHK.銀行存提款作業.MdiParent = MainForm.main;
                Basic.CHK.銀行存提款作業.MdiParaeter();
                Basic.CHK.銀行存提款作業.Show();
            }
        }
        public void 應付票據批次異動_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("應付票據批次異動"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("應付票據批次異動"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.應付票據批次異動);
            }
            else
            {
                Basic.CHK.應付票據批次異動 = new s_1單據作業.應付票據批次異動();
                Basic.CHK.應付票據批次異動.Name = "應付票據批次異動";
                Basic.CHK.應付票據批次異動.MdiParent = MainForm.main;
                Basic.CHK.應付票據批次異動.MdiParaeter();
                Basic.CHK.應付票據批次異動.Show();
            }
        }
        public void 銀行轉帳作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("銀行轉帳作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("銀行轉帳作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.銀行轉帳作業);
            }
            else
            {
                Basic.CHK.銀行轉帳作業 = new s_1單據作業.銀行轉帳作業();
                Basic.CHK.銀行轉帳作業.Name = "銀行轉帳作業";
                Basic.CHK.銀行轉帳作業.MdiParent = MainForm.main;
                Basic.CHK.銀行轉帳作業.MdiParaeter();
                Basic.CHK.銀行轉帳作業.Show();
            }
        }
        public void 帳款匯入作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("帳款匯入作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("帳款匯入作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.帳款匯入作業);
            }
            else
            {
                Basic.CHK.帳款匯入作業 = new s_1單據作業.帳款匯入作業();
                Basic.CHK.帳款匯入作業.Name = "帳款匯入作業";
                Basic.CHK.帳款匯入作業.MdiParent = MainForm.main;
                Basic.CHK.帳款匯入作業.MdiParaeter();
                Basic.CHK.帳款匯入作業.Show();
            }
        }
        public void 帳款匯出作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("帳款匯出作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("帳款匯出作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.帳款匯出作業);
            }
            else
            {
                Basic.CHK.帳款匯出作業 = new s_1單據作業.帳款匯出作業();
                Basic.CHK.帳款匯出作業.Name = "帳款匯出作業";
                Basic.CHK.帳款匯出作業.MdiParent = MainForm.main;
                Basic.CHK.帳款匯出作業.MdiParaeter();
                Basic.CHK.帳款匯出作業.Show();
            }
        }
        //第二頁
        public void 應收票據明細_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("應收票據明細"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("應收票據明細"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.應收票據明細);
            }
            else
            {
                Basic.CHK.應收票據明細 = new s_2統計圖表.應收票據明細();
                Basic.CHK.應收票據明細.Name = "應收票據明細";
                Basic.CHK.應收票據明細.MdiParent = MainForm.main;
                Basic.CHK.應收票據明細.MdiParaeter();
                Basic.CHK.應收票據明細.Show();
            }
        }
        public void 應付票據明細_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("應付票據明細"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("應付票據明細"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.應付票據明細);
            }
            else
            {
                Basic.CHK.應付票據明細 = new s_2統計圖表.應付票據明細();
                Basic.CHK.應付票據明細.Name = "應付票據明細";
                Basic.CHK.應付票據明細.MdiParent = MainForm.main;
                Basic.CHK.應付票據明細.MdiParaeter();
                Basic.CHK.應付票據明細.Show();
            }
        }
        public void 客戶票額統計_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("客戶票額統計"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("客戶票額統計"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.客戶票額統計);
            }
            else
            {
                Basic.CHK.客戶票額統計 = new s_2統計圖表.客戶票額統計();
                Basic.CHK.客戶票額統計.Name = "客戶票額統計";
                Basic.CHK.客戶票額統計.MdiParent = MainForm.main;
                Basic.CHK.客戶票額統計.MdiParaeter();
                Basic.CHK.客戶票額統計.Show();
            }
        }
        public void 廠商票額統計_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("廠商票額統計"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("廠商票額統計"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.廠商票額統計);
            }
            else
            {
                Basic.CHK.廠商票額統計 = new s_2統計圖表.廠商票額統計();
                Basic.CHK.廠商票額統計.Name = "廠商票額統計";
                Basic.CHK.廠商票額統計.MdiParent = MainForm.main;
                Basic.CHK.廠商票額統計.MdiParaeter();
                Basic.CHK.廠商票額統計.Show();
            }
        }
        public void 應收票齡分析_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("應收票齡分析"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("應收票齡分析"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.應收票齡分析);
            }
            else
            {
                Basic.CHK.應收票齡分析 = new s_2統計圖表.應收票齡分析();
                Basic.CHK.應收票齡分析.Name = "應收票齡分析";
                Basic.CHK.應收票齡分析.MdiParent = MainForm.main;
                Basic.CHK.應收票齡分析.MdiParaeter();
                Basic.CHK.應收票齡分析.Show();
            }
        }
        public void 應付票齡分析_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("應付票齡分析"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("應付票齡分析"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.應付票齡分析);
            }
            else
            {
                Basic.CHK.應付票齡分析 = new s_2統計圖表.應付票齡分析();
                Basic.CHK.應付票齡分析.Name = "應付票齡分析";
                Basic.CHK.應付票齡分析.MdiParent = MainForm.main;
                Basic.CHK.應付票齡分析.MdiParaeter();
                Basic.CHK.應付票齡分析.Show();
            }
        }
        public void 銀行對帳報表_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("銀行對帳報表"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("銀行對帳報表"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.銀行對帳報表);
            }
            else
            {
                Basic.CHK.銀行對帳報表 = new s_2統計圖表.銀行對帳報表();
                Basic.CHK.銀行對帳報表.Name = "銀行對帳報表";
                Basic.CHK.銀行對帳報表.MdiParent = MainForm.main;
                Basic.CHK.銀行對帳報表.MdiParaeter();
                Basic.CHK.銀行對帳報表.Show();
            }
        }
        public void 銀行資金預估作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("銀行資金預估作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("銀行資金預估作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.銀行資金預估作業);
            }
            else
            {
                Basic.CHK.銀行資金預估作業 = new s_2統計圖表.銀行資金預估作業();
                Basic.CHK.銀行資金預估作業.Name = "銀行資金預估作業";
                Basic.CHK.銀行資金預估作業.MdiParent = MainForm.main;
                Basic.CHK.銀行資金預估作業.MdiParaeter();
                Basic.CHK.銀行資金預估作業.Show();
            }
        }
        //第三頁
        public void 客戶建檔作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("客戶建檔作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("客戶建檔作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.客戶建檔作業);
            }
            else
            {
                Basic.CHK.客戶建檔作業 = new s_3基本資料.客戶建檔作業();
                Basic.CHK.客戶建檔作業.Name = "客戶建檔作業";
                Basic.CHK.客戶建檔作業.MdiParent = MainForm.main;
                Basic.CHK.客戶建檔作業.MdiParaeter();
                Basic.CHK.客戶建檔作業.Show();
            }
        }
        public void 客戶類別建檔_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("客戶類別建檔"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("客戶類別建檔"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.客戶類別建檔);
            }
            else
            {
                Basic.CHK.客戶類別建檔 = new s_3基本資料.客戶類別建檔();
                Basic.CHK.客戶類別建檔.Name = "客戶類別建檔";
                Basic.CHK.客戶類別建檔.MdiParent = MainForm.main;
                Basic.CHK.客戶類別建檔.MdiParaeter();
                Basic.CHK.客戶類別建檔.Show();
            }
        }
        public void 客戶資料瀏覽_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("客戶資料瀏覽"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("客戶資料瀏覽"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.客戶資料瀏覽);
            }
            else
            {
                Basic.CHK.客戶資料瀏覽 = new s_3基本資料.客戶資料瀏覽();
                Basic.CHK.客戶資料瀏覽.Name = "客戶資料瀏覽";
                Basic.CHK.客戶資料瀏覽.MdiParent = MainForm.main;
                Basic.CHK.客戶資料瀏覽.MdiParaeter();
                Basic.CHK.客戶資料瀏覽.Show();
            }
        }
        public void 客戶郵遞標籤_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("客戶郵遞標籤"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("客戶郵遞標籤"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.客戶郵遞標籤);
            }
            else
            {
                Basic.CHK.客戶郵遞標籤 = new s_3基本資料.客戶郵遞標籤();
                Basic.CHK.客戶郵遞標籤.Name = "客戶郵遞標籤";
                Basic.CHK.客戶郵遞標籤.MdiParent = MainForm.main;
                Basic.CHK.客戶郵遞標籤.MdiParaeter();
                Basic.CHK.客戶郵遞標籤.Show();
            }
        }
        public void 廠商建檔作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("廠商建檔作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("廠商建檔作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.廠商建檔作業);
            }
            else
            {
                Basic.CHK.廠商建檔作業 = new s_3基本資料.廠商建檔作業();
                Basic.CHK.廠商建檔作業.Name = "廠商建檔作業";
                Basic.CHK.廠商建檔作業.MdiParent = MainForm.main;
                Basic.CHK.廠商建檔作業.MdiParaeter();
                Basic.CHK.廠商建檔作業.Show();
            }
        }
        public void 廠商類別建檔_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("廠商類別建檔"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("廠商類別建檔"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.廠商類別建檔);
            }
            else
            {
                Basic.CHK.廠商類別建檔 = new s_3基本資料.廠商類別建檔();
                Basic.CHK.廠商類別建檔.Name = "廠商類別建檔";
                Basic.CHK.廠商類別建檔.MdiParent = MainForm.main;
                Basic.CHK.廠商類別建檔.MdiParaeter();
                Basic.CHK.廠商類別建檔.Show();
            }
        }
        public void 廠商資料瀏覽_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("廠商資料瀏覽"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("廠商資料瀏覽"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.廠商資料瀏覽);
            }
            else
            {
                Basic.CHK.廠商資料瀏覽 = new s_3基本資料.廠商資料瀏覽();
                Basic.CHK.廠商資料瀏覽.Name = "廠商資料瀏覽";
                Basic.CHK.廠商資料瀏覽.MdiParent = MainForm.main;
                Basic.CHK.廠商資料瀏覽.MdiParaeter();
                Basic.CHK.廠商資料瀏覽.Show();
            }
        }
        public void 廠商郵遞標籤_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("廠商郵遞標籤"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("廠商郵遞標籤"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.廠商郵遞標籤);
            }
            else
            {
                Basic.CHK.廠商郵遞標籤 = new s_3基本資料.廠商郵遞標籤();
                Basic.CHK.廠商郵遞標籤.Name = "廠商郵遞標籤";
                Basic.CHK.廠商郵遞標籤.MdiParent = MainForm.main;
                Basic.CHK.廠商郵遞標籤.MdiParaeter();
                Basic.CHK.廠商郵遞標籤.Show();
            }
        }
        public void 銀行帳號建檔_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("銀行帳號建檔"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("銀行帳號建檔"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.銀行帳號建檔);
            }
            else
            {
                Basic.CHK.銀行帳號建檔 = new s_3基本資料.銀行帳號建檔();
                Basic.CHK.銀行帳號建檔.Name = "銀行帳號建檔";
                Basic.CHK.銀行帳號建檔.MdiParent = MainForm.main;
                Basic.CHK.銀行帳號建檔.MdiParaeter();
                Basic.CHK.銀行帳號建檔.Show();
            }
        }
        public void 全省銀行建檔_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("全省銀行建檔"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("全省銀行建檔"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.全省銀行建檔);
            }
            else
            {
                Basic.CHK.全省銀行建檔 = new s_3基本資料.全省銀行建檔();
                Basic.CHK.全省銀行建檔.Name = "全省銀行建檔";
                Basic.CHK.全省銀行建檔.MdiParent = MainForm.main;
                Basic.CHK.全省銀行建檔.MdiParaeter();
                Basic.CHK.全省銀行建檔.Show();
            }
        }
        public void 人員建檔作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("人員建檔作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("人員建檔作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.人員建檔作業);
            }
            else
            {
                Basic.CHK.人員建檔作業 = new s_3基本資料.人員建檔作業();
                Basic.CHK.人員建檔作業.Name = "人員建檔作業";
                Basic.CHK.人員建檔作業.MdiParent = MainForm.main;
                Basic.CHK.人員建檔作業.MdiParaeter();
                Basic.CHK.人員建檔作業.Show();
            }
        }
        public void 公司建檔作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("公司建檔作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("公司建檔作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.公司建檔作業);
            }
            else
            {
                Basic.CHK.公司建檔作業 = new s_3基本資料.公司建檔作業();
                Basic.CHK.公司建檔作業.Name = "公司建檔作業";
                Basic.CHK.公司建檔作業.MdiParent = MainForm.main;
                Basic.CHK.公司建檔作業.MdiParaeter();
                Basic.CHK.公司建檔作業.Show();
            }
        }
        public void 貨幣建檔作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("貨幣建檔作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("貨幣建檔作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.貨幣建檔作業);
            }
            else
            {
                Basic.CHK.貨幣建檔作業 = new s_3基本資料.貨幣建檔作業();
                Basic.CHK.貨幣建檔作業.Name = "貨幣建檔作業";
                Basic.CHK.貨幣建檔作業.MdiParent = MainForm.main;
                Basic.CHK.貨幣建檔作業.MdiParaeter();
                Basic.CHK.貨幣建檔作業.Show();
            }
        }
        public void 部門基本資料_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("部門基本資料"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("部門基本資料"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.部門基本資料);
            }
            else
            {
                Basic.CHK.部門基本資料 = new s_3基本資料.部門基本資料();
                Basic.CHK.部門基本資料.Name = "部門基本資料";
                Basic.CHK.部門基本資料.MdiParent = MainForm.main;
                Basic.CHK.部門基本資料.MdiParaeter();
                Basic.CHK.部門基本資料.Show();
            }
        }
        public void 職謂建檔作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("職謂建檔作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("職謂建檔作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.職謂建檔作業);
            }
            else
            {
                Basic.CHK.職謂建檔作業 = new s_3基本資料.職謂建檔作業();
                Basic.CHK.職謂建檔作業.Name = "職謂建檔作業";
                Basic.CHK.職謂建檔作業.MdiParent = MainForm.main;
                Basic.CHK.職謂建檔作業.MdiParaeter();
                Basic.CHK.職謂建檔作業.Show();
            }
        }
        public void 區域建檔作業_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("區域建檔作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("區域建檔作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.區域建檔作業);
            }
            else
            {
                Basic.CHK.區域建檔作業 = new s_3基本資料.區域建檔作業();
                Basic.CHK.區域建檔作業.Name = "區域建檔作業";
                Basic.CHK.區域建檔作業.MdiParent = MainForm.main;
                Basic.CHK.區域建檔作業.MdiParaeter();
                Basic.CHK.區域建檔作業.Show();
            }
        }
        public void 結帳類別建檔_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("結帳類別建檔"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("結帳類別建檔"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.結帳類別建檔);
            }
            else
            {
                Basic.CHK.結帳類別建檔 = new s_3基本資料.結帳類別建檔();
                Basic.CHK.結帳類別建檔.Name = "結帳類別建檔";
                Basic.CHK.結帳類別建檔.MdiParent = MainForm.main;
                Basic.CHK.結帳類別建檔.MdiParaeter();
                Basic.CHK.結帳類別建檔.Show();
            }
        }
        public void 常用片語建檔_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("常用片語建檔"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("常用片語建檔"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.常用片語建檔);
            }
            else
            {
                Basic.CHK.常用片語建檔 = new s_3基本資料.常用片語建檔();
                Basic.CHK.常用片語建檔.Name = "常用片語建檔";
                Basic.CHK.常用片語建檔.MdiParent = MainForm.main;
                Basic.CHK.常用片語建檔.MdiParaeter();
                Basic.CHK.常用片語建檔.Show();
            }
        }

        //第四頁
        public void 系統參數設定_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("系統參數設定"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("系統參數設定"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.系統參數設定);
            }
            else
            {
                Basic.CHK.系統參數設定 = new s_4系統維護.系統參數設定();
                Basic.CHK.系統參數設定.Name = "系統參數設定";
                Basic.CHK.系統參數設定.MdiParent = MainForm.main;
                Basic.CHK.系統參數設定.MdiParaeter();
                Basic.CHK.系統參數設定.Show();
            }
        }
        public void 使用者參數設定_Click(object sender, EventArgs e)
        {
            if (!Model.PowerOfPage.IsPowerful("使用者參數設定作業"))
            {
                MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Basic.SetParameter.CheckMdiChild("使用者參數設定作業"))
            {
                Basic.SetParameter.RefreshFormLocation(Basic.CHK.使用者參數設定);
            }
            else
            {
                Basic.CHK.使用者參數設定 = new s_4系統維護.使用者參數設定();
                Basic.CHK.使用者參數設定.Name = "使用者參數設定";
                Basic.CHK.使用者參數設定.MdiParent = MainForm.main;
                Basic.CHK.使用者參數設定.MdiParaeter();
                Basic.CHK.使用者參數設定.Show();
            }
        }

























































































































        





































































    }
}