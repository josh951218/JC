using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace S_61.Basic
{
    public partial class FrmBackUpList : Form
    {
        public List<string> list = new List<string>();
        public string fileName = "";

        public FrmBackUpList()
        {
            InitializeComponent();
        }

        private void FrmBackUpList_Load(object sender, EventArgs e)
        {
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    listBox1.Items.Add(list[i]);
                }
            }
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            btnSure_Click(null, null);
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            fileName = listBox1.SelectedItem.ToString();
            int index = fileName.IndexOf(",備份時間");
            fileName = new string(fileName.Take(index).ToArray());
            this.Dispose();
        }
    }
}
