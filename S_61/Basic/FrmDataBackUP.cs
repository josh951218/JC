using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace S_61.Basic
{
    public partial class FrmDataBackUP : Form
    {
        string dbPath = "";
        string dir = "";

        public FrmDataBackUP()
        {
            InitializeComponent();
        }

        private void FrmDataBackUP_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT physical_name FROM sys.database_files";
                    dbPath = cmd.ExecuteScalar().ToString();
                }
            }
            CheckBak();
        }

        private void btnBackUp_Click(object sender, EventArgs e)
        {
            btnBackUp.Enabled = false;
            btnRestore.Enabled = false;

            string[] str = Common.sqlConnString.Split(';');
            int index = str.ToList().FindIndex(s => s.Contains("Initial Catalog"));
            str[index] = "Initial Catalog=master";
            index = str.ToList().FindIndex(s => s.Contains("Application Name"));
            str[index] = "";

            string cnstr = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Length > 0)
                    cnstr += str[i] + ";";
            }

            index = cnstr.Length - 1;
            cnstr = cnstr.Remove(index);
            Common.ckTime.Enabled = false;

            try
            {
                //backup
                string no = DateTime.Now.ToString("yyyyMMdd");

                using (SqlConnection conn = new SqlConnection(cnstr))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "backup database " + Common.DatabaseName + " to disk=N'" + dir + no + "_" + Common.DatabaseName + ".bak'";
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("資料庫備份成功！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Common.ckTime.Enabled = true;
                CheckBak();
                btnBackUp.Enabled = true;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select TOP 1 * from CUST";
                        cmd.ExecuteReader();
                    }
                }
            }
            catch { }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            CheckBak();
            if (!btnRestore.Enabled) return;

            btnBackUp.Enabled = false;
            btnRestore.Enabled = false;

            string msg = "您將使用資料庫還原功能，資料庫將會回複至稍早的時間點！";
            if (MessageBox.Show(msg, "確認視窗", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                DirectoryInfo directory = new DirectoryInfo(dir);
                FileInfo[] infos = directory.GetFiles();
                List<string> strlist = new List<string>();
                for (int i = 0; i < infos.Length; i++)
                {
                    if (infos[i].Name.Contains(Common.DatabaseName + ".bak"))
                    {
                        strlist.Add(infos[i].Name + ",備份時間:" + infos[i].LastWriteTime);
                    }
                }
                string fileNfame = "";
                using (FrmBackUpList frm = new FrmBackUpList())
                {
                    frm.list = strlist;
                    frm.ShowDialog();
                    fileNfame = frm.fileName;
                }

                FileInfo info = new FileInfo(dir + fileNfame);
                msg = "您將使用" + info.LastWriteTime.ToString() + "所建立的備份檔！\n";
                msg += "確定請按    是(Y)，\n";
                msg += "如要使用最新的備份檔案，請選擇    否(N)，\n";
                msg += "並點擊『備份-資料庫』按鈕，備份最新的資料庫";
                if (MessageBox.Show(msg, "確認視窗", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    string[] str = Common.sqlConnString.Split(';');
                    int index = str.ToList().FindIndex(s => s.Contains("Initial Catalog"));
                    str[index] = "Initial Catalog=master";
                    index = str.ToList().FindIndex(s => s.Contains("Application Name"));
                    str[index] = "";

                    string cnstr = "";
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (str[i].Length > 0)
                            cnstr += str[i] + ";";
                    }

                    index = cnstr.Length - 1;
                    cnstr = cnstr.Remove(index);
                    Common.ckTime.Enabled = false;

                    try
                    {
                        //restore
                        using (SqlConnection conn = new SqlConnection(cnstr))
                        {
                            conn.Open();
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = " Alter database " + Common.DatabaseName + " Set Offline With Rollback immediate;";
                                cmd.CommandText += " restore database " + Common.DatabaseName + " from disk=N'" + dir + fileNfame + "' with replace;";
                                cmd.CommandText += " Alter database " + Common.DatabaseName + " Set Online With Rollback immediate;";
                                cmd.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("資料庫已還原。", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            Common.ckTime.Enabled = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select TOP 1 * from CUST";
                        cmd.ExecuteReader();
                    }
                }
            }
            catch { }
            btnBackUp.Enabled = true;
            btnRestore.Enabled = true;
        }

        private void CheckBak()
        {
            int index = dbPath.LastIndexOf("\\") + 1;
            dir = new string(dbPath.Take(index).ToArray());
            DirectoryInfo info = new DirectoryInfo(dir);
            if (info.GetFiles().ToList().Find(f => f.Name.Contains("_" + Common.DatabaseName + ".bak")) != null)
            {
                btnRestore.Enabled = true;
            }
            else
                btnRestore.Enabled = false;
        }









































    }
}
